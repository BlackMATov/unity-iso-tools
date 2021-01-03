using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace IsoTools.Internal {
	[CustomEditor(typeof(IsoParent)), CanEditMultipleObjects]
	public class IsoParentEditor : Editor {
		static bool _showWorldSettings = false;

		Dictionary<IsoWorld, List<IsoParent>> _isoParents   = new Dictionary<IsoWorld, List<IsoParent>>();
		Dictionary<IsoWorld, List<IsoObject>> _isoObjects   = new Dictionary<IsoWorld, List<IsoObject>>();
		Dictionary<IsoWorld, List<IsoObject>> _otherObjects = new Dictionary<IsoWorld, List<IsoObject>>();
		Dictionary<IsoWorld, Vector3>         _viewCenters  = new Dictionary<IsoWorld, Vector3>();

		// ---------------------------------------------------------------------
		//
		//
		//
		// ---------------------------------------------------------------------

		void PrepareTargets() {
			_isoParents = targets
				.OfType<IsoParent>()
				.Where(p => p.isoWorld)
				.GroupBy(p => p.isoWorld)
				.ToDictionary(p => p.Key, p => p.ToList());
			_isoObjects = _isoParents.ToDictionary(
				p => p.Key,
				p => p.Value.SelectMany(t => t.GetComponentsInChildren<IsoObject>(true)).ToList());
			_otherObjects = FindObjectsOfType<IsoObject>()
				.Where(p => p.IsActive() && p.isoWorld)
				.Where(p => _isoObjects.ContainsKey(p.isoWorld))
				.Where(p => !_isoObjects[p.isoWorld].Contains(p))
				.GroupBy(p => p.isoWorld)
				.ToDictionary(p => p.Key, p => p.ToList());
			_viewCenters = _isoParents.ToDictionary(
				pair => pair.Key,
				pair => {
					var iso_world = pair.Key;
					return pair.Value.Aggregate(Vector3.zero, (AccIn, p) => {
						return AccIn + p.transform.position;
					}) / pair.Value.Count;
				});
		}

		void DrawCustomInspector() {
			if ( DrawDetachedInspector() ) {
				return;
			}

			var iso_worlds = _isoObjects.Keys.ToArray();
			if ( iso_worlds.Length == 0 ) {
				return;
			}

			_showWorldSettings = IsoEditorUtils.DoFoldoutHeaderGroup(_showWorldSettings, "World Settings", () => {
				IsoEditorUtils.DrawSelfWorldProperty(iso_worlds);
				IsoEditorUtils.DrawWorldProperties(iso_worlds);
			});
		}

		bool DrawDetachedInspector() {
			var iso_parent = targets.Length == 1 ? target as IsoParent : null;
			var detached = iso_parent && iso_parent.IsActive() && !iso_parent.isoWorld;

			if ( detached ) {
				EditorGUILayout.HelpBox(
					"Detached IsoParent\nNeed to be a child of IsoWorld",
					MessageType.Warning,
					true);
			}

			return detached;
		}

		// ---------------------------------------------------------------------
		//
		//
		//
		// ---------------------------------------------------------------------

		void XYMoveSliderTool(Color color, Vector3 dir) {
			foreach ( var iso_world in _viewCenters.Keys.ToList() ) {
				EditorGUI.BeginChangeCheck();
				var old_center = _viewCenters[iso_world];
				var new_center = IsoEditorUtils.GizmoSlider(
					color,
					old_center,
					iso_world.IsoToScreen(dir));
				if ( EditorGUI.EndChangeCheck() ) {
					Undo.RecordObjects(
						_isoParents[iso_world].Select(p => p.transform).ToArray(),
						_isoParents[iso_world].Count > 1 ? "Move IsoParents" : "Move IsoParent");
					var old_delta = new_center - old_center;
					var new_delta = iso_world.IsoToScreen(IsoEditorUtils.XYMoveIsoObjects(
						false,
						iso_world,
						_isoObjects,
						_otherObjects,
						iso_world.ScreenToIso(old_delta)));
					_viewCenters[iso_world] = old_center + IsoUtils.Vec3FromVec2(new_delta);
					foreach ( var parent in _isoParents[iso_world] ) {
						parent.transform.position += IsoUtils.Vec3FromVec2(new_delta);
					}
					foreach ( var iso_object in _isoObjects[iso_world] ) {
						iso_object.FixIsoPosition();
						iso_object.positionXY = IsoUtils.VectorBeautifier(iso_object.positionXY);
						if ( IsoEditorUtils.IsPrefabInstance(iso_object.gameObject) ) {
							PrefabUtility.RecordPrefabInstancePropertyModifications(iso_object);
						}
					}
				}
			}
		}

		void XYMoveRectangleTool() {
			foreach ( var iso_world in _viewCenters.Keys.ToList() ) {
				EditorGUI.BeginChangeCheck();
				var old_center = _viewCenters[iso_world];
				var new_center = IsoEditorUtils.GizmoRectangle(old_center);
				if ( EditorGUI.EndChangeCheck() ) {
					Undo.RecordObjects(
						_isoParents[iso_world].Select(p => p.transform).ToArray(),
						_isoParents[iso_world].Count > 1 ? "Move IsoParents" : "Move IsoParent");
					var old_delta = new_center - old_center;
					var new_delta = iso_world.IsoToScreen(IsoEditorUtils.XYMoveIsoObjects(
						false,
						iso_world,
						_isoObjects,
						_otherObjects,
						iso_world.ScreenToIso(old_delta)));
					_viewCenters[iso_world] = old_center + IsoUtils.Vec3FromVec2(new_delta);
					foreach ( var parent in _isoParents[iso_world] ) {
						parent.transform.position += IsoUtils.Vec3FromVec2(new_delta);
					}
					foreach ( var iso_object in _isoObjects[iso_world] ) {
						iso_object.FixIsoPosition();
						iso_object.positionXY = IsoUtils.VectorBeautifier(iso_object.positionXY);
						if ( IsoEditorUtils.IsPrefabInstance(iso_object.gameObject) ) {
							PrefabUtility.RecordPrefabInstancePropertyModifications(iso_object);
						}
					}
				}
			}
		}

		// ---------------------------------------------------------------------
		//
		//
		//
		// ---------------------------------------------------------------------

		void DisableCustomTools() {
			if ( Tools.hidden ) {
				Tools.hidden = false;
				Tools.current = Tool.Move;
			}
		}

		void DrawCustomTools() {
			if ( Tools.current == Tool.Move ) {
				Tools.hidden = true;
				XYMoveSliderTool(Handles.xAxisColor, IsoUtils.vec3OneX);
				XYMoveSliderTool(Handles.yAxisColor, IsoUtils.vec3OneY);
				XYMoveRectangleTool();
			} else {
				Tools.hidden = false;
			}
		}

		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		void OnEnable() {
			PrepareTargets();
		}

		void OnDisable() {
			DisableCustomTools();
		}

		void OnSceneGUI() {
			DrawCustomTools();
		}

		public override void OnInspectorGUI() {
			PrepareTargets();
			DrawDefaultInspector();
			DrawCustomInspector();
		}
	}
}