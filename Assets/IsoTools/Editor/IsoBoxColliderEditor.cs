using UnityEngine;
using UnityEditor;
using System.Linq;

namespace IsoTools {
	[CustomEditor(typeof(IsoBoxCollider)), CanEditMultipleObjects]
	class IsoBoxColliderEditor : Editor {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if ( GUILayout.Button("Reset") ) {
				var colliders = targets
					.Where(p => p as IsoBoxCollider)
					.Select(p => p as IsoBoxCollider);
				Undo.RecordObjects(colliders.ToArray(), "Reset");
				foreach ( var collider in colliders ) {
					collider.EditorReset();
				}
			}
		}
	}
} // namespace IsoTools