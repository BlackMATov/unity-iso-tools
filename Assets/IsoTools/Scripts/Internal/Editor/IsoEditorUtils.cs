using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace IsoTools.Internal {
	static class IsoEditorUtils {

		// ---------------------------------------------------------------------
		//
		// Inspector
		//
		// ---------------------------------------------------------------------

		public static void DoWithMixedValue(bool mixed, System.Action act) {
			var last_show_mixed_value = EditorGUI.showMixedValue;
			EditorGUI.showMixedValue = mixed;
			try {
				act();
			} finally {
				EditorGUI.showMixedValue = last_show_mixed_value;
			}
		}

		public static void DoWithEnabledGUI(bool enabled, System.Action act) {
			EditorGUI.BeginDisabledGroup(!enabled);
			try {
				act();
			} finally {
				EditorGUI.EndDisabledGroup();
			}
		}

		public static bool DoFoldoutHeaderGroup(bool foldout, string header, System.Action act) {
			foldout = EditorGUILayout.Foldout(foldout, header);
			if ( foldout ) {
				act();
			}
			return foldout;
		}

		public static void DrawWorldProperties(IsoWorld[] iso_worlds) {
			if ( iso_worlds.Length > 0 ) {
				var so = new SerializedObject(iso_worlds);
				var so_prop = so.GetIterator();
				if ( so_prop.NextVisible(true) ) {
					while ( so_prop.NextVisible(true) ) {
						EditorGUILayout.PropertyField(so_prop);
					}
				}
				if ( GUI.changed ) {
					so.ApplyModifiedProperties();
				}
			}
		}

		public static void DrawSelfWorldProperty(IsoWorld[] iso_worlds) {
			if ( iso_worlds.Length > 0 ) {
				var mixed_world = iso_worlds.GroupBy(p => p).Count() > 1;
				EditorGUILayout.Space();
				IsoEditorUtils.DoWithEnabledGUI(false, () => {
					IsoEditorUtils.DoWithMixedValue(mixed_world, () => {
						EditorGUILayout.ObjectField(
							"Current IsoWorld",
							iso_worlds.First(),
							typeof(IsoWorld),
							true);
					});
				});
			}
		}

		public static bool IsPrefabInstance(GameObject go) {
		#if UNITY_2018_3_OR_NEWER
			return PrefabUtility.IsPartOfAnyPrefab(go);
		#else
			return PrefabUtility.GetPrefabType(go) != PrefabType.None;
		#endif
		}

		// ---------------------------------------------------------------------
		//
		// Editor
		//
		// ---------------------------------------------------------------------

		public static float ZMoveIsoObjects(
			bool                                  move,
			IsoWorld                              iso_world,
			Dictionary<IsoWorld, List<IsoObject>> all_iso_objects,
			Dictionary<IsoWorld, List<IsoObject>> all_other_objects,
			float                                 delta)
		{
			List<IsoObject> iso_objects;
			if ( all_iso_objects.TryGetValue(iso_world, out iso_objects) ) {
				if ( move ) {
					Undo.RecordObjects(
						iso_objects.ToArray(),
						iso_objects.Count > 1 ? "Move IsoObjects" : "Move IsoObject");
				}
				var snapping_z = false;
				if ( IsSnapByObjectsEnabled(iso_world) ) {
					List<IsoObject> other_objects;
					if ( all_other_objects.TryGetValue(iso_world, out other_objects) && other_objects.Count > 0 ) {
						foreach ( var iso_object in iso_objects ) {
							var iso_orig_z = iso_object.positionZ;
							var result_p_z = iso_orig_z + delta;
							foreach ( var other in other_objects ) {
								var x_snap_overlaps = IsoEditorUtils.IsSnapOverlaps(
									iso_world.snappingDistance, iso_object.positionX, iso_object.sizeX, other.positionX, other.sizeX);
								var y_snap_overlaps = IsoEditorUtils.IsSnapOverlaps(
									iso_world.snappingDistance, iso_object.positionY, iso_object.sizeY, other.positionY, other.sizeY);
								if ( x_snap_overlaps && y_snap_overlaps ){
									var new_snapping_z = !snapping_z && IsoEditorUtils.IsSnapOverlaps(
											iso_world.snappingDistance, result_p_z, iso_object.sizeZ, other.positionZ, other.sizeZ)
										? IsoEditorUtils.SnapProcess(
											iso_world.snappingDistance,
											ref result_p_z, iso_object.sizeZ, other.positionZ, other.sizeZ)
										: false;
									if ( new_snapping_z && IsSnapByObjectAllowed(iso_object, other) ) {
										delta      = result_p_z - iso_orig_z;
										snapping_z = true;
										break;
									}
								}
							}
							if ( snapping_z ) {
								break;
							}
						}
					}
				}
				if ( IsSnapByCellsEnabled(iso_world) && !snapping_z ) {
					foreach ( var iso_object in iso_objects ) {
						var iso_orig_z = iso_object.positionZ;
						var result_p_z = iso_orig_z + delta;
						var new_snapping_z = IsoEditorUtils.SnapProcess(
							iso_world.snappingDistance,
							ref result_p_z, iso_object.sizeZ, iso_object.tilePositionZ, 1.0f);
						if ( new_snapping_z ) {
							delta      = result_p_z - iso_orig_z;
							snapping_z = true;
							break;
						}
					}
				}
				return iso_objects.Aggregate(0.0f, (AccIn, iso_object) => {
					var iso_orig_z = iso_object.positionZ;
					var result_p_z = iso_orig_z + delta;
					if ( move ) {
						iso_object.positionZ = IsoUtils.FloatBeautifier(result_p_z);
						if ( IsoEditorUtils.IsPrefabInstance(iso_object.gameObject) ) {
							PrefabUtility.RecordPrefabInstancePropertyModifications(iso_object);
						}
					}
					var z_delta = result_p_z - iso_orig_z;
					return Mathf.Abs(z_delta) > Mathf.Abs(AccIn) ? z_delta : AccIn;
				});
			}
			return delta;
		}

		public static Vector3 XYMoveIsoObjects(
			bool                                  move,
			IsoWorld                              iso_world,
			Dictionary<IsoWorld, List<IsoObject>> all_iso_objects,
			Dictionary<IsoWorld, List<IsoObject>> all_other_objects,
			Vector3                               iso_delta)
		{
			List<IsoObject> iso_objects;
			if ( all_iso_objects.TryGetValue(iso_world, out iso_objects) ) {
				if ( move ) {
					Undo.RecordObjects(
						iso_objects.ToArray(),
						iso_objects.Count > 1 ? "Move IsoObjects" : "Move IsoObject");
				}
				var snapping_x = false;
				var snapping_y = false;
				if ( IsSnapByObjectsEnabled(iso_world) ) {
					List<IsoObject> other_objects;
					if ( all_other_objects.TryGetValue(iso_world, out other_objects) && other_objects.Count > 0 ) {
						foreach ( var iso_object in iso_objects ) {
							var iso_orig_p     = iso_object.position;
							var result_pos_iso = iso_orig_p + iso_delta;
							foreach ( var other in other_objects ) {
								var z_snap_overlaps = IsoEditorUtils.IsSnapOverlaps(
									iso_world.snappingDistance, iso_object.positionZ, iso_object.sizeZ, other.positionZ, other.sizeZ);
								if ( z_snap_overlaps ) {
									var new_snapping_x = !snapping_x && IsoEditorUtils.IsSnapOverlaps(
											iso_world.snappingDistance, result_pos_iso.y, iso_object.sizeY, other.positionY, other.sizeY)
										? IsoEditorUtils.SnapProcess(
											iso_world.snappingDistance,
											ref result_pos_iso.x, iso_object.sizeX, other.positionX, other.sizeX)
										: false;
									var new_snapping_y = !snapping_y && IsoEditorUtils.IsSnapOverlaps(
											iso_world.snappingDistance, result_pos_iso.x, iso_object.sizeX, other.positionX, other.sizeX)
										? IsoEditorUtils.SnapProcess(
											iso_world.snappingDistance,
											ref result_pos_iso.y, iso_object.sizeY, other.positionY, other.sizeY)
										: false;
									if ( (new_snapping_x || new_snapping_y) && IsSnapByObjectAllowed(iso_object, other) ) {
										if ( new_snapping_x ) {
											snapping_x  = true;
											iso_delta.x = result_pos_iso.x - iso_orig_p.x;
											iso_delta.y = result_pos_iso.y - iso_orig_p.y;
										}
										if ( new_snapping_y ) {
											snapping_y  = true;
											iso_delta.x = result_pos_iso.x - iso_orig_p.x;
											iso_delta.y = result_pos_iso.y - iso_orig_p.y;
										}
										if ( snapping_x && snapping_y ) {
											break;
										}
									}
								}
							}
							if ( snapping_x && snapping_y ) {
								break;
							}
						}
					}
				}
				if ( IsSnapByCellsEnabled(iso_world) && !snapping_x && !snapping_y ) {
					foreach ( var iso_object in iso_objects ) {
						var iso_orig_p     = iso_object.position;
						var result_pos_iso = iso_orig_p + iso_delta;
						var new_snapping_x = IsoEditorUtils.SnapProcess(
							iso_world.snappingDistance,
							ref result_pos_iso.x, iso_object.sizeX, iso_object.tilePositionX, 1.0f);
						var new_snapping_y = IsoEditorUtils.SnapProcess(
							iso_world.snappingDistance,
							ref result_pos_iso.y, iso_object.sizeY, iso_object.tilePositionY, 1.0f);
						if ( new_snapping_x || new_snapping_y ) {
							if ( new_snapping_x ) {
								iso_delta.x    = result_pos_iso.x - iso_orig_p.x;
								iso_delta.y    = result_pos_iso.y - iso_orig_p.y;
								snapping_x = true;
							}
							if ( new_snapping_y ) {
								iso_delta.x    = result_pos_iso.x - iso_orig_p.x;
								iso_delta.y    = result_pos_iso.y - iso_orig_p.y;
								snapping_y = true;
							}
							if ( snapping_x && snapping_y ) {
								break;
							}
						}
					}
				}
				return iso_objects.Aggregate(Vector3.zero, (AccIn, iso_object) => {
					var iso_orig_p     = iso_object.position;
					var result_pos_iso = iso_orig_p + iso_delta;
					if ( move ) {
						iso_object.position = IsoUtils.VectorBeautifier(result_pos_iso);
						if ( IsoEditorUtils.IsPrefabInstance(iso_object.gameObject) ) {
							PrefabUtility.RecordPrefabInstancePropertyModifications(iso_object);
						}
					}
					var pos_delta = result_pos_iso - iso_orig_p;
					return pos_delta.magnitude > AccIn.magnitude ? pos_delta : AccIn;
				});
			}
			return iso_delta;
		}

		// ---------------------------------------------------------------------
		//
		// Gizmos
		//
		// ---------------------------------------------------------------------

		public static Vector3 GizmoRectangle(Vector3 center) {
			GizmoDotCap(new Color(1.0f, 1.0f, 1.0f, 0.3f) * Handles.zAxisColor, center, 0.15f);
			GizmoArrowCap(Handles.zAxisColor, center, 1.0f);
			return GizmoFreeMoveHandle(Handles.zAxisColor, center, 0.15f);
		}

		public static Vector3 GizmoSlider(Color color, Vector3 pos, Vector3 dir) {
			Handles.color = color;
			return Handles.Slider(pos, dir);
		}

		public static void GizmoDotCap(Color color, Vector3 center, float scale) {
			Handles.color = color;
		#if UNITY_5_6_OR_NEWER
			Handles.DotHandleCap(
				0,
				center,
				Quaternion.identity,
				HandleUtility.GetHandleSize(center) * scale,
				EventType.Repaint);
		#else
			Handles.DotCap(
				0,
				center,
				Quaternion.identity,
				HandleUtility.GetHandleSize(center) * scale);
		#endif
		}

		public static void GizmoArrowCap(Color color, Vector3 center, float scale) {
			Handles.color = color;
		#if UNITY_5_6_OR_NEWER
			Handles.ArrowHandleCap(
				0,
				center,
				Quaternion.identity,
				HandleUtility.GetHandleSize(center) * scale,
				EventType.Repaint);
		#else
			Handles.ArrowCap(
				0,
				center,
				Quaternion.identity,
				HandleUtility.GetHandleSize(center) * scale);
		#endif
		}

		public static Vector3 GizmoFreeMoveHandle(Color color, Vector3 center, float scale) {
		#if UNITY_5_6_OR_NEWER
			return Handles.FreeMoveHandle(
				center,
				Quaternion.identity,
				HandleUtility.GetHandleSize(center) * scale,
				Vector3.zero,
				Handles.RectangleHandleCap);
		#else
			return Handles.FreeMoveHandle(
				center,
				Quaternion.identity,
				HandleUtility.GetHandleSize(center) * scale,
				Vector3.zero,
				Handles.RectangleCap);
		#endif
		}

		// ---------------------------------------------------------------------
		//
		// Snapping
		//
		// ---------------------------------------------------------------------

		public static bool SnapProcess(
			float snap_dist, ref float min_a, float size_a, float min_b, float size_b)
		{
			var max_a  = min_a + size_a;
			var max_b  = min_b + size_b;
			var result = false;
			if ( IsSnapOverlaps(snap_dist, min_a, size_a, min_b, size_b) ) {
				// |min_a max_a|min_b max_b|
				if ( Mathf.Abs(max_a - min_b) < snap_dist ) {
					min_a  = min_b - size_a;
					result = true;
				}
				// |min_b max_b|min_a max_a|
				if ( Mathf.Abs(max_b - min_a) < snap_dist ) {
					min_a  = max_b;
					result = true;
				}
				// |min_a_____max_a|
				// |min_b__max_b|
				if ( Mathf.Abs(min_a - min_b) < snap_dist ) {
					min_a  = min_b;
					result = true;
				}
				// |min_a_____max_a|
				//    |min_b__max_b|
				if ( Mathf.Abs(max_a - max_b) < snap_dist ) {
					min_a  = max_b - size_a;
					result = true;
				}
			}
			return result;
		}

		public static bool IsSnapOverlaps(
			float snap_dist, float min_a, float size_a, float min_b, float size_b)
		{
			return
				min_a + size_a + snap_dist >= min_b &&
				min_a - snap_dist <= min_b + size_b;
		}

		public static bool IsSnapByCellsEnabled(IsoWorld iso_world) {
			return
				iso_world &&
				(iso_world.isSnapByCells != Event.current.control);
		}

		public static bool IsSnapByObjectsEnabled(IsoWorld iso_world) {
			return
				iso_world &&
				(iso_world.isSnapByObjects != Event.current.control);
		}

		public static bool IsSnapByObjectAllowed(IsoObject iso_object, IsoObject other_object) {
			var iso_object_trans = iso_object.transform;
			var other_object_trans = other_object.transform;
			while ( other_object_trans ) {
				if ( other_object_trans.parent == iso_object_trans ) {
					return false;
				}
				other_object_trans = other_object_trans.parent;
			}
			return true;
		}
	}
}