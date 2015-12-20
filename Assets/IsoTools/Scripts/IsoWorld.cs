using UnityEngine;
using IsoTools.Internal;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[ExecuteInEditMode, DisallowMultipleComponent]
	public class IsoWorld : MonoBehaviour {

		bool               _dirty        = false;
		HashSet<IsoObject> _objects      = new HashSet<IsoObject>();
		HashSet<IsoObject> _visibles     = new HashSet<IsoObject>();
		HashSet<IsoObject> _oldVisibles  = new HashSet<IsoObject>();

		bool               _dirtyMat     = true;
		Matrix4x4          _isoMatrix    = Matrix4x4.identity;
		Matrix4x4          _isoRMatrix   = Matrix4x4.identity;
		List<Renderer>     _tmpRenderers = new List<Renderer>();

		class Sector {
			public List<IsoObject> objects = new List<IsoObject>();
			public void Reset() {
				objects.Clear();
			}
		}

		List<Sector> _sectors            = new List<Sector>();
		float        _sectorsSize        = 0.0f;
		Vector2      _sectorsMinNumPos   = Vector2.zero;
		Vector2      _sectorsMaxNumPos   = Vector2.zero;
		Vector2      _sectorsNumPosCount = Vector2.zero;

		// ------------------------------------------------------------------------
		//
		// Constants
		//
		// ------------------------------------------------------------------------

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

		// ------------------------------------------------------------------------
		//
		// Sorting properties
		//
		// ------------------------------------------------------------------------

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

		// ------------------------------------------------------------------------
		//
		// IsoToScreen/ScreenToIso
		//
		// ------------------------------------------------------------------------

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

		// ------------------------------------------------------------------------
		//
		// TouchIsoPosition
		//
		// ------------------------------------------------------------------------

		public Vector3 TouchIsoPosition(int finger_id) {
			return TouchIsoPosition(finger_id, 0.0f);
		}

		public Vector3 TouchIsoPosition(int finger_id, float iso_z) {
			if ( !Camera.main ) {
				Debug.LogError("Main camera not found!", this);
				return Vector3.zero;
			}
			return TouchIsoPosition(finger_id, Camera.main, iso_z);
		}

		public Vector3 TouchIsoPosition(int finger_id, Camera camera) {
			return TouchIsoPosition(finger_id, camera, 0.0f);
		}

		public Vector3 TouchIsoPosition(int finger_id, Camera camera, float iso_z) {
			if ( !camera ) {
				Debug.LogError("Camera argument is incorrect!", this);
				return Vector3.zero;
			}
			for ( var i = 0; i < Input.touchCount; ++i ) {
				var touch = Input.GetTouch(i);
				if ( touch.fingerId == finger_id ) {
					return ScreenToIso(
						camera.ScreenToWorldPoint(touch.position),
						iso_z);
				}
			}
			Debug.LogError("Touch finger id argument is incorrect!", this);
			return Vector3.zero;
		}

		// ------------------------------------------------------------------------
		//
		// TouchIsoTilePosition
		//
		// ------------------------------------------------------------------------
		
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

		// ------------------------------------------------------------------------
		//
		// MouseIsoPosition
		//
		// ------------------------------------------------------------------------

		public Vector3 MouseIsoPosition() {
			return MouseIsoPosition(0.0f);
		}

		public Vector3 MouseIsoPosition(float iso_z) {
			if ( !Camera.main ) {
				Debug.LogError("Main camera not found!", this);
				return Vector3.zero;
			}
			return MouseIsoPosition(Camera.main, iso_z);
		}

		public Vector3 MouseIsoPosition(Camera camera) {
			return MouseIsoPosition(camera, 0.0f);
		}

		public Vector3 MouseIsoPosition(Camera camera, float iso_z) {
			if ( !camera ) {
				Debug.LogError("Camera argument is incorrect!", this);
				return Vector3.zero;
			}
			return ScreenToIso(
				camera.ScreenToWorldPoint(Input.mousePosition),
				iso_z);
		}

		// ------------------------------------------------------------------------
		//
		// MouseIsoTilePosition
		//
		// ------------------------------------------------------------------------

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

		// ------------------------------------------------------------------------
		//
		// Internal
		//
		// ------------------------------------------------------------------------

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

		public void AddIsoObject(IsoObject iso_object) {
			_objects.Add(iso_object);
		}

		public void RemoveIsoObject(IsoObject iso_object) {
			ClearIsoObjectDepends(iso_object);
			_objects.Remove(iso_object);
			_visibles.Remove(iso_object);
			_oldVisibles.Remove(iso_object);
		}

		// ------------------------------------------------------------------------
		//
		// Private
		//
		// ------------------------------------------------------------------------

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
			var objects_iter = _objects.GetEnumerator();
			while ( objects_iter.MoveNext() ) {
				objects_iter.Current.FixTransform();
			}
		}

		void ChangeSortingProperty() {
			MarkDirty();
			MarkDirtyIsoMatrix();
			FixAllTransforms();
		}

		bool UpdateIsoObjectBounds3d(IsoObject iso_object) {
			if ( iso_object.mode == IsoObject.Mode.Mode3d ) {
				var bounds3d = IsoObject3DBounds(iso_object);
				var offset3d = iso_object.transform.position.z - bounds3d.center.z;
				if ( iso_object.Internal.Bounds3d.extents != bounds3d.extents ||
				     !Mathf.Approximately(iso_object.Internal.Offset3d, offset3d) )
				{
					iso_object.Internal.Bounds3d = bounds3d;
					iso_object.Internal.Offset3d = offset3d;
					return true;
				}
			}
			return false;
		}
		
		Bounds IsoObject3DBounds(IsoObject iso_object) {
			var bounds = new Bounds();
			iso_object.GetComponentsInChildren<Renderer>(_tmpRenderers);
			if ( _tmpRenderers.Count > 0 ) {
				bounds = _tmpRenderers[0].bounds;
				for ( var i = 1; i < _tmpRenderers.Count; ++i ) {
					bounds.Encapsulate(_tmpRenderers[i].bounds);
				}
			}
			return bounds;
		}

		bool IsIsoObjectVisible(IsoObject iso_object) {
			iso_object.GetComponentsInChildren<Renderer>(_tmpRenderers);
			for ( var i = 0; i < _tmpRenderers.Count; ++i ) {
				if ( _tmpRenderers[i].isVisible ) {
					return true;
				}
			}
			return false;
		}

		bool IsIsoObjectDepends(Vector3 a_min, Vector3 a_size, Vector3 b_min, Vector3 b_size) {
			var a_max = a_min + a_size;
			var b_max = b_min + b_size;
			var a_yesno = a_max.x > b_min.x && a_max.y > b_min.y && b_max.z > a_min.z;
			var b_yesno = b_max.x > a_min.x && b_max.y > a_min.y && a_max.z > b_min.z;
			if ( a_yesno && b_yesno ) {
				var da_p = new Vector3(a_max.x - b_min.x, a_max.y - b_min.y, b_max.z - a_min.z);
				var db_p = new Vector3(b_max.x - a_min.x, b_max.y - a_min.y, a_max.z - b_min.z);
				var dp_p = a_size + b_size - IsoUtils.Vec3Abs(da_p - db_p);
				if ( dp_p.x <= dp_p.y && dp_p.x <= dp_p.z ) {
					return da_p.x > db_p.x;
				} else if ( dp_p.y <= dp_p.x && dp_p.y <= dp_p.z ) {
					return da_p.y > db_p.y;
				} else {
					return da_p.z > db_p.z;
				}
			}
			return a_yesno;
		}

		bool IsIsoObjectDepends(IsoObject a, IsoObject b) {
			return
				a.Internal.ScreenRect.Overlaps(b.Internal.ScreenRect) &&
				IsIsoObjectDepends(a.position, a.size, b.position, b.size);
		}

		int SectorIndex(Vector2 num_pos) {
			return Mathf.FloorToInt(num_pos.x + _sectorsNumPosCount.x * num_pos.y);
		}
		
		Sector FindSector(Vector2 num_pos) {
			if ( num_pos.x < 0 || num_pos.y < 0 ) {
				return null;
			}
			if ( num_pos.x >= _sectorsNumPosCount.x || num_pos.y >= _sectorsNumPosCount.y ) {
				return null;
			}
			return _sectors[SectorIndex(num_pos)];
		}
		
		void LookUpSectorDepends(Vector2 num_pos, IsoObject obj_a) {
			var sec = FindSector(num_pos);
			if ( sec != null ) {
				var sec_objects_iter = sec.objects.GetEnumerator();
				while ( sec_objects_iter.MoveNext() ) {
					var obj_b = sec_objects_iter.Current;
					if ( obj_a != obj_b && !obj_b.Internal.Dirty && IsIsoObjectDepends(obj_a, obj_b) ) {
						obj_a.Internal.SelfDepends.Add(obj_b);
						obj_b.Internal.TheirDepends.Add(obj_a);
					}
				}
			}
		}

		void LookUpSectorRDepends(Vector2 num_pos, IsoObject obj_a) {
			var sec = FindSector(num_pos);
			if ( sec != null ) {
				var sec_objects_iter = sec.objects.GetEnumerator();
				while ( sec_objects_iter.MoveNext() ) {
					var obj_b = sec_objects_iter.Current;
					if ( obj_a != obj_b && !obj_b.Internal.Dirty && IsIsoObjectDepends(obj_b, obj_a) ) {
						obj_b.Internal.SelfDepends.Add(obj_a);
						obj_a.Internal.TheirDepends.Add(obj_b);
					}
				}
			}
		}

		void SetupSectorSize() {
			_sectorsSize = 0.0f;
			var visibles_iter = _visibles.GetEnumerator();
			while ( visibles_iter.MoveNext() ) {
				_sectorsSize += IsoUtils.Vec2MaxF(visibles_iter.Current.Internal.ScreenRect.size);
			}
			var min_sector_size = IsoToScreen(IsoUtils.vec3OneX).x - IsoToScreen(Vector3.zero).x;
			_sectorsSize = Mathf.Round(Mathf.Max(min_sector_size, _sectorsSize / _visibles.Count));
		}

		void SetupObjectsSectors() {
			_sectorsMinNumPos = IsoUtils.Vec2From(float.MaxValue);
			_sectorsMaxNumPos = IsoUtils.Vec2From(float.MinValue);
			var visibles_iter = _visibles.GetEnumerator();
			while ( visibles_iter.MoveNext() ) {
				var iso_internal = visibles_iter.Current.Internal;
				iso_internal.MinSector = IsoUtils.Vec3DivFloor(iso_internal.ScreenRect.min, _sectorsSize);
				iso_internal.MaxSector = IsoUtils.Vec3DivCeil (iso_internal.ScreenRect.max, _sectorsSize);
				_sectorsMinNumPos = IsoUtils.Vec3Min(_sectorsMinNumPos, iso_internal.MinSector);
				_sectorsMaxNumPos = IsoUtils.Vec3Max(_sectorsMaxNumPos, iso_internal.MaxSector);
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

			var sectors_iter = _sectors.GetEnumerator();
			while ( sectors_iter.MoveNext() ) {
				sectors_iter.Current.Reset();
			}
		}

		void TuneSectors() {
			var visibles_iter = _visibles.GetEnumerator();
			while ( visibles_iter.MoveNext() ) {
				var iso_object = visibles_iter.Current;
				iso_object.Internal.MinSector -= _sectorsMinNumPos;
				iso_object.Internal.MaxSector -= _sectorsMinNumPos;
				var min = iso_object.Internal.MinSector;
				var max = iso_object.Internal.MaxSector;
				for ( var y = min.y; y < max.y; ++y ) {
				for ( var x = min.x; x < max.x; ++x ) {
					var sector = FindSector(new Vector2(x, y));
					if ( sector != null ) {
						sector.objects.Add(iso_object);
					}
				}}
			}
		}
		
		void SetupSectors() {
			ResizeSectors(Mathf.FloorToInt(_sectorsNumPosCount.x * _sectorsNumPosCount.y));
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
			var objects_iter = _objects.GetEnumerator();
			while ( objects_iter.MoveNext() ) {
				var iso_object = objects_iter.Current;
				if ( IsIsoObjectVisible(iso_object) ) {
					iso_object.Internal.Visited = false;
					_oldVisibles.Add(iso_object);
				}
			}
			var old_visibles = _visibles;
			_visibles = _oldVisibles;
			_oldVisibles = old_visibles;
		}

		void ResolveVisibles() {
			var visibles_iter = _visibles.GetEnumerator();
			while ( visibles_iter.MoveNext() ) {
				var iso_object = visibles_iter.Current;
				if ( iso_object.Internal.Dirty || !_oldVisibles.Contains(iso_object) ) {
					MarkDirty();
					SetupIsoObjectDepends(iso_object);
					iso_object.Internal.Dirty = false;
				}
				if ( UpdateIsoObjectBounds3d(iso_object) ) {
					MarkDirty();
				}
			}
			
			var old_visibles_iter = _oldVisibles.GetEnumerator();
			while ( old_visibles_iter.MoveNext() ) {
				var iso_object = old_visibles_iter.Current;
				if ( !_visibles.Contains(iso_object) ) {
					MarkDirty();
					ClearIsoObjectDepends(iso_object);
				}
			}
		}

		void ClearIsoObjectDepends(IsoObject iso_object) {
			var their_depends_iter = iso_object.Internal.TheirDepends.GetEnumerator();
			while ( their_depends_iter.MoveNext() ) {
				var their_iso_object = their_depends_iter.Current;
				if ( !their_iso_object.Internal.Dirty ) {
					their_depends_iter.Current.Internal.SelfDepends.Remove(iso_object);
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
				var v = new Vector2(x, y);
				LookUpSectorDepends(v, obj_a);
				LookUpSectorRDepends(v, obj_a);
			}}
		}

		void PlaceAllVisibles() {
			var depth = startDepth;
			var visibles_iter = _visibles.GetEnumerator();
			while ( visibles_iter.MoveNext() ) {
				depth = RecursivePlaceIsoObject(visibles_iter.Current, depth);
			}
		}

		float RecursivePlaceIsoObject(IsoObject iso_object, float depth) {
			if ( iso_object.Internal.Visited ) {
				return depth;
			}
			iso_object.Internal.Visited = true;
			var self_depends_iter = iso_object.Internal.SelfDepends.GetEnumerator();
			while ( self_depends_iter.MoveNext() ) {
				depth = RecursivePlaceIsoObject(self_depends_iter.Current, depth);
			}

			if ( iso_object.mode == IsoObject.Mode.Mode3d ) {
				var zoffset = iso_object.Internal.Offset3d;
				var extents = iso_object.Internal.Bounds3d.extents.z;
				PlaceIsoObject(iso_object, depth + extents + zoffset);
				return depth + extents * 2.0f + stepDepth;
			} else {
				PlaceIsoObject(iso_object, depth);
				return depth + stepDepth;
			}
		}

		void PlaceIsoObject(IsoObject iso_object, float depth) {
			var trans = iso_object.transform;
			trans.position = IsoUtils.Vec3ChangeZ(trans.position, depth);
		}

		// ------------------------------------------------------------------------
		//
		// Messages
		//
		// ------------------------------------------------------------------------

		void Start() {
			ChangeSortingProperty();
			StepSort();
		}

		void LateUpdate() {
			StepSort();
		}

		void OnEnable() {
			_objects = new HashSet<IsoObject>(FindObjectsOfType<IsoObject>());
			_objects.RemoveWhere(iso_object => !iso_object.enabled);
			_visibles.Clear();
			_sectors.Clear();
			MarkDirty();
		}

		void OnDisable() {
			_objects.Clear();
			_visibles.Clear();
			_sectors.Clear();
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
		#endif
	}
} // namespace IsoTools