using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace IsoTools {
	[CustomEditor(typeof(IsoRigidbody)), CanEditMultipleObjects]
	class IsoRigidbodyEditor : Editor {
		void DrawTop(Vector3 pos, Vector3 size) {
			var iso_world = GameObject.FindObjectOfType<IsoWorld>();
			if ( iso_world ) {
				var points = new Vector3[]{
					iso_world.IsoToScreen(pos),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromX(size.x)),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromXY(size.x, size.y)),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromY(size.y)),
					iso_world.IsoToScreen(pos)
				};
				Handles.DrawPolyLine(points);
			}
		}

		void DrawVert(Vector3 pos, Vector3 size) {
			var iso_world = GameObject.FindObjectOfType<IsoWorld>();
			if ( iso_world ) {
				Handles.DrawLine(
					iso_world.IsoToScreen(pos),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromZ(size.z)));
			}
		}

		void DrawCube(Vector3 pos, Vector3 size) {
			Handles.color = Color.green;
			DrawTop (pos - IsoUtils.Vec3FromZ(0.5f), size);
			DrawTop (pos + IsoUtils.Vec3FromZ(size.z - 0.5f), size);
			DrawVert(pos - IsoUtils.Vec3FromZ(0.5f), size);
			DrawVert(pos - IsoUtils.Vec3FromZ(0.5f) + IsoUtils.Vec3FromX(size.x), size);
			DrawVert(pos - IsoUtils.Vec3FromZ(0.5f) + IsoUtils.Vec3FromY(size.y), size);
		}

		void OnSceneGUI() {
			var iso_rigidbody = target as IsoRigidbody;
			var iso_object = iso_rigidbody.GetComponent<IsoObject>();
			if ( iso_object ) {
				DrawCube(iso_object.Position, iso_object.Size);
			}
		}
	}
} // namespace IsoTools