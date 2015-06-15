using UnityEngine;
using UnityEditor;
using System.Linq;

namespace IsoTools {
	[CustomEditor(typeof(IsoSphereCollider)), CanEditMultipleObjects]
	class IsoSphereColliderEditor : Editor {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if ( GUILayout.Button("Reset") ) {
				var colliders = targets
					.Where(p => p as IsoSphereCollider)
					.Select(p => p as IsoSphereCollider);
				Undo.RecordObjects(colliders.ToArray(), "Reset");
				foreach ( var collider in colliders ) {
					collider.EditorReset();
				}
			}
		}
	}
} // namespace IsoTools