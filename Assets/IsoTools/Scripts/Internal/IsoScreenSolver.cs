using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace IsoTools.Internal {
	public class IsoScreenSolver {
		Vector2                           _minIsoXY           = Vector2.zero;

		IsoAssocList<IsoObject>           _oldVisibles        = new IsoAssocList<IsoObject>(47);
		IsoAssocList<IsoObject>           _curVisibles        = new IsoAssocList<IsoObject>(47);

		IsoQuadTree<IsoObject>            _quadTree           = new IsoQuadTree<IsoObject>(47);
		IsoQTBoundsLookUpper              _qtBoundsLU         = new IsoQTBoundsLookUpper();
		IsoQTDependsLookUpper             _qtDependsLU        = new IsoQTDependsLookUpper();
		IsoQTVisibilityLookUpper          _qtVisibilityLU     = new IsoQTVisibilityLookUpper();

		IsoIPool<ParentInfo>              _parentInfoPool     = new ParentInfoPool(47);
		IsoAssocList<ParentInfo>          _parentInfoList     = new IsoAssocList<ParentInfo>(47);
		Dictionary<IsoObject, Transform>  _isoObjectToParent  = new Dictionary<IsoObject, Transform>(47);
		Dictionary<Transform, ParentInfo> _parentToParentInfo = new Dictionary<Transform, ParentInfo>(47);

		// ---------------------------------------------------------------------
		//
		// IsoQTBoundsLookUpper
		//
		// ---------------------------------------------------------------------

		class IsoQTBoundsLookUpper : IsoQuadTree<IsoObject>.IBoundsLookUpper {
			public void LookUp(IsoRect bounds) {
			#if UNITY_EDITOR
				IsoUtils.DrawSolidRect(
					bounds,
					IsoUtils.ColorChangeA(Color.red, 0.05f),
					Color.red);
			#endif
			}
		}

		// ---------------------------------------------------------------------
		//
		// IsoQTDependsLookUpper
		//
		// ---------------------------------------------------------------------

		class IsoQTDependsLookUpper : IsoQuadTree<IsoObject>.IContentLookUpper {
			IsoObject _isoObject;

			public void LookUp(IsoObject other_iso_object) {
				LookUpCellForLDepends(_isoObject, other_iso_object);
				LookUpCellForRDepends(_isoObject, other_iso_object);
			}

			public void LookUpForDepends(
				IsoScreenSolver screen_solver, IsoObject iso_object)
			{
				_isoObject = iso_object;
				if ( iso_object.Internal.QTItem != null ) {
					screen_solver._quadTree.VisitItemsByItem(
						iso_object.Internal.QTItem, this);
				}
				_isoObject = null;
			}
		}

		// ---------------------------------------------------------------------
		//
		// IsoQTVisibilityLookUpper
		//
		// ---------------------------------------------------------------------

		class IsoQTVisibilityLookUpper : IsoQuadTree<IsoObject>.IContentLookUpper {
			Camera[]        _tmpCameras   = new Camera[8];
			IsoScreenSolver _screenSolver = null;

			//
			// Public
			//

			public void LookUp(IsoObject iso_object) {
				iso_object.Internal.Placed = false;
				_screenSolver._oldVisibles.Add(iso_object);
			}

			public void LookUpForVisibility(IsoScreenSolver screen_solver, bool include_scene_view) {
				_screenSolver = screen_solver;
				_screenSolver._oldVisibles.Clear();
				var cam_count = FillLookUpCameras(include_scene_view);
				for ( var i = 0; i < cam_count; ++i ) {
					var tmp_cam = _tmpCameras[i];
					var vp_rect = tmp_cam.rect;
					var wrl_min = tmp_cam.ViewportToWorldPoint(new Vector3(vp_rect.xMin, vp_rect.yMin));
					var wrl_max = tmp_cam.ViewportToWorldPoint(new Vector3(vp_rect.xMax, vp_rect.yMax));
					_screenSolver._quadTree.VisitItemsByBounds(new IsoRect(wrl_min, wrl_max), this);
				}
				ResetLookUpCameras();
				_screenSolver = null;
			}

			//
			// Private
			//

			int FillLookUpCameras(bool include_scene_view) {
				var camera_count = Camera.allCamerasCount;
				if ( _tmpCameras.Length < camera_count + 1 ) {
					_tmpCameras = new Camera[camera_count * 2 + 1];
				}
				Camera.GetAllCameras(_tmpCameras);
				return include_scene_view
					? AddSceneViewCamera(camera_count)
					: camera_count;
			}

			int AddSceneViewCamera(int camera_count) {
			#if UNITY_EDITOR
				var scene_view = SceneView.lastActiveSceneView;
				if ( scene_view && scene_view.camera ) {
					_tmpCameras[camera_count++] = scene_view.camera;
				}
			#endif
				return camera_count;
			}

			void ResetLookUpCameras() {
				System.Array.Clear(_tmpCameras, 0, _tmpCameras.Length);
			}
		}

		// ---------------------------------------------------------------------
		//
		// ParentInfo
		//
		// ---------------------------------------------------------------------

		class ParentInfo {
			public Transform               Parent     = null;
			public Vector3                 LastTrans  = Vector3.zero;
			public IsoAssocList<IsoObject> IsoObjects = new IsoAssocList<IsoObject>();

			public ParentInfo Init(Transform parent) {
				Parent    = parent;
				LastTrans = parent ? parent.position : Vector3.zero;
				return this;
			}

			public ParentInfo Clear() {
				IsoObjects.Clear();
				return Init(null);
			}
		}

		class ParentInfoPool : IsoPool<ParentInfo> {
			public ParentInfoPool(int capacity) : base(capacity) {
			}

			public override ParentInfo CreateItem() {
				return new ParentInfo();
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

		public IsoAssocList<IsoObject> oldVisibles {
			get { return _oldVisibles; }
		}

		public IsoAssocList<IsoObject> curVisibles {
			get { return _curVisibles; }
		}

		// ---------------------------------------------------------------------
		//
		// Callbacks
		//
		// ---------------------------------------------------------------------

		public void OnAddIsoObject(IsoObject iso_object) {
			iso_object.Internal.QTItem = _quadTree.AddItem(
				iso_object.Internal.QTBounds,
				iso_object);
			_minIsoXY = IsoUtils.Vec2Min(_minIsoXY, iso_object.position);
			RegisterIsoObjectParent(iso_object);
		}

		public void OnRemoveIsoObject(IsoObject iso_object) {
			_oldVisibles.Remove(iso_object);
			_curVisibles.Remove(iso_object);
			if ( iso_object.Internal.QTItem != null ) {
				_quadTree.RemoveItem(iso_object.Internal.QTItem);
				iso_object.Internal.QTItem = null;
			}
			ClearIsoObjectDepends(iso_object);
			UnregisterIsoObjectParent(iso_object);
		}

		public void OnMarkDirtyIsoObject(IsoObject iso_object) {
			if ( iso_object.Internal.QTItem != null ) {
				iso_object.Internal.QTItem = _quadTree.MoveItem(
					iso_object.Internal.QTBounds,
					iso_object.Internal.QTItem);
			} else {
				iso_object.Internal.QTItem = _quadTree.AddItem(
					iso_object.Internal.QTBounds,
					iso_object);
			}
			_minIsoXY = IsoUtils.Vec2Min(_minIsoXY, iso_object.position);
			iso_object.Internal.Dirty = true;
		}

		public void OnDrawGizmos(IsoWorld iso_world) {
			if ( iso_world.isShowQuadTree ) {
				_quadTree.VisitAllBounds(_qtBoundsLU);
			}
		}

		// ---------------------------------------------------------------------
		//
		// Public
		//
		// ---------------------------------------------------------------------

		public void StepSortingAction(IsoWorld iso_world) {
			Profiler.BeginSample("IsoScreenSolver.ProcessParents");
			ProcessParents();
			Profiler.EndSample();
			Profiler.BeginSample("IsoScreenSolver.ProcessVisibles");
			ProcessVisibles(iso_world.isSortInSceneView);
			Profiler.EndSample();
		}

		public void Clear() {
			_oldVisibles.Clear();
			_curVisibles.Clear();
			_quadTree.Clear();
		}

		public void SetupIsoObjectDepends(IsoObject iso_object) {
			ClearIsoObjectDepends(iso_object);
			_qtDependsLU.LookUpForDepends(this, iso_object);
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
		// Parents
		//
		// ---------------------------------------------------------------------

		void RegisterIsoObjectParent(IsoObject iso_object) {
			var parent = iso_object ? iso_object.transform.parent : null;
			if ( parent ) {
				ParentInfo parent_info;
				if ( _parentToParentInfo.TryGetValue(parent, out parent_info) ) {
					parent_info.IsoObjects.Add(iso_object);
				} else {
					parent_info = _parentInfoPool.Take().Init(parent);
					parent_info.IsoObjects.Add(iso_object);
					_parentToParentInfo.Add(parent, parent_info);
					_parentInfoList.Add(parent_info);
				}
				_isoObjectToParent.Add(iso_object, parent);
			}
		}

		void UnregisterIsoObjectParent(IsoObject iso_object) {
			Transform parent;
			if ( _isoObjectToParent.TryGetValue(iso_object, out parent) ) {
				ParentInfo parent_info;
				if ( _parentToParentInfo.TryGetValue(parent, out parent_info) ) {
					parent_info.IsoObjects.Remove(iso_object);
					if ( parent_info.IsoObjects.Count == 0 ) {
						_parentToParentInfo.Remove(parent);
						_parentInfoList.Remove(parent_info);
						_parentInfoPool.Release(parent_info.Clear());
					}
				}
				_isoObjectToParent.Remove(iso_object);
			}
		}

		// ---------------------------------------------------------------------
		//
		// Processes
		//
		// ---------------------------------------------------------------------

		void ProcessParents() {
			for ( int i = 0, ie = _parentInfoList.Count; i < ie; ++i ) {
				var parent_info  = _parentInfoList[i];
				var parent_trans = parent_info.Parent.position;
				if ( parent_info.LastTrans != parent_trans ) {
					parent_info.LastTrans = parent_trans;
					for ( int j = 0, je = parent_info.IsoObjects.Count; j < je; ++j ) {
						var iso_object = parent_info.IsoObjects[j];
						iso_object.FixIsoPosition();
					}
				}
			}
		}

		void ProcessVisibles(bool include_scene_view) {
			_qtVisibilityLU.LookUpForVisibility(this, include_scene_view);
			SwapCurrentVisibles();
		}

		void SwapCurrentVisibles() {
			var swap_tmp = _curVisibles;
			_curVisibles = _oldVisibles;
			_oldVisibles = swap_tmp;
		}

		// ---------------------------------------------------------------------
		//
		// LookUpCellForDepends
		//
		// ---------------------------------------------------------------------

		static void LookUpCellForLDepends(IsoObject obj_a, IsoObject obj_b) {
			if ( !obj_b.Internal.Dirty &&
				 obj_a != obj_b &&
				 IsIsoObjectDepends(obj_a.position, obj_a.size, obj_b.position, obj_b.size) )
			{
				obj_a.Internal.SelfDepends.Add(obj_b);
				obj_b.Internal.TheirDepends.Add(obj_a);
			}
		}

		static void LookUpCellForRDepends(IsoObject obj_a, IsoObject obj_b) {
			if ( !obj_b.Internal.Dirty &&
				 obj_a != obj_b &&
				 IsIsoObjectDepends(obj_b.position, obj_b.size, obj_a.position, obj_a.size) )
			{
				obj_b.Internal.SelfDepends.Add(obj_a);
				obj_a.Internal.TheirDepends.Add(obj_b);
			}
		}

		static bool IsIsoObjectDepends(Vector3 a_min, Vector3 a_size, Vector3 b_min, Vector3 b_size) {
			//var a_max = a_min + a_size;
			//var b_max = b_min + b_size;

			var a_min_x  = a_min.x;
			var a_min_y  = a_min.y;
			var a_min_z  = a_min.z;

			var a_size_x = a_size.x;
			var a_size_y = a_size.y;
			var a_size_z = a_size.z;

			var b_min_x  = b_min.x;
			var b_min_y  = b_min.y;
			var b_min_z  = b_min.z;

			var b_size_x = b_size.x;
			var b_size_y = b_size.y;
			var b_size_z = b_size.z;

			var a_max_x  = a_min_x + a_size_x;
			var a_max_y  = a_min_y + a_size_y;
			var a_max_z  = a_min_z + a_size_z;

			var b_max_x  = b_min_x + b_size_x;
			var b_max_y  = b_min_y + b_size_y;
			var b_max_z  = b_min_z + b_size_z;

			var a_yesno = a_max_x > b_min_x && a_max_y > b_min_y && b_max_z > a_min_z;
			if ( a_yesno ) {
				var b_yesno = b_max_x > a_min_x && b_max_y > a_min_y && a_max_z > b_min_z;
				if ( b_yesno ) {
					//var da_p = new Vector3(a_max.x - b_min.x, a_max.y - b_min.y, b_max.z - a_min.z);
					//var db_p = new Vector3(b_max.x - a_min.x, b_max.y - a_min.y, a_max.z - b_min.z);
					//var dp_p = a_size + b_size - IsoUtils.Vec3Abs(da_p - db_p);

					var dA_x = a_max_x - b_min_x;
					var dA_y = a_max_y - b_min_y;
					var dA_z = b_max_z - a_min_z;

					var dB_x = b_max_x - a_min_x;
					var dB_y = b_max_y - a_min_y;
					var dB_z = a_max_z - b_min_z;

					var dD_x = dB_x - dA_x;
					var dD_y = dB_y - dA_y;
					var dD_z = dB_z - dA_z;

					var dP_x = a_size_x + b_size_x - (dD_x < 0.0f ? -dD_x : dD_x);
					var dP_y = a_size_y + b_size_y - (dD_y < 0.0f ? -dD_y : dD_y);
					var dP_z = a_size_z + b_size_z - (dD_z < 0.0f ? -dD_z : dD_z);

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