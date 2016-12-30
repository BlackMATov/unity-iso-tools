﻿using UnityEngine;
using IsoTools.Internal;

namespace IsoTools {
	[ExecuteInEditMode, DisallowMultipleComponent]
	public sealed class IsoWorld : IsoWorldBase {

		Matrix4x4        _isoMatrix     = Matrix4x4.identity;
		Matrix4x4        _isoRMatrix    = Matrix4x4.identity;
		IsoScreenSolver  _screenSolver  = new IsoScreenSolver();
		IsoSortingSolver _sortingSolver = new IsoSortingSolver();
		IsoWarningSolver _warningSolver = new IsoWarningSolver();

		// ---------------------------------------------------------------------
		//
		// Constants
		//
		// ---------------------------------------------------------------------

		public const float DefTileSize     = 32.0f;
		public const float MinTileSize     = float.Epsilon;
		public const float MaxTileSize     = float.MaxValue;

		public const float DefTileRatio    = 0.5f;
		public const float MinTileRatio    = 0.25f;
		public const float MaxTileRatio    = 1.0f;

		public const float DefTileAngle    = 45.0f;
		public const float MinTileAngle    = 0.0f;
		public const float MaxTileAngle    = 90.0f;

		public const float DefTileHeight   = DefTileSize;
		public const float MinTileHeight   = MinTileSize;
		public const float MaxTileHeight   = MaxTileSize;

		public const float DefStepDepth    = 0.1f;
		public const float MinStepDepth    = float.Epsilon;
		public const float MaxStepDepth    = float.MaxValue;

		public const float DefStartDepth   = 1.0f;
		public const float MinStartDepth   = float.MinValue;
		public const float MaxStartDepth   = float.MaxValue;

		public const float DefSnapDistance = 0.2f;
		public const float MinSnapDistance = 0.0f;
		public const float MaxSnapDistance = 0.5f;

		// ---------------------------------------------------------------------
		//
		// Sorting properties
		//
		// ---------------------------------------------------------------------

		[Header("World Settings")]
		[SerializeField]
		float _tileSize = DefTileSize;
		public float tileSize {
			get { return _tileSize; }
			set {
				_tileSize = Mathf.Clamp(value, MinTileSize, MaxTileSize);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		float _tileRatio = DefTileRatio;
		public float tileRatio {
			get { return _tileRatio; }
			set {
				_tileRatio = Mathf.Clamp(value, MinTileRatio, MaxTileRatio);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		float _tileAngle = DefTileAngle;
		public float tileAngle {
			get { return _tileAngle; }
			set {
				_tileAngle = Mathf.Clamp(value, MinTileAngle, MaxTileAngle);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		float _tileHeight = DefTileHeight;
		public float tileHeight {
			get { return _tileHeight; }
			set {
				_tileHeight = Mathf.Clamp(value, MinTileHeight, MaxTileHeight);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		float _stepDepth = DefStepDepth;
		public float stepDepth {
			get { return _stepDepth; }
			set {
				_stepDepth = Mathf.Clamp(value, MinStepDepth, MaxStepDepth);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		float _startDepth = DefStartDepth;
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
			var screen_pos = _isoMatrix.MultiplyPoint(iso_pnt);
			return new Vector2(
				screen_pos.x,
				screen_pos.y + iso_pnt.z * tileHeight);
		}

		public Vector3 ScreenToIso(Vector2 pos) {
			return _isoRMatrix.MultiplyPoint(
				new Vector3(pos.x, pos.y, 0.0f));
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

			var min_iso_xy      = _screenSolver.minIsoXY;
			var min_screen_pnt  = IsoToScreen(min_iso_xy - Vector2.one);
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
			var camera = Camera.main;
			if ( !camera ) {
				throw new UnityException("Main camera not found!");
			}
			return TouchIsoPosition(finger_id, camera, iso_z);
		}

		public Vector3 TouchIsoPosition(int finger_id, Camera camera) {
			return TouchIsoPosition(finger_id, camera, 0.0f);
		}

		public Vector3 TouchIsoPosition(int finger_id, Camera camera, float iso_z) {
			if ( !camera ) {
				throw new UnityException("Camera argument is null!");
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
			var camera = Camera.main;
			if ( !camera ) {
				throw new UnityException("Main camera not found!");
			}
			return MouseIsoPosition(camera, iso_z);
		}

		public Vector3 MouseIsoPosition(Camera camera) {
			return MouseIsoPosition(camera, 0.0f);
		}

		public Vector3 MouseIsoPosition(Camera camera, float iso_z) {
			if ( !camera ) {
				throw new UnityException("Camera argument is null!");
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
		[SerializeField] bool _sortInSceneView = true;
		public bool isSortInSceneView {
			get { return _sortInSceneView; }
			set { _sortInSceneView = value; }
		}
		[SerializeField]
		[Range(MinSnapDistance, MaxSnapDistance)]
		float _snappingDistance = DefSnapDistance;
		public float snappingDistance {
			get { return _snappingDistance; }
			set { _snappingDistance = Mathf.Clamp(value, MinSnapDistance, MaxSnapDistance); }
		}
		[Header("Development Only")]
		[SerializeField] bool _showDepends = false;
		public bool isShowDepends {
			get { return _showDepends; }
			set { _showDepends = value; }
		}
		[SerializeField] bool _showQuadTree = false;
		public bool isShowQuadTree {
			get { return _showQuadTree; }
			set { _showQuadTree = value; }
		}

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		public void Internal_MarkDirty(IsoObject iso_object) {
			if ( _screenSolver.OnMarkDirtyIsoObject(iso_object) ) {
				Internal_SetDirtyInEditorMode();
			}
			if ( _sortingSolver.OnMarkDirtyIsoObject(iso_object) ) {
				Internal_SetDirtyInEditorMode();
			}
			if ( _warningSolver.OnMarkDirtyIsoObject(iso_object) ) {
				Internal_SetDirtyInEditorMode();
			}
		}

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		void UpdateIsoMatrix() {
			_isoMatrix =
				Matrix4x4.Scale(new Vector3(1.0f, tileRatio, 1.0f)) *
				Matrix4x4.TRS(
					Vector3.zero,
					Quaternion.AngleAxis(90.0f - tileAngle, IsoUtils.vec3OneZ),
					new Vector3(tileSize * Mathf.Sqrt(2), tileSize * Mathf.Sqrt(2), tileHeight));
			_isoRMatrix = _isoMatrix.inverse;
		}

		void FixIsoObjectTransforms() {
			var iso_objects = GetIsoObjects();
			for ( int i = 0, e = iso_objects.Count; i < e; ++i ) {
				iso_objects[i].FixTransform();
			}
		}

		void ChangeSortingProperty() {
			UpdateIsoMatrix();
			FixIsoObjectTransforms();
			Internal_SetDirtyInEditorMode();
		}

		void StepSortingProcess() {
			_screenSolver.StepSortingAction(this);
			if ( _sortingSolver.StepSortingAction(this, _screenSolver) ) {
				Internal_SetDirtyInEditorMode();
			}
			_warningSolver.StepSortingAction(this);
		}

		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		void Start() {
			ChangeSortingProperty();
			StepSortingProcess();
		}

		void LateUpdate() {
			StepSortingProcess();
		}

		protected override void OnEnable() {
			base.OnEnable();
		}

		protected override void OnDisable() {
			base.OnDisable();
			_screenSolver.Clear();
			_sortingSolver.Clear();
			_warningSolver.Clear();
		}

		protected override void OnAddIsoObjectToWorld(IsoObject iso_object) {
			base.OnAddIsoObjectToWorld(iso_object);
			_screenSolver.OnAddIsoObject(iso_object);
			_sortingSolver.OnAddIsoObject(iso_object);
			_warningSolver.OnAddIsoObject(iso_object);
		}

		protected override void OnRemoveIsoObjectFromWorld(IsoObject iso_object) {
			base.OnRemoveIsoObjectFromWorld(iso_object);
			_screenSolver.OnRemoveIsoObject(iso_object);
			_sortingSolver.OnRemoveIsoObject(iso_object);
			_warningSolver.OnRemoveIsoObject(iso_object);
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
			var camera = Camera.current;
			if ( camera && camera.name == "SceneCamera" ) {
				Internal_SetDirtyInEditorMode();
			}
		}

		void OnDrawGizmos() {
			_screenSolver.OnDrawGizmos(this);
			_sortingSolver.OnDrawGizmos(this);
			_warningSolver.OnDrawGizmos(this);
		}
	#endif
	}
}