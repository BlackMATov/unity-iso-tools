﻿using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace IsoTools.Internal {
	public class IsoScreenSolver {
		Vector2                 _minIsoXY    = Vector2.zero;

		IsoAssocList<IsoObject> _oldVisibles = new IsoAssocList<IsoObject>();
		IsoAssocList<IsoObject> _curVisibles = new IsoAssocList<IsoObject>();

		IsoQuadTree<IsoObject>  _quadTree    = new IsoQuadTree<IsoObject>(47);
		IsoGrid<IsoObject>      _screenGrid  = new IsoGrid<IsoObject>(new IsoSGItemAdapter(), 47);

		IsoQTBoundsLookUpper    _qtBoundsLU  = new IsoQTBoundsLookUpper();
		IsoQTContentLookUpper   _qtContentLU = new IsoQTContentLookUpper();

		IsoSGBoundsLookUpper    _sgBoundsLU  = new IsoSGBoundsLookUpper();
		IsoSGContentLookUpper   _sgContentLU = new IsoSGContentLookUpper();

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
		// IsoQTContentLookUpper
		//
		// ---------------------------------------------------------------------

		class IsoQTContentLookUpper : IsoQuadTree<IsoObject>.IContentLookUpper {
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
		// IsoSGItemAdapter
		//
		// ---------------------------------------------------------------------

		class IsoSGItemAdapter : IsoGrid<IsoObject>.IItemAdapter {
			public IsoRect GetBounds(IsoObject item) {
				return item.Internal.ScreenBounds;
			}

			public void SetMinMaxCells(IsoObject item, IsoPoint2 min, IsoPoint2 max) {
				item.Internal.MinGridCell = min;
				item.Internal.MaxGridCell = max;
			}

			public void GetMinMaxCells(IsoObject item, ref IsoPoint2 min, ref IsoPoint2 max) {
				min = item.Internal.MinGridCell;
				max = item.Internal.MaxGridCell;
			}
		}

		// ---------------------------------------------------------------------
		//
		// IsoSGBoundsLookUpper
		//
		// ---------------------------------------------------------------------

		class IsoSGBoundsLookUpper : IsoGrid<IsoObject>.IBoundsLookUpper {
			public void LookUp(IsoRect bounds) {
			#if UNITY_EDITOR
				IsoUtils.DrawSolidRect(
					bounds,
					IsoUtils.ColorChangeA(Color.green, 0.1f),
					Color.green);
			#endif
			}
		}

		// ---------------------------------------------------------------------
		//
		// IsoSGContentLookUpper
		//
		// ---------------------------------------------------------------------

		class IsoSGContentLookUpper : IsoGrid<IsoObject>.IContentLookUpper {
			IsoObject _isoObject;

			public void LookUp(IsoList<IsoObject> items) {
				LookUpCellForLDepends(_isoObject, items);
				LookUpCellForRDepends(_isoObject, items);
			}

			public void LookUpForDepends(
				IsoScreenSolver screen_solver, IsoObject iso_object)
			{
				_isoObject = iso_object;
				screen_solver._screenGrid.VisitItemsByCells(
					iso_object.Internal.MinGridCell,
					iso_object.Internal.MaxGridCell,
					this);
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

		public void OnAddInstance(IsoObject iso_object) {
			_quadTree.AddItem(
				iso_object.Internal.ScreenBounds,
				iso_object);
			_minIsoXY = IsoUtils.Vec2Min(_minIsoXY, iso_object.position);
		}

		public void OnRemoveInstance(IsoObject iso_object) {
			_oldVisibles.Remove(iso_object);
			_curVisibles.Remove(iso_object);
			_quadTree.RemoveItem(iso_object);
			ClearIsoObjectDepends(iso_object);
		}

		public bool OnMarkDirtyInstance(IsoObject iso_object) {
			_quadTree.MoveItem(
				iso_object.Internal.ScreenBounds,
				iso_object);
			_minIsoXY = IsoUtils.Vec2Min(_minIsoXY, iso_object.position);
			if ( !iso_object.Internal.Dirty && _curVisibles.Contains(iso_object) ) {
				iso_object.Internal.Dirty = true;
				return true;
			}
			return false;
		}

	#if UNITY_EDITOR
		public void OnDrawGizmos(IsoWorld iso_world) {
			if ( iso_world.isShowQuadTree ) {
				_quadTree.VisitAllBounds(_qtBoundsLU);
			}
			if ( iso_world.isShowScreenGrid ) {
				_screenGrid.VisitAllBounds(_sgBoundsLU);
			}
		}
	#endif

		// ---------------------------------------------------------------------
		//
		// Functions
		//
		// ---------------------------------------------------------------------

		public void StepSortingAction(IsoWorld iso_world, IsoAssocList<IsoObject> instances) {
			Profiler.BeginSample("IsoScreenSolver.ResolveVisibles");
			ResolveVisibles(iso_world, instances);
			Profiler.EndSample();
			Profiler.BeginSample("IsoScreenSolver.ResolveVisibleGrid");
			ResolveVisibleGrid(iso_world);
			Profiler.EndSample();
		}

		public void PostStepSortingAction() {
		}

		public void Clear() {
			_oldVisibles.Clear();
			_curVisibles.Clear();
			_quadTree.Clear();
			_screenGrid.ClearGrid();
		}

		public void SetupIsoObjectDepends(IsoObject iso_object) {
			ClearIsoObjectDepends(iso_object);
			_sgContentLU.LookUpForDepends(this, iso_object);
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

		void ResolveVisibles(IsoWorld iso_world, IsoAssocList<IsoObject> instances) {
			Profiler.BeginSample("ProcessAllInstances");
			ProcessAllInstances(instances);
			Profiler.EndSample();
			Profiler.BeginSample("ProcessNewVisibles");
			ProcessNewVisibles(iso_world.isSortInSceneView);
			Profiler.EndSample();
		}

		void ProcessAllInstances(IsoAssocList<IsoObject> instances) {
			if ( instances.Count > 0 ) {
				for ( int i = 0, e = instances.Count; i < e; ++i ) {
					var iso_object = instances[i];
					if ( !IsoUtils.Vec2Approximately(
						iso_object.Internal.LastTrans,
						iso_object.Internal.Transform.position) )
					{
						iso_object.FixIsoPosition();
					}
				}
			}
		}

		void ProcessNewVisibles(bool include_scene_view) {
			_oldVisibles.Clear();
			_qtContentLU.LookUpForVisibility(this, include_scene_view);
			SwapCurrentVisibles();
		}

		void SwapCurrentVisibles() {
			var swap_tmp = _curVisibles;
			_curVisibles = _oldVisibles;
			_oldVisibles = swap_tmp;
		}

		// ---------------------------------------------------------------------
		//
		// ResolveVisibleGrid
		//
		// ---------------------------------------------------------------------

		void ResolveVisibleGrid(IsoWorld iso_world) {
			_screenGrid.ClearItems();
			for ( int i = 0, e = _curVisibles.Count; i < e; ++i ) {
				var iso_object = _curVisibles[i];
				_screenGrid.AddItem(iso_object);
			}
			var min_sector_size = IsoUtils.Vec2MaxF(
				iso_world.IsoToScreen(IsoUtils.vec3OneXY) -
				iso_world.IsoToScreen(Vector3.zero));
			_screenGrid.RebuildGrid(min_sector_size);
		}

		// ---------------------------------------------------------------------
		//
		// LookUpCellForDepends
		//
		// ---------------------------------------------------------------------

		static void LookUpCellForLDepends(IsoObject obj_a, IsoList<IsoObject> others) {
			for ( int i = 0, e = others.Count; i < e; ++i ) {
				var obj_b = others[i];
				if ( obj_a != obj_b && !obj_b.Internal.Dirty && IsIsoObjectDepends(obj_a, obj_b) ) {
					obj_a.Internal.SelfDepends.Add(obj_b);
					obj_b.Internal.TheirDepends.Add(obj_a);
				}
			}
		}

		static void LookUpCellForRDepends(IsoObject obj_a, IsoList<IsoObject> others) {
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

					var dD_x = dB_x - dA_x;
					var dD_y = dB_y - dA_y;
					var dD_z = dB_z - dA_z;

					var dP_x = a_size.x + b_size.x - (dD_x > 0.0f ? dD_x : -dD_x);
					var dP_y = a_size.y + b_size.y - (dD_y > 0.0f ? dD_y : -dD_y);
					var dP_z = a_size.z + b_size.z - (dD_z > 0.0f ? dD_z : -dD_z);

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