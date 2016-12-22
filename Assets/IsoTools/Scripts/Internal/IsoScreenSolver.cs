using UnityEngine;
using IsoTools.Internal;
using System.Collections.Generic;

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace IsoTools {
	public class IsoScreenSolver {
		public Vector2                 minXY       = Vector2.zero;
		public IsoAssocList<IsoObject> curVisibles = new IsoAssocList<IsoObject>();
		public IsoAssocList<IsoObject> oldVisibles = new IsoAssocList<IsoObject>();

		class Sector {
			public IsoList<IsoObject> objects = new IsoList<IsoObject>();
			public void Reset() {
				objects.Clear();
			}
		}

		IsoList<Sector> _sectors            = new IsoList<Sector>();
		float           _sectorsSize        = 0.0f;
		Vector2         _sectorsMinNumPos   = Vector2.zero;
		Vector2         _sectorsMaxNumPos   = Vector2.zero;
		Vector2         _sectorsNumPosCount = Vector2.zero;

		List<Renderer>  _tmpRenderers       = new List<Renderer>();

		// ---------------------------------------------------------------------
		//
		// Callbacks
		//
		// ---------------------------------------------------------------------

		public void OnAddInstance(IsoObject iso_object) {
		}

		public void OnRemoveInstance(IsoObject iso_object) {
			oldVisibles.Remove(iso_object);
			curVisibles.Remove(iso_object);
			ClearIsoObjectDepends(iso_object);
		}

		public bool OnMarkDirtyInstance(IsoObject iso_object) {
			if ( !iso_object.Internal.Dirty && curVisibles.Contains(iso_object) ) {
				iso_object.Internal.Dirty = true;
				return true;
			}
			return false;
		}

		// ---------------------------------------------------------------------
		//
		// Functions
		//
		// ---------------------------------------------------------------------

		public void StepSortingAction(IsoWorld iso_world, IsoAssocList<IsoObject> instances) {
			Profiler.BeginSample("ResolveNewVisibles");
			ResolveNewVisibles(instances);
			Profiler.EndSample();
			Profiler.BeginSample("ResolveSectors");
			ResolveSectors(iso_world);
			Profiler.EndSample();
		}

		public void PostStepSortingAction() {
			_tmpRenderers.Clear();
		}

		public void Clear() {
			curVisibles.Clear();
			oldVisibles.Clear();
			_sectors.Clear();
		}

		// ---------------------------------------------------------------------
		//
		//
		//
		// ---------------------------------------------------------------------

		void ResolveNewVisibles(IsoAssocList<IsoObject> instances) {
			oldVisibles.Clear();
			if ( instances.Count > 0 ) {
				minXY.Set(float.MaxValue, float.MaxValue);
				for ( int i = 0, e = instances.Count; i < e; ++i ) {
					var iso_object = instances[i];
					var object_pos = iso_object.position;
					if ( minXY.x > object_pos.x ) {
						minXY.x = object_pos.x;
					}
					if ( minXY.y > object_pos.y ) {
						minXY.y = object_pos.y;
					}
					if ( !IsoUtils.Vec2Approximately(
						iso_object.Internal.LastTrans,
						iso_object.Internal.Transform.position) )
					{
						iso_object.FixIsoPosition();
					}
					if ( IsIsoObjectVisible(iso_object) ) {
						iso_object.Internal.Placed = false;
						oldVisibles.Add(iso_object);
					}
				}
			} else {
				minXY.Set(0.0f, 0.0f);
			}
			var temp_visibles = curVisibles;
			curVisibles = oldVisibles;
			oldVisibles = temp_visibles;
		}

		void ResolveSectors(IsoWorld iso_world) {
			SetupSectorSize(iso_world);
			SetupObjectsSectors();
			SetupSectors();
		}

		// ---------------------------------------------------------------------
		//
		// ResolveSectors
		//
		// ---------------------------------------------------------------------

		void SetupSectorSize(IsoWorld iso_world) {
			_sectorsSize = 0.0f;
			for ( int i = 0, e = curVisibles.Count; i < e; ++i ) {
				var iso_internal = curVisibles[i].Internal;
				_sectorsSize += IsoUtils.Vec2MaxF(iso_internal.ScreenBounds.size);
			}
			var min_sector_size_xy = iso_world.IsoToScreen(IsoUtils.vec3OneXY) - iso_world.IsoToScreen(Vector3.zero);
			var min_sector_size    = Mathf.Max(min_sector_size_xy.x, min_sector_size_xy.y);
			_sectorsSize = curVisibles.Count > 0
				? Mathf.Round(Mathf.Max(min_sector_size, _sectorsSize / curVisibles.Count))
				: min_sector_size;
		}

		void SetupObjectsSectors() {
			if ( curVisibles.Count > 0 ) {
				_sectorsMinNumPos.Set(float.MaxValue, float.MaxValue);
				_sectorsMaxNumPos.Set(float.MinValue, float.MinValue);
				for ( int i = 0, e = curVisibles.Count; i < e; ++i ) {
					var iso_internal = curVisibles[i].Internal;

					// high performance tricks
					var min_x = iso_internal.ScreenBounds.x.min / _sectorsSize;
					var min_y = iso_internal.ScreenBounds.y.min / _sectorsSize;
					var max_x = iso_internal.ScreenBounds.x.max / _sectorsSize;
					var max_y = iso_internal.ScreenBounds.y.max / _sectorsSize;
					iso_internal.MinSector.x = (int)(min_x >= 0.0f ? min_x : min_x - 1.0f);
					iso_internal.MinSector.y = (int)(min_y >= 0.0f ? min_y : min_y - 1.0f);
					iso_internal.MaxSector.x = (int)(max_x >= 0.0f ? max_x + 1.0f : max_x);
					iso_internal.MaxSector.y = (int)(max_y >= 0.0f ? max_y + 1.0f : max_y);
					if ( _sectorsMinNumPos.x > iso_internal.MinSector.x ) {
						_sectorsMinNumPos.x = iso_internal.MinSector.x;
					}
					if ( _sectorsMinNumPos.y > iso_internal.MinSector.y ) {
						_sectorsMinNumPos.y = iso_internal.MinSector.y;
					}
					if ( _sectorsMaxNumPos.x < iso_internal.MaxSector.x ) {
						_sectorsMaxNumPos.x = iso_internal.MaxSector.x;
					}
					if ( _sectorsMaxNumPos.y < iso_internal.MaxSector.y ) {
						_sectorsMaxNumPos.y = iso_internal.MaxSector.y;
					}
				}
			} else {
				_sectorsMinNumPos.Set(0.0f, 0.0f);
				_sectorsMaxNumPos.Set(_sectorsSize, _sectorsSize);
			}
			_sectorsNumPosCount = _sectorsMaxNumPos - _sectorsMinNumPos;
		}

		void ResizeSectors(int count) {
			if ( _sectors.Count < count ) {
				if ( _sectors.Capacity < count ) {
					_sectors.Capacity = count;
				}
				while ( _sectors.Count < _sectors.Capacity ) {
					_sectors.Add(new Sector());
				}
			}
			for ( int i = 0, e = _sectors.Count; i < e; ++i ) {
				_sectors[i].Reset();
			}
		}

		void TuneSectors() {
			for ( int i = 0, e = curVisibles.Count; i < e; ++i ) {
				var iso_object = curVisibles[i];
				iso_object.Internal.MinSector -= _sectorsMinNumPos;
				iso_object.Internal.MaxSector -= _sectorsMinNumPos;
				var min = iso_object.Internal.MinSector;
				var max = iso_object.Internal.MaxSector;
				for ( var y = min.y; y < max.y; ++y ) {
					for ( var x = min.x; x < max.x; ++x ) {
						var sector = FindSector(x, y);
						if ( sector != null ) {
							sector.objects.Add(iso_object);
						}
					}}
			}
		}

		void SetupSectors() {
			ResizeSectors((int)(_sectorsNumPosCount.x * _sectorsNumPosCount.y));
			TuneSectors();
		}

		Sector FindSector(float num_pos_x, float num_pos_y) {
			if ( num_pos_x < 0 || num_pos_y < 0 ) {
				return null;
			}
			if ( num_pos_x >= _sectorsNumPosCount.x || num_pos_y >= _sectorsNumPosCount.y ) {
				return null;
			}
			var sector_index = (int)(num_pos_x + _sectorsNumPosCount.x * num_pos_y);
			return _sectors[sector_index];
		}

		bool IsIsoObjectDepends(Vector3 a_min, Vector3 a_size, Vector3 b_min, Vector3 b_size) {
			var a_max = a_min + a_size;
			var b_max = b_min + b_size;
			var a_yesno = a_max.x > b_min.x && a_max.y > b_min.y && b_max.z > a_min.z;
			if ( a_yesno ) {
				var b_yesno = b_max.x > a_min.x && b_max.y > a_min.y && a_max.z > b_min.z;
				if ( b_yesno ) {

					//var da_p = new Vector3(a_max.x - b_min.x, a_max.y - b_min.y, b_max.z - a_min.z);
					//var db_p = new Vector3(b_max.x - a_min.x, b_max.y - a_min.y, a_max.z - b_min.z);
					//var dp_p = a_size + b_size - IsoUtils.Vec3Abs(da_p - db_p);

					var dA_x = a_max.x - b_min.x;
					var dA_y = a_max.y - b_min.y;
					var dA_z = b_max.z - a_min.z;

					var dB_x = b_max.x - a_min.x;
					var dB_y = b_max.y - a_min.y;
					var dB_z = a_max.z - b_min.z;

					var dP_x = a_size.x + b_size.x - Mathf.Abs(dA_x - dB_x);
					var dP_y = a_size.y + b_size.y - Mathf.Abs(dA_y - dB_y);
					var dP_z = a_size.z + b_size.z - Mathf.Abs(dA_z - dB_z);

					if ( dP_x <= dP_y && dP_x <= dP_z ) {
						return dA_x > dB_x;
					} else if ( dP_y <= dP_x && dP_y <= dP_z ) {
						return dA_y > dB_y;
					} else {
						return dA_z > dB_z;
					}
				}
			}
			return a_yesno;
		}

		bool IsIsoObjectDepends(IsoObject a, IsoObject b) {
			return
				a.Internal.ScreenBounds.Overlaps(b.Internal.ScreenBounds) &&
				IsIsoObjectDepends(a.position, a.size, b.position, b.size);
		}

		void LookUpSectorDepends(float num_pos_x, float num_pos_y, IsoObject obj_a) {
			var sec = FindSector(num_pos_x, num_pos_y);
			if ( sec != null ) {
				for ( int i = 0, e = sec.objects.Count; i < e; ++i ) {
					var obj_b = sec.objects[i];
					if ( obj_a != obj_b && !obj_b.Internal.Dirty && IsIsoObjectDepends(obj_a, obj_b) ) {
						obj_a.Internal.SelfDepends.Add(obj_b);
						obj_b.Internal.TheirDepends.Add(obj_a);
					}
				}
			}
		}

		void LookUpSectorRDepends(float num_pos_x, float num_pos_y, IsoObject obj_a) {
			var sec = FindSector(num_pos_x, num_pos_y);
			if ( sec != null ) {
				for ( int i = 0, e = sec.objects.Count; i < e; ++i ) {
					var obj_b = sec.objects[i];
					if ( obj_a != obj_b && !obj_b.Internal.Dirty && IsIsoObjectDepends(obj_b, obj_a) ) {
						obj_b.Internal.SelfDepends.Add(obj_a);
						obj_a.Internal.TheirDepends.Add(obj_b);
					}
				}
			}
		}

		public void SetupIsoObjectDepends(IsoObject obj_a) {
			ClearIsoObjectDepends(obj_a);
			var min = obj_a.Internal.MinSector;
			var max = obj_a.Internal.MaxSector;
			for ( var y = min.y; y < max.y; ++y ) {
				for ( var x = min.x; x < max.x; ++x ) {
					LookUpSectorDepends(x, y, obj_a);
					LookUpSectorRDepends(x, y, obj_a);
				}}
		}

		public void ClearIsoObjectDepends(IsoObject iso_object) {
			var their_depends = iso_object.Internal.TheirDepends;
			for ( int i = 0, e = their_depends.Count; i < e; ++i ) {
				var their_iso_object = their_depends[i];
				if ( !their_iso_object.Internal.Dirty ) {
					their_iso_object.Internal.SelfDepends.Remove(iso_object);
				}
			}
			iso_object.Internal.SelfDepends.Clear();
			iso_object.Internal.TheirDepends.Clear();
		}

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		public bool IsIsoObjectVisible(IsoObject iso_object) {
			var renderers = GetIsoObjectRenderers(iso_object);
			for ( int i = 0, e = renderers.Count; i < e; ++i ) {
				if ( renderers[i].isVisible ) {
					return true;
				}
			}
			return false;
		}

		List<Renderer> GetIsoObjectRenderers(IsoObject iso_object) {
			if ( iso_object.cacheRenderers ) {
				return iso_object.Internal.Renderers;
			} else {
				iso_object.GetComponentsInChildren<Renderer>(_tmpRenderers);
				return _tmpRenderers;
			}
		}
	}
}