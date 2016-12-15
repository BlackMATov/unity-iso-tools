using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace IsoTools.Internal {
	[CustomEditor(typeof(IsoSnappingParent)), CanEditMultipleObjects]
	public class IsoSnappingParentEditor : Editor {

		IDictionary<IsoSnappingParent, Vector3> _parents      = new Dictionary<IsoSnappingParent, Vector3>();
		IDictionary<IsoObject, Vector3>         _positions    = new Dictionary<IsoObject, Vector3>();
		IList<IsoObject>                        _otherObjects = new List<IsoObject>();
		Vector3                                 _center       = Vector3.zero;
		Vector3                                 _viewCenter   = Vector3.zero;

		void GrabPositions() {
			var iso_world = IsoWorld.Instance;
			if ( iso_world ) {
				_parents = targets
					.Where(p => p is IsoSnappingParent)
					.Select(p => p as IsoSnappingParent)
					.ToDictionary(p => p, p => p.transform.position);
				foreach ( var parent in _parents ) {
					var iso_objects = parent.Key.GetComponentsInChildren<IsoObject>(true);
					foreach ( var iso_object in iso_objects ) {
						_positions[iso_object] = iso_object.transform.position;
					}
				}
				_center = _viewCenter = _parents.Aggregate(Vector3.zero, (AccIn, p) => {
					return AccIn + p.Key.transform.position;
				}) / _parents.Count;
			} else {
				_parents.Clear();
				_positions.Clear();
			}
			_otherObjects = FindObjectsOfType<IsoObject>()
				.Where(p => p.gameObject.activeInHierarchy && !_positions.ContainsKey(p))
				.ToList();
		}

		void DrawWorldEditorProperties() {
			var iso_world = IsoWorld.Instance;
			if ( iso_world ) {
				var so = new SerializedObject(iso_world);
				EditorGUILayout.PropertyField(so.FindProperty("_showIsoBounds"));
				EditorGUILayout.PropertyField(so.FindProperty("_showScreenBounds"));
				EditorGUILayout.PropertyField(so.FindProperty("_showDepends"));
				EditorGUILayout.PropertyField(so.FindProperty("_snappingEnabled"));
				if ( GUI.changed ) {
					so.ApplyModifiedProperties();
				}
			}
		}

		void XYMoveSlider(Color color, Vector3 dir) {
			var iso_world = IsoWorld.Instance;
			if ( iso_world ) {
				Handles.color = color;
				var delta = Handles.Slider(_viewCenter, iso_world.IsoToScreen(dir)) - _viewCenter;
				if ( delta.magnitude > Mathf.Epsilon ) {
					Undo.RecordObjects(
						_parents.Select(p => p.Key.transform).ToArray(),
						_parents.Count > 1 ? "Move IsoSnappingParents" : "Move IsoSnappingParent");
					_viewCenter = _center + IsoObjectEditor.XYMoveIsoObjects(
						false,
						_viewCenter - _center + delta,
						_positions,
						_otherObjects);
					foreach ( var parent in _parents ) {
						parent.Key.transform.position = parent.Value + (_viewCenter - _center);
					}
					foreach ( var pos in _positions ) {
						pos.Key.FixIsoPosition();
						pos.Key.positionXY = IsoUtils.VectorBeautifier(pos.Key.positionXY);
					}
				}
			}
		}

		void XYMoveRectangle() {
			var iso_world = IsoWorld.Instance;
			if ( iso_world ) {
				Handles.color = new Color(
					Handles.zAxisColor.r,
					Handles.zAxisColor.g,
					Handles.zAxisColor.b,
					0.3f);
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
					Undo.RecordObjects(
						_parents.Select(p => p.Key.transform).ToArray(),
						_parents.Count > 1 ? "Move IsoSnappingParents" : "Move IsoSnappingParent");
					_viewCenter = _center + IsoObjectEditor.XYMoveIsoObjects(
						false,
						_viewCenter - _center + delta,
						_positions,
						_otherObjects);
					foreach ( var parent in _parents ) {
						parent.Key.transform.position = parent.Value + (_viewCenter - _center);
					}
					foreach ( var pos in _positions ) {
						pos.Key.FixIsoPosition();
						pos.Key.positionXY = IsoUtils.VectorBeautifier(pos.Key.positionXY);
					}
				}
			}
		}

		//
		//
		//

		void OnEnable() {
			GrabPositions();
		}

		void OnDisable() {
			if ( Tools.hidden ) {
				Tools.hidden = false;
				Tools.current = Tool.Move;
			}
		}

		void OnSceneGUI() {
			if ( Tools.current == Tool.Move ) {
				Tools.hidden = true;
				XYMoveSlider(Handles.xAxisColor, IsoUtils.vec3OneX);
				XYMoveSlider(Handles.yAxisColor, IsoUtils.vec3OneY);
				XYMoveRectangle();
			} else {
				Tools.hidden = false;
			}
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			GrabPositions();
			DrawWorldEditorProperties();
		}
	}
}