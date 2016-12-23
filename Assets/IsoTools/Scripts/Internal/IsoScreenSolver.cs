using UnityEngine;
using System.Collections.Generic;

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace IsoTools.Internal {
	public class IsoScreenSolver {
		Vector2                 _minIsoXY      = Vector2.zero;
		IsoAssocList<IsoObject> _curVisibles   = new IsoAssocList<IsoObject>();
		IsoAssocList<IsoObject> _oldVisibles   = new IsoAssocList<IsoObject>();
		IsoGrid<IsoObject>      _visibleGrid   = new IsoGrid<IsoObject>(new IsoGridItemAdapter(), 47);
		IsoGridLookUpper        _gridLookUpper = new IsoGridLookUpper();
		List<Renderer>          _tmpRenderers  = new List<Renderer>();

		// ---------------------------------------------------------------------
		//
		// IsoGridItemAdapter
		//
		// ---------------------------------------------------------------------

		class IsoGridItemAdapter : IsoGrid<IsoObject>.IItemAdapter {
			public IsoRect GetBounds(IsoObject item) {
				return item.Internal.ScreenBounds;
			}

			public void SetMinMaxCells(IsoObject item, Vector2 min, Vector2 max) {
				item.Internal.MinSector = min;
				item.Internal.MaxSector = max;
			}

			public void GetMinMaxCells(IsoObject item, ref Vector2 min, ref Vector2 max) {
				min = item.Internal.MinSector;
				max = item.Internal.MaxSector;
			}
		}

		// ---------------------------------------------------------------------
		//
		// IsoGridLookUpper
		//
		// ---------------------------------------------------------------------

		class IsoGridLookUpper : IsoGrid<IsoObject>.ILookUpper {
			IsoObject _isoObject;

			public void LookUp(IsoList<IsoObject> items) {
				LookUpSectorDepends(_isoObject, items);
				LookUpSectorRDepends(_isoObject, items);
			}

			public void Setup(IsoObject iso_object) {
				_isoObject = iso_object;
			}

			public void Reset() {
				_isoObject = null;
			}
		}

		// ---------------------------------------------------------------------
		//
		// Properties
		//
		// ---------------------------------------------------------------------

		public Vector2 minIsoXY {
			get { return _minIsoXY; }
		}

		public IsoAssocList<IsoObject> curVisibles {
			get { return _curVisibles; }
		}

		public IsoAssocList<IsoObject> oldVisibles {
			get { return _oldVisibles; }
		}

		// ---------------------------------------------------------------------
		//
		// Callbacks
		//
		// ---------------------------------------------------------------------

		public void OnAddInstance(IsoObject iso_object) {
		}

		public void OnRemoveInstance(IsoObject iso_object) {
			_oldVisibles.Remove(iso_object);
			_curVisibles.Remove(iso_object);
			ClearIsoObjectDepends(iso_object);
		}

		public bool OnMarkDirtyInstance(IsoObject iso_object) {
			if ( !iso_object.Internal.Dirty && _curVisibles.Contains(iso_object) ) {
				iso_object.Internal.Dirty = true;
				return true;
			}
			return false;
		}

	#if UNITY_EDITOR
		public void OnDrawGizmos(IsoWorld iso_world) {
			/*
			for ( int y = 0, ye = (int)_sectorsNumPosCount.y; y < ye; ++y ) {
			for ( int x = 0, xe = (int)_sectorsNumPosCount.x; x < xe; ++x ) {
				var sector = FindSector((float)x, (float)y);
				if ( sector != null && sector.objects.Count > 0 ) {
					var rect = new IsoRect(
						(x * _sectorsSize),
						(y * _sectorsSize),
						(x * _sectorsSize) + _sectorsSize,
						(y * _sectorsSize) + _sectorsSize);
					rect.Translate(_sectorsMinNumPos * _sectorsSize);
					IsoUtils.DrawRect(rect, Color.blue);
				}
			}}*/
		}
	#endif

		// ---------------------------------------------------------------------
		//
		// Functions
		//
		// ---------------------------------------------------------------------

		public void StepSortingAction(IsoWorld iso_world, IsoAssocList<IsoObject> instances) {
			Profiler.BeginSample("ResolveVisibles");
			ResolveVisibles(instances);
			Profiler.EndSample();
			Profiler.BeginSample("ResolveVisibleGrid");
			ResolveVisibleGrid(iso_world);
			Profiler.EndSample();
		}

		public void PostStepSortingAction() {
			_tmpRenderers.Clear();
		}

		public void Clear() {
			_curVisibles.Clear();
			_oldVisibles.Clear();
			_visibleGrid.ClearGrid();
		}

		public void SetupIsoObjectDepends(IsoObject iso_object) {
			ClearIsoObjectDepends(iso_object);
			_gridLookUpper.Setup(iso_object);
			_visibleGrid.LookUpCells(
				iso_object.Internal.MinSector,
				iso_object.Internal.MaxSector,
				_gridLookUpper);
			_gridLookUpper.Reset();
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
		// ResolveVisibles
		//
		// ---------------------------------------------------------------------

		void ResolveVisibles(IsoAssocList<IsoObject> instances) {
			_oldVisibles.Clear();
			if ( instances.Count > 0 ) {
				_minIsoXY.Set(float.MaxValue, float.MaxValue);
				for ( int i = 0, e = instances.Count; i < e; ++i ) {
					var iso_object = instances[i];
					var object_pos = iso_object.position;
					if ( _minIsoXY.x > object_pos.x ) {
						_minIsoXY.x = object_pos.x;
					}
					if ( _minIsoXY.y > object_pos.y ) {
						_minIsoXY.y = object_pos.y;
					}
					if ( !IsoUtils.Vec2Approximately(
						iso_object.Internal.LastTrans,
						iso_object.Internal.Transform.position) )
					{
						iso_object.FixIsoPosition();
					}
					if ( IsIsoObjectVisible(iso_object) ) {
						iso_object.Internal.Placed = false;
						_oldVisibles.Add(iso_object);
					}
				}
			} else {
				_minIsoXY.Set(0.0f, 0.0f);
			}
			var temp_visibles = _curVisibles;
			_curVisibles = _oldVisibles;
			_oldVisibles = temp_visibles;
		}

		bool IsIsoObjectVisible(IsoObject iso_object) {
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

		// ---------------------------------------------------------------------
		//
		// ResolveVisibleGrid
		//
		// ---------------------------------------------------------------------

		void ResolveVisibleGrid(IsoWorld iso_world) {
			_visibleGrid.ClearItems();
			for ( int i = 0, e = _curVisibles.Count; i < e; ++i ) {
				var iso_object = _curVisibles[i];
				_visibleGrid.AddItem(iso_object);
			}
			var min_sector_size = IsoUtils.Vec2MaxF(
				iso_world.IsoToScreen(IsoUtils.vec3OneXY) -
				iso_world.IsoToScreen(Vector3.zero));
			_visibleGrid.RebuildGrid(min_sector_size);
		}

		// ---------------------------------------------------------------------
		//
		// LookUpSectorDepends
		//
		// ---------------------------------------------------------------------

		static void LookUpSectorDepends(IsoObject obj_a, IsoList<IsoObject> others) {
			for ( int i = 0, e = others.Count; i < e; ++i ) {
				var obj_b = others[i];
				if ( obj_a != obj_b && !obj_b.Internal.Dirty && IsIsoObjectDepends(obj_a, obj_b) ) {
					obj_a.Internal.SelfDepends.Add(obj_b);
					obj_b.Internal.TheirDepends.Add(obj_a);
				}
			}
		}

		static void LookUpSectorRDepends(IsoObject obj_a, IsoList<IsoObject> others) {
			for ( int i = 0, e = others.Count; i < e; ++i ) {
				var obj_b = others[i];
				if ( obj_a != obj_b && !obj_b.Internal.Dirty && IsIsoObjectDepends(obj_b, obj_a) ) {
					obj_b.Internal.SelfDepends.Add(obj_a);
					obj_a.Internal.TheirDepends.Add(obj_b);
				}
			}
		}

		static bool IsIsoObjectDepends(IsoObject a, IsoObject b) {
			return
				a.Internal.ScreenBounds.Overlaps(b.Internal.ScreenBounds) &&
				IsIsoObjectDepends(a.position, a.size, b.position, b.size);
		}

		static bool IsIsoObjectDepends(Vector3 a_min, Vector3 a_size, Vector3 b_min, Vector3 b_size) {
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
	}
}