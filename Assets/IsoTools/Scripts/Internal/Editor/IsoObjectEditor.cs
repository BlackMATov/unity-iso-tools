using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace IsoTools.Internal {
	[CustomEditor(typeof(IsoObject)), CanEditMultipleObjects]
	class IsoObjectEditor : Editor {

		IDictionary<IsoObject, Vector3> _positions     = new Dictionary<IsoObject, Vector3>();
		IDictionary<IsoObject, float>   _isoZPositions = new Dictionary<IsoObject, float>();
		IList<IsoObject>                _otherObjects  = new List<IsoObject>();
		Vector3                         _center        = Vector3.zero;
		Vector3                         _viewCenter    = Vector3.zero;

		public static readonly float SnappingDistance = 0.2f;

		static bool IsSnappingEnabled() {
			return !Event.current.control;
		}

		void GrabPositions() {
			var iso_world = IsoWorld.Instance;
			if ( iso_world ) {
				_positions = targets
					.Where(p => p is IsoObject)
					.Select(p => p as IsoObject)
					.ToDictionary(p => p, p => p.transform.position);
				_isoZPositions = targets
					.Where(p => p is IsoObject)
					.Select(p => p as IsoObject)
					.ToDictionary(p => p, p => p.position.z);
				_center = _viewCenter = _positions.Aggregate(Vector3.zero, (AccIn, p) => {
					return AccIn + IsoUtils.Vec3FromVec2(iso_world.IsoToScreen(p.Key.position + p.Key.size * 0.5f));
				}) / _positions.Count;
			} else {
				_positions.Clear();
				_isoZPositions.Clear();
			}
			_otherObjects = FindObjectsOfType<IsoObject>()
				.Where(p => p.gameObject.activeInHierarchy && !_positions.ContainsKey(p))
				.ToList();
		}

		void DirtyTargetPosition() {
			if ( targets.Length == 1 && (target is IsoObject) && (target as IsoObject).gameObject.activeInHierarchy ) {
				var position_prop = serializedObject.FindProperty("_position");
				if ( position_prop != null ) {
					var last_value = position_prop.vector3Value;
					position_prop.vector3Value = last_value + Vector3.one;
					PrefabUtility.RecordPrefabInstancePropertyModifications(target);
					serializedObject.ApplyModifiedProperties();
					position_prop.vector3Value = last_value;
					PrefabUtility.RecordPrefabInstancePropertyModifications(target);
					serializedObject.ApplyModifiedProperties();
				}
			}
		}

		void DrawWorldEditorProperties() {
			var iso_world = IsoWorld.Instance;
			if ( iso_world ) {
				var so = new SerializedObject(iso_world);
				EditorGUILayout.PropertyField(so.FindProperty("_showIsoBounds"));
				EditorGUILayout.PropertyField(so.FindProperty("_showScreenBounds"));
				EditorGUILayout.PropertyField(so.FindProperty("_showDepends"));
				if ( GUI.changed ) {
					so.ApplyModifiedProperties();
				}
			}
		}

		static bool SnappingProcess(ref float min_a, float size_a, float min_b, float size_b) {
			var max_a  = min_a + size_a;
			var max_b  = min_b + size_b;
			var result = false;
			if ( IsSnappingIntersect(min_a, size_a, min_b, size_b) ) {
				// |min_a max_a|min_b max_b|
				if ( Mathf.Abs(max_a - min_b) < SnappingDistance ) {
					min_a  = min_b - size_a;
					result = true;
				}
				// |min_b max_b|min_a max_a|
				if ( Mathf.Abs(max_b - min_a) < SnappingDistance ) {
					min_a  = max_b;
					result = true;
				}
				// |min_a_____max_a|
				// |min_b__max_b|
				if ( Mathf.Abs(min_a - min_b) < SnappingDistance ) {
					min_a  = min_b;
					result = true;
				}
				// |min_a_____max_a|
				//    |min_b__max_b|
				if ( Mathf.Abs(max_a - max_b) < SnappingDistance ) {
					min_a  = max_b - size_a;
					result = true;
				}
			}
			return result;
		}

		static bool IsSnappingIntersect(float min_a, float size_a, float min_b, float size_b) {
			return
				min_a + size_a + SnappingDistance >= min_b &&
				min_a - SnappingDistance <= min_b + size_b;
		}

		public static float ZMoveIsoObjects(
			bool                          move,
			float                         delta,
			IDictionary<IsoObject, float> iso_z_positions,
			IList<IsoObject>              other_objects)
		{
			if ( move ) {
				Undo.RecordObjects(
					iso_z_positions.Keys.ToArray(),
					iso_z_positions.Count > 1 ? "Move IsoObjects" : "Move IsoObject");
			}
			if ( IsSnappingEnabled() ) {
				var snapping_z = false;
				foreach ( var pair in iso_z_positions ) {
					var iso_object = pair.Key;
					var iso_orig_z = pair.Value;
					var result_p_z = iso_orig_z + delta;
					foreach ( var other in other_objects ) {
						if ( IsSnappingIntersect(iso_object.positionX, iso_object.sizeX, other.positionX, other.sizeX) &&
							 IsSnappingIntersect(iso_object.positionY, iso_object.sizeY, other.positionY, other.sizeY) )
						{
							var new_snapping_z = !snapping_z && IsSnappingIntersect(result_p_z, iso_object.sizeZ, other.positionZ, other.sizeZ)
								? SnappingProcess(ref result_p_z, iso_object.sizeZ, other.positionZ, other.sizeZ)
								: false;
							if ( new_snapping_z ) {
								delta      = result_p_z - iso_orig_z;
								snapping_z = true;
							}
						}
					}
					if ( snapping_z ) {
						break;
					}
				}
				if ( !snapping_z ) {
					var pair       = iso_z_positions.First();
					var iso_object = pair.Key;
					var iso_orig_z = pair.Value;
					var result_p_z = iso_orig_z + delta;
					var new_snapping_z = SnappingProcess(ref result_p_z, iso_object.sizeZ, iso_object.tilePositionZ, 1.0f);
					if ( new_snapping_z ) {
						delta      = result_p_z - iso_orig_z;
						snapping_z = true;
					}
				}
			}
			return iso_z_positions.Aggregate(0.0f, (AccIn, pair) => {
				var iso_object = pair.Key;
				var iso_orig_z = pair.Value;
				var result_p_z = iso_orig_z + delta;
				if ( move ) {
					iso_object.positionZ = IsoUtils.FloatBeautifier(result_p_z);
				}
				var z_delta = result_p_z - iso_orig_z;
				return Mathf.Abs(z_delta) > Mathf.Abs(AccIn) ? z_delta : AccIn;
			});
		}

		public static Vector3 XYMoveIsoObjects(
			bool                            move,
			Vector3                         delta,
			IDictionary<IsoObject, Vector3> positions,
			IList<IsoObject>                other_objects)
		{
			if ( move ) {
				Undo.RecordObjects(
					positions.Keys.ToArray(),
					positions.Count > 1 ? "Move IsoObjects" : "Move IsoObject");
			}
			if ( IsSnappingEnabled() ) {
				var snapping_x = false;
				var snapping_y = false;
				foreach ( var pair in positions ) {
					var iso_object     = pair.Key;
					var iso_orig_p     = pair.Value;
					var result_pos     = iso_orig_p + delta;
					var result_pos_iso = IsoWorld.Instance.ScreenToIso(result_pos, iso_object.positionZ);
					foreach ( var other in other_objects ) {
						if ( IsSnappingIntersect(iso_object.positionZ, iso_object.sizeZ, other.positionZ, other.sizeZ) ) {
							var new_snapping_x = !snapping_x && IsSnappingIntersect(result_pos_iso.y, iso_object.sizeY, other.positionY, other.sizeY)
								? SnappingProcess(ref result_pos_iso.x, iso_object.sizeX, other.positionX, other.sizeX)
								: false;
							var new_snapping_y = !snapping_y && IsSnappingIntersect(result_pos_iso.x, iso_object.sizeX, other.positionX, other.sizeX)
								? SnappingProcess(ref result_pos_iso.y, iso_object.sizeY, other.positionY, other.sizeY)
								: false;
							if ( new_snapping_x || new_snapping_y ) {
								result_pos = IsoWorld.Instance.IsoToScreen(result_pos_iso);
								if ( new_snapping_x ) {
									delta.x    = result_pos.x - iso_orig_p.x;
									delta.y    = result_pos.y - iso_orig_p.y;
									snapping_x = true;
								}
								if ( new_snapping_y ) {
									delta.x    = result_pos.x - iso_orig_p.x;
									delta.y    = result_pos.y - iso_orig_p.y;
									snapping_y = true;
								}
							}
						}
					}
					if ( snapping_x && snapping_y ) {
						break;
					}
				}
				if ( !snapping_x && !snapping_y ) {
					var pair           = positions.First();
					var iso_object     = pair.Key;
					var iso_orig_p     = pair.Value;
					var result_pos     = iso_orig_p + delta;
					var result_pos_iso = IsoWorld.Instance.ScreenToIso(result_pos, iso_object.positionZ);
					var new_snapping_x = SnappingProcess(ref result_pos_iso.x, iso_object.sizeX, iso_object.tilePositionX, 1.0f);
					var new_snapping_y = SnappingProcess(ref result_pos_iso.y, iso_object.sizeY, iso_object.tilePositionY, 1.0f);
					if ( new_snapping_x || new_snapping_y ) {
						result_pos = IsoWorld.Instance.IsoToScreen(result_pos_iso);
						if ( new_snapping_x ) {
							delta.x    = result_pos.x - iso_orig_p.x;
							delta.y    = result_pos.y - iso_orig_p.y;
							snapping_x = true;
						}
						if ( new_snapping_y ) {
							delta.x    = result_pos.x - iso_orig_p.x;
							delta.y    = result_pos.y - iso_orig_p.y;
							snapping_y = true;
						}
					}
				}
			}
			return positions.Aggregate(Vector3.zero, (AccIn, pair) => {
				var iso_object = pair.Key;
				var iso_orig_p = pair.Value;
				var result_pos = iso_orig_p + delta;
				if ( move ) {
					var new_iso_pos = IsoWorld.Instance.ScreenToIso(
						result_pos,
						iso_object.positionZ);
					iso_object.position = IsoUtils.VectorBeautifier(new_iso_pos);
				}
				var pos_delta = result_pos - iso_orig_p;
				return pos_delta.magnitude > AccIn.magnitude ? pos_delta : AccIn;
			});
		}

		void ZMoveSlider() {
			var iso_world = IsoWorld.Instance;
			if ( iso_world ) {
				Handles.color = Handles.zAxisColor;
				var delta = Handles.Slider(_viewCenter, IsoUtils.vec3OneY) - _viewCenter;
				if ( Mathf.Abs(delta.y) > Mathf.Epsilon ) {
					float tmp_y = ZMoveIsoObjects(
						true,
						(_viewCenter.y - _center.y + delta.y) / iso_world.tileHeight,
						_isoZPositions,
						_otherObjects);
					_viewCenter = _center + IsoUtils.Vec3FromY(tmp_y * iso_world.tileHeight);
				}
			}
		}

		void XYMoveSlider(Color color, Vector3 dir) {
			var iso_world = IsoWorld.Instance;
			if ( iso_world ) {
				Handles.color = color;
				var delta = Handles.Slider(_viewCenter, iso_world.IsoToScreen(dir)) - _viewCenter;
				if ( delta.magnitude > Mathf.Epsilon ) {
					_viewCenter = _center + XYMoveIsoObjects(
						true,
						_viewCenter - _center + delta,
						_positions,
						_otherObjects);
				}
			}
		}

		void XYMoveRectangle() {
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
				_viewCenter = _center + XYMoveIsoObjects(
					true,
					_viewCenter - _center + delta,
					_positions,
					_otherObjects);
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
				ZMoveSlider();
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
			DirtyTargetPosition();
			DrawWorldEditorProperties();
		}
	}
}