using UnityEngine;
using IsoTools.Internal;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[ExecuteInEditMode, DisallowMultipleComponent]
	public sealed class IsoWorld : IsoHolder<IsoWorld, IsoObject> {

		Matrix4x4        _isoMatrix     = Matrix4x4.identity;
		Matrix4x4        _isoRMatrix    = Matrix4x4.identity;
		IsoScreenSolver  _screenSolver  = new IsoScreenSolver();
		IsoSortingSolver _sortingSolver = new IsoSortingSolver();

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
	#endif

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		public void MarkDirty(IsoObject iso_object) {
			if ( _screenSolver.OnMarkDirtyInstance(iso_object) ) {
				MarkDirty();
			}
			if ( _sortingSolver.OnMarkDirtyInstance(iso_object) ) {
				MarkDirty();
			}
		}

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		void MarkDirty() {
		#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
		#endif
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

		void FixInstanceTransforms() {
			var instances = GetInstances();
			for ( int i = 0, e = instances.Count; i < e; ++i ) {
				instances[i].FixTransform();
			}
		}

		void ChangeSortingProperty() {
			MarkDirty();
			UpdateIsoMatrix();
			FixInstanceTransforms();
		}

		void StepSortingProcess() {
			_screenSolver.StepSortingAction(this, GetInstances());
			if ( _sortingSolver.StepSortingAction(this, _screenSolver) ) {
				MarkDirty();
			}
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
		}

		protected override void OnAddInstanceToHolder(IsoObject instance) {
			base.OnAddInstanceToHolder(instance);
			_screenSolver.OnAddInstance(instance);
			_sortingSolver.OnAddInstance(instance);
		}

		protected override void OnRemoveInstanceFromHolder(IsoObject instance) {
			base.OnRemoveInstanceFromHolder(instance);
			_screenSolver.OnRemoveInstance(instance);
			_sortingSolver.OnRemoveInstance(instance);
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
				MarkDirty();
			}
		}

		void OnDrawGizmos() {
			_screenSolver.OnDrawGizmos(this);
			_sortingSolver.OnDrawGizmos();
		}
	#endif
	}
}