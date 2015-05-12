using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace IsoTools {
	[CustomEditor(typeof(IsoObject)), CanEditMultipleObjects]
	class IsoObjectEditor : Editor {

		IDictionary<IsoObject, Vector3> _positions      = new Dictionary<IsoObject, Vector3>();
		IDictionary<IsoObject, float>   _iso_zpositions = new Dictionary<IsoObject, float>();
		Vector3                         _center         = Vector3.zero;
		Vector3                         _viewCenter     = Vector3.zero;

		void GrabPositions() {
			_positions = targets
				.Where(p => p as IsoObject)
				.Select(p => p as IsoObject)
				.ToDictionary(p => p, p => p.transform.position);
			_iso_zpositions = targets
				.Where(p => p as IsoObject)
				.Select(p => p as IsoObject)
				.ToDictionary(p => p, p => p.Position.z);
			_center = _viewCenter = _positions.Aggregate(Vector3.zero, (AccIn, p) => {
				return AccIn + p.Value;
			}) / _positions.Count;
		}

		float ZMoveIsoObjects(float delta) {
			Undo.RecordObjects(_iso_zpositions.Keys.ToArray(), "Move");
			return _iso_zpositions.Aggregate(0.0f, (AccIn, pair) => {
				var iso_object = pair.Key;
				var iso_orig_z = pair.Value;
				iso_object.PositionZ = iso_orig_z + delta;
				iso_object.FixTransform();
				var z_delta = iso_object.Position.z - iso_orig_z;
				return Mathf.Abs(z_delta) > Mathf.Abs(AccIn) ? z_delta : AccIn;
			});
		}

		Vector3 XYMoveIsoObjects(Vector3 delta) {
			Undo.RecordObjects(_positions.Keys.ToArray(), "Move");
			return _positions.Aggregate(Vector3.zero, (AccIn, pair) => {
				var iso_object = pair.Key;
				var iso_orig_p = pair.Value;
				iso_object.transform.position = iso_orig_p + delta;
				iso_object.FixIsoPosition();
				var pos_delta = iso_object.transform.position - iso_orig_p;
				return pos_delta.magnitude > AccIn.magnitude ? pos_delta : AccIn;
			});
		}

		void ZMoveSlider() {
			var iso_world = GameObject.FindObjectOfType<IsoWorld>();
			if ( iso_world ) {
				Handles.color = Handles.zAxisColor;
				var delta = Handles.Slider(_viewCenter, IsoUtils.Vec3OneY) - _viewCenter;
				if ( Mathf.Abs(delta.y) > Mathf.Epsilon ) {
					float tmp_y = ZMoveIsoObjects((_viewCenter.y - _center.y + delta.y) / iso_world.TileSize);
					_viewCenter = _center + IsoUtils.Vec3FromY(tmp_y * iso_world.TileSize);
				}
			}
		}

		void XYMoveSlider(Color color, Vector3 dir) {
			var iso_world = GameObject.FindObjectOfType<IsoWorld>();
			if ( iso_world ) {
				Handles.color = color;
				var delta = Handles.Slider(_viewCenter, iso_world.IsoToScreen(dir)) - _viewCenter;
				if ( delta.magnitude > Mathf.Epsilon ) {
					_viewCenter = _center + XYMoveIsoObjects(_viewCenter - _center + delta);
				}
			}
		}

		void XYMoveRectangle() {
			Handles.color = IsoUtils.ColorChangeA(Handles.zAxisColor, 0.3f);
			Handles.DotCap(
				0,
				_viewCenter,
				Quaternion.identity,
				HandleUtility.GetHandleSize(_viewCenter) * 0.15f);
			Handles.color = Handles.zAxisColor;
			Handles.ArrowCap(
				0,
				_viewCenter,
				Quaternion.identity,
				HandleUtility.GetHandleSize(_viewCenter));
			Handles.color = Handles.zAxisColor;
			var delta = Handles.FreeMoveHandle(
				_viewCenter,
				Quaternion.identity,
				HandleUtility.GetHandleSize(_viewCenter) * 0.15f,
				Vector3.zero,
				Handles.RectangleCap) - _viewCenter;
			if ( delta.magnitude > Mathf.Epsilon ) {
				_viewCenter = _center + XYMoveIsoObjects(_viewCenter - _center + delta);
			}
		}

		void OnEnable() {
			GrabPositions();
		}

		void OnDisable() {
			Tools.hidden = false;
		}

		void OnSceneGUI() {
			if ( Tools.current == Tool.Move ) {
				Tools.hidden = true;
				ZMoveSlider();
				XYMoveSlider(Handles.xAxisColor, IsoUtils.Vec3OneX);
				XYMoveSlider(Handles.yAxisColor, IsoUtils.Vec3OneY);
				XYMoveRectangle();
			} else {
				Tools.hidden = false;
			}
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			GrabPositions();
		}
	}
} // namespace IsoTools