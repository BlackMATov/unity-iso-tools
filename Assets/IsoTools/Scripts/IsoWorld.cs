﻿using UnityEngine;
using IsoTools.Internal;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace IsoTools {
	[ExecuteInEditMode, DisallowMultipleComponent]
	public sealed class IsoWorld : IsoHolder<IsoWorld, IsoObject> {

		bool                    _dirty        = false;
		Vector2                 _minXY        = Vector2.zero;
		IsoAssocList<IsoObject> _visibles     = new IsoAssocList<IsoObject>();
		IsoAssocList<IsoObject> _oldVisibles  = new IsoAssocList<IsoObject>();

		bool                    _dirtyMat     = true;
		Matrix4x4               _isoMatrix    = Matrix4x4.identity;
		Matrix4x4               _isoRMatrix   = Matrix4x4.identity;
		List<Renderer>          _tmpRenderers = new List<Renderer>();

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

		// ---------------------------------------------------------------------
		//
		// Constants
		//
		// ---------------------------------------------------------------------

		public static readonly float DefTileSize   = 32.0f;
		public static readonly float MinTileSize   = Mathf.Epsilon;
		public static readonly float MaxTileSize   = float.MaxValue;

		public static readonly float DefTileRatio  = 0.5f;
		public static readonly float MinTileRatio  = 0.25f;
		public static readonly float MaxTileRatio  = 1.0f;

		public static readonly float DefTileAngle  = 45.0f;
		public static readonly float MinTileAngle  = 0.0f;
		public static readonly float MaxTileAngle  = 90.0f;

		public static readonly float DefTileHeight = DefTileSize;
		public static readonly float MinTileHeight = MinTileSize;
		public static readonly float MaxTileHeight = MaxTileSize;

		public static readonly float DefStepDepth  = 0.1f;
		public static readonly float MinStepDepth  = Mathf.Epsilon;
		public static readonly float MaxStepDepth  = float.MaxValue;

		public static readonly float DefStartDepth = 1.0f;
		public static readonly float MinStartDepth = float.MinValue;
		public static readonly float MaxStartDepth = float.MaxValue;

		// ---------------------------------------------------------------------
		//
		// Sorting properties
		//
		// ---------------------------------------------------------------------

		[Header("World Settings")]
		[SerializeField]
		public float _tileSize = DefTileSize;
		public float tileSize {
			get { return _tileSize; }
			set {
				_tileSize = Mathf.Clamp(value, MinTileSize, MaxTileSize);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		public float _tileRatio = DefTileRatio;
		public float tileRatio {
			get { return _tileRatio; }
			set {
				_tileRatio = Mathf.Clamp(value, MinTileRatio, MaxTileRatio);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		public float _tileAngle = DefTileAngle;
		public float tileAngle {
			get { return _tileAngle; }
			set {
				_tileAngle = Mathf.Clamp(value, MinTileAngle, MaxTileAngle);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		public float _tileHeight = DefTileHeight;
		public float tileHeight {
			get { return _tileHeight; }
			set {
				_tileHeight = Mathf.Clamp(value, MinTileHeight, MaxTileHeight);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		public float _stepDepth = DefStepDepth;
		public float stepDepth {
			get { return _stepDepth; }
			set {
				_stepDepth = Mathf.Clamp(value, MinStepDepth, MaxStepDepth);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		public float _startDepth = DefStartDepth;
		public float startDepth {
			get { return _startDepth; }
			set {
				_startDepth = Mathf.Clamp(value, MinStartDepth, MaxStartDepth);
				ChangeSortingProperty();
			}
		}

		// ---------------------------------------------------------------------
		//
		// Instances
		//
		// ---------------------------------------------------------------------

		public static int AllWorldCount {
			get { return AllBehaviourCount; }
		}

		public static IsoWorld GetWorld(int index) {
			return GetBehaviourByIndex(index);
		}

		// ---------------------------------------------------------------------
		//
		// IsoToScreen/ScreenToIso
		//
		// ---------------------------------------------------------------------

		public Vector2 IsoToScreen(Vector3 iso_pnt) {
			if ( _dirtyMat ) {
				UpdateIsoMatrix();
				_dirtyMat = false;
			}
			var screen_pos = _isoMatrix.MultiplyPoint(iso_pnt);
			return new Vector2(
				screen_pos.x,
				screen_pos.y + iso_pnt.z * tileHeight);
		}

		public Vector3 ScreenToIso(Vector2 pos) {
			if ( _dirtyMat ) {
				UpdateIsoMatrix();
				_dirtyMat = false;
			}
			return _isoRMatrix.MultiplyPoint(new Vector3(pos.x, pos.y, 0.0f));
		}

		public Vector3 ScreenToIso(Vector2 pos, float iso_z) {
			return IsoUtils.Vec3ChangeZ(
				ScreenToIso(new Vector2(pos.x, pos.y - iso_z * tileHeight)),
				iso_z);
		}

		// ---------------------------------------------------------------------
		//
		// RayFromIsoCameraToIsoPoint
		//
		// ---------------------------------------------------------------------

		public Ray RayFromIsoCameraToIsoPoint(Vector3 iso_pnt) {
			var screen_pnt      = IsoToScreen(iso_pnt);

			var min_screen_pnt  = IsoToScreen(_minXY - Vector2.one);
			var max_screen_dist = screen_pnt.y - min_screen_pnt.y;

			var screen_down_pnt = new Vector2(screen_pnt.x, screen_pnt.y - max_screen_dist);
			var iso_down_pnt    = ScreenToIso(screen_down_pnt, iso_pnt.z);
			iso_down_pnt.z     += max_screen_dist / tileHeight;
			return new Ray(iso_down_pnt, iso_pnt - iso_down_pnt);
		}

		// ---------------------------------------------------------------------
		//
		// TouchIsoPosition
		//
		// ---------------------------------------------------------------------

		public Vector3 TouchIsoPosition(int finger_id) {
			return TouchIsoPosition(finger_id, 0.0f);
		}

		public Vector3 TouchIsoPosition(int finger_id, float iso_z) {
			if ( !Camera.main ) {
				throw new UnityException("Main camera not found!");
			}
			return TouchIsoPosition(finger_id, Camera.main, iso_z);
		}

		public Vector3 TouchIsoPosition(int finger_id, Camera camera) {
			return TouchIsoPosition(finger_id, camera, 0.0f);
		}

		public Vector3 TouchIsoPosition(int finger_id, Camera camera, float iso_z) {
			if ( !camera ) {
				throw new UnityException("Camera argument is incorrect!");
			}
			for ( var i = 0; i < Input.touchCount; ++i ) {
				var touch = Input.GetTouch(i);
				if ( touch.fingerId == finger_id ) {
					return ScreenToIso(
						camera.ScreenToWorldPoint(touch.position),
						iso_z);
				}
			}
			throw new UnityException("Touch finger id argument is incorrect!");
		}

		// ---------------------------------------------------------------------
		//
		// TouchIsoTilePosition
		//
		// ---------------------------------------------------------------------
		
		public Vector3 TouchIsoTilePosition(int finger_id) {
			return IsoUtils.Vec3Floor(TouchIsoPosition(finger_id));
		}
		
		public Vector3 TouchIsoTilePosition(int finger_id, float iso_z) {
			return IsoUtils.Vec3Floor(TouchIsoPosition(finger_id, iso_z));
		}
		
		public Vector3 TouchIsoTilePosition(int finger_id, Camera camera) {
			return IsoUtils.Vec3Floor(TouchIsoPosition(finger_id, camera));
		}
		
		public Vector3 TouchIsoTilePosition(int finger_id, Camera camera, float iso_z) {
			return IsoUtils.Vec3Floor(TouchIsoPosition(finger_id, camera, iso_z));
		}

		// ---------------------------------------------------------------------
		//
		// MouseIsoPosition
		//
		// ---------------------------------------------------------------------

		public Vector3 MouseIsoPosition() {
			return MouseIsoPosition(0.0f);
		}

		public Vector3 MouseIsoPosition(float iso_z) {
			if ( !Camera.main ) {
				throw new UnityException("Main camera not found!");
			}
			return MouseIsoPosition(Camera.main, iso_z);
		}

		public Vector3 MouseIsoPosition(Camera camera) {
			return MouseIsoPosition(camera, 0.0f);
		}

		public Vector3 MouseIsoPosition(Camera camera, float iso_z) {
			if ( !camera ) {
				throw new UnityException("Camera argument is incorrect!");
			}
			return ScreenToIso(
				camera.ScreenToWorldPoint(Input.mousePosition),
				iso_z);
		}

		// ---------------------------------------------------------------------
		//
		// MouseIsoTilePosition
		//
		// ---------------------------------------------------------------------

		public Vector3 MouseIsoTilePosition() {
			return IsoUtils.Vec3Floor(MouseIsoPosition());
		}

		public Vector3 MouseIsoTilePosition(float iso_z) {
			return IsoUtils.Vec3Floor(MouseIsoPosition(iso_z));
		}

		public Vector3 MouseIsoTilePosition(Camera camera) {
			return IsoUtils.Vec3Floor(MouseIsoPosition(camera));
		}
		
		public Vector3 MouseIsoTilePosition(Camera camera, float iso_z) {
			return IsoUtils.Vec3Floor(MouseIsoPosition(camera, iso_z));
		}

		// ---------------------------------------------------------------------
		//
		// For editor
		//
		// ---------------------------------------------------------------------

	#if UNITY_EDITOR
		[Header("Editor Only")]
		[SerializeField] bool _showIsoBounds = false;
		public bool isShowIsoBounds {
			get { return _showIsoBounds; }
			set { _showIsoBounds = value; }
		}
		[SerializeField] bool _showScreenBounds = false;
		public bool isShowScreenBounds {
			get { return _showScreenBounds; }
			set { _showScreenBounds = value; }
		}
		[SerializeField] bool _showDepends = false;
		public bool isShowDepends {
			get { return _showDepends; }
			set { _showDepends = value; }
		}
		[SerializeField] bool _snapByCells = true;
		public bool isSnapByCells {
			get { return _snapByCells; }
			set { _snapByCells = value; }
		}
		[SerializeField] bool _snapByObjects = true;
		public bool isSnapByObjects {
			get { return _snapByObjects; }
			set { _snapByObjects = value; }
		}
	#endif

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		public void MarkDirty() {
			if ( !_dirty ) {
				_dirty = true;
			#if UNITY_EDITOR
				EditorUtility.SetDirty(this);
			#endif
			}
		}
		
		public void MarkDirty(IsoObject iso_object) {
			if ( !iso_object.Internal.Dirty && _visibles.Contains(iso_object) ) {
				iso_object.Internal.Dirty = true;
				MarkDirty();
			}
		}

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		void MarkDirtyIsoMatrix() {
			_dirtyMat = true;
		}

		void UpdateIsoMatrix() {
			_isoMatrix =
				Matrix4x4.Scale(new Vector3(1.0f, tileRatio, 1.0f)) *
				Matrix4x4.TRS(
					Vector3.zero,
					Quaternion.AngleAxis(90.0f - tileAngle, IsoUtils.vec3OneZ),
					new Vector3(tileSize * Mathf.Sqrt(2), tileSize * Mathf.Sqrt(2), tileHeight));
			_isoRMatrix = _isoMatrix.inverse;
		}

		void FixAllTransforms() {
			var instances = GetInstances();
			for ( int i = 0, e = instances.Count; i < e; ++i ) {
				instances[i].FixTransform();
			}
		}

		void ChangeSortingProperty() {
			MarkDirty();
			MarkDirtyIsoMatrix();
			FixAllTransforms();
		}

		bool UpdateIsoObjectBounds3d(IsoObject iso_object) {
			if ( iso_object.mode == IsoObject.Mode.Mode3d ) {
				var minmax3d = IsoObjectMinMax3D(iso_object);
				var offset3d = iso_object.Internal.Transform.position.z - minmax3d.center;
				if ( iso_object.Internal.MinMax3d.Approximately(minmax3d) ||
					!Mathf.Approximately(iso_object.Internal.Offset3d, offset3d) )
				{
					iso_object.Internal.MinMax3d = minmax3d;
					iso_object.Internal.Offset3d = offset3d;
					return true;
				}
			}
			return false;
		}

		IsoMinMax IsoObjectMinMax3D(IsoObject iso_object) {
			bool inited    = false;
			var  result    = IsoMinMax.zero;
			var  renderers = GetIsoObjectRenderers(iso_object);
			for ( int i = 0, e = renderers.Count; i < e; ++i ) {
				var bounds = renderers[i].bounds;
				var extents = bounds.extents;
				if ( extents.x > 0.0f || extents.y > 0.0f || extents.z > 0.0f ) {
					var center    = bounds.center.z;
					var minbounds = center - extents.z;
					var maxbounds = center + extents.z;
					if ( inited ) {
						if ( minbounds < result.min ) {
							result.min = minbounds;
						}
						if ( maxbounds > result.max ) {
							result.max = maxbounds;
						}
					} else {
						inited = true;
						result = new IsoMinMax(minbounds, maxbounds);
					}
				}
			}
			return inited ? result : IsoMinMax.zero;
		}

		List<Renderer> GetIsoObjectRenderers(IsoObject iso_object) {
			if ( iso_object.cacheRenderers ) {
				return iso_object.Internal.Renderers;
			} else {
				iso_object.GetComponentsInChildren<Renderer>(_tmpRenderers);
				return _tmpRenderers;
			}
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

		void SetupSectorSize() {
			_sectorsSize = 0.0f;
			for ( int i = 0, e = _visibles.Count; i < e; ++i ) {
				var iso_internal = _visibles[i].Internal;
				_sectorsSize += IsoUtils.Vec2MaxF(iso_internal.ScreenBounds.size);
			}
			var min_sector_size_xy = IsoToScreen(IsoUtils.vec3OneXY) - IsoToScreen(Vector3.zero);
			var min_sector_size    = Mathf.Max(min_sector_size_xy.x, min_sector_size_xy.y);
			_sectorsSize = _visibles.Count > 0
				? Mathf.Round(Mathf.Max(min_sector_size, _sectorsSize / _visibles.Count))
				: min_sector_size;
		}

		void SetupObjectsSectors() {
			if ( _visibles.Count > 0 ) {
				_sectorsMinNumPos.Set(float.MaxValue, float.MaxValue);
				_sectorsMaxNumPos.Set(float.MinValue, float.MinValue);
				for ( int i = 0, e = _visibles.Count; i < e; ++i ) {
					var iso_internal = _visibles[i].Internal;

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
			for ( int i = 0, e = _visibles.Count; i < e; ++i ) {
				var iso_object = _visibles[i];
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

		void StepSort() {
			Profiler.BeginSample("UpdateVisibles");
			UpdateVisibles();
			Profiler.EndSample();
			if ( _dirty ) {
				Profiler.BeginSample("PlaceAllVisibles");
				PlaceAllVisibles();
				Profiler.EndSample();
				_dirty = false;
			}
			PostStepSortActions();
		}

		void PostStepSortActions() {
			_tmpRenderers.Clear();
		}

		void UpdateVisibles() {
			Profiler.BeginSample("CalculateNewVisibles");
			CalculateNewVisibles();
			Profiler.EndSample();

			Profiler.BeginSample("CalculateSectors");
			SetupSectorSize();
			SetupObjectsSectors();
			SetupSectors();
			Profiler.EndSample();

			Profiler.BeginSample("ResolveVisibles");
			ResolveVisibles();
			Profiler.EndSample();
		}

		void CalculateNewVisibles() {
			_oldVisibles.Clear();
			var instances = GetInstances();
			if ( instances.Count > 0 ) {
				_minXY.Set(float.MaxValue, float.MaxValue);
				for ( int i = 0, e = instances.Count; i < e; ++i ) {
					var iso_object     = instances[i];
					var iso_object_pos = iso_object.position;
					if ( _minXY.x > iso_object_pos.x ) {
						_minXY.x = iso_object_pos.x;
					}
					if ( _minXY.y > iso_object_pos.y ) {
						_minXY.y = iso_object_pos.y;
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
				_minXY.Set(0.0f, 0.0f);
			}
			var old_visibles = _visibles;
			_visibles = _oldVisibles;
			_oldVisibles = old_visibles;
		}

		void ResolveVisibles() {
			for ( int i = 0, e = _visibles.Count; i < e; ++i ) {
				var iso_object = _visibles[i];
				if ( iso_object.Internal.Dirty || !_oldVisibles.Contains(iso_object) ) {
					MarkDirty();
					SetupIsoObjectDepends(iso_object);
					iso_object.Internal.Dirty = false;
				}
				if ( UpdateIsoObjectBounds3d(iso_object) ) {
					MarkDirty();
				}
			}
			for ( int i = 0, e = _oldVisibles.Count; i < e; ++i ) {
				var iso_object = _oldVisibles[i];
				if ( !_visibles.Contains(iso_object) ) {
					MarkDirty();
					ClearIsoObjectDepends(iso_object);
				}
			}
		}

		void ClearIsoObjectDepends(IsoObject iso_object) {
			var their_depends = iso_object.Internal.TheirDepends;
			for ( int i = 0, e = their_depends.Count; i < e; ++i ) {
				var their_depend = their_depends[i];
				if ( !their_depend.Internal.Dirty ) {
					their_depend.Internal.SelfDepends.Remove(iso_object);
				}
			}
			iso_object.Internal.SelfDepends.Clear();
			iso_object.Internal.TheirDepends.Clear();
		}
		
		void SetupIsoObjectDepends(IsoObject obj_a) {
			ClearIsoObjectDepends(obj_a);
			var min = obj_a.Internal.MinSector;
			var max = obj_a.Internal.MaxSector;
			for ( var y = min.y; y < max.y; ++y ) {
			for ( var x = min.x; x < max.x; ++x ) {
				LookUpSectorDepends(x, y, obj_a);
				LookUpSectorRDepends(x, y, obj_a);
			}}
		}

		void PlaceAllVisibles() {
			var depth = startDepth;
			for ( int i = 0, e = _visibles.Count; i < e; ++i ) {
				depth = RecursivePlaceIsoObject(_visibles[i], depth);
			}
		}

		float RecursivePlaceIsoObject(IsoObject iso_object, float depth) {
			if ( iso_object.Internal.Placed ) {
				return depth;
			}
			iso_object.Internal.Placed = true;
			var self_depends = iso_object.Internal.SelfDepends;
			for ( int i = 0, e = self_depends.Count; i < e; ++i ) {
				depth = RecursivePlaceIsoObject(self_depends[i], depth);
			}
			if ( iso_object.mode == IsoObject.Mode.Mode3d ) {
				var zoffset = iso_object.Internal.Offset3d;
				var extents = iso_object.Internal.MinMax3d.size;
				PlaceIsoObject(iso_object, depth + extents * 0.5f + zoffset);
				return depth + extents + stepDepth;
			} else {
				PlaceIsoObject(iso_object, depth);
				return depth + stepDepth;
			}
		}

		void PlaceIsoObject(IsoObject iso_object, float depth) {
			var iso_internal = iso_object.Internal;
			iso_internal.Transform.position =
				IsoUtils.Vec3FromVec2(iso_internal.LastTrans, depth);
		}

		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		void Start() {
			ChangeSortingProperty();
			StepSort();
		}

		void LateUpdate() {
			StepSort();
		}

		protected override void OnEnable() {
			base.OnEnable();
			_visibles.Clear();
			_oldVisibles.Clear();
			_sectors.Clear();
			MarkDirty();
		}

		protected override void OnDisable() {
			base.OnDisable();
			_visibles.Clear();
			_oldVisibles.Clear();
			_sectors.Clear();
		}

		protected override void OnAddInstanceToHolder(IsoObject instance) {
			base.OnAddInstanceToHolder(instance);
			if ( instance.cacheRenderers ) {
				instance.UpdateCachedRenderers();
			}
		}

		protected override void OnRemoveInstanceFromHolder(IsoObject instance) {
			base.OnRemoveInstanceFromHolder(instance);
			if ( instance.cacheRenderers ) {
				instance.ClearCachedRenderers();
			}
			ClearIsoObjectDepends(instance);
			_visibles.Remove(instance);
			_oldVisibles.Remove(instance);
		}

	#if UNITY_EDITOR
		void Reset() {
			tileSize   = DefTileSize;
			tileRatio  = DefTileRatio;
			tileAngle  = DefTileAngle;
			tileHeight = DefTileHeight;
			stepDepth  = DefStepDepth;
			startDepth = DefStartDepth;
		}
		
		void OnValidate() {
			tileSize   = _tileSize;
			tileRatio  = _tileRatio;
			tileAngle  = _tileAngle;
			tileHeight = _tileHeight;
			stepDepth  = _stepDepth;
			startDepth = _startDepth;
		}

		void OnRenderObject() {
			if ( Camera.current && Camera.current.name == "SceneCamera" ) {
				StepSort();
			}
		}

		/* QuadTree WIP
		void OnDrawGizmos() {
			var qt = new IsoQuadTree<IsoObject>(_objects.Count);
			for ( int i = 0, e = _objects.Count; i < e; ++i ) {
				qt.Insert(_objects[i].Internal.ScreenRect, _objects[i]);
			}
			qt.VisitAllBounds(rect => {
				IsoUtils.DrawRect(rect, Color.green);
			});
		}*/
	#endif
	}
}