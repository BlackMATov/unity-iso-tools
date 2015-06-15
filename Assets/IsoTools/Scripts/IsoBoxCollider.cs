using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	public class IsoBoxCollider : IsoCollider {
		public Vector3 Size   = Vector3.zero;
		public Vector3 Offset = Vector3.zero;

		protected override Collider CreateCollider() {
			var collider    = IsoFakeObject.gameObject.AddComponent<BoxCollider>();
			collider.center = Offset;
			collider.size   = Size;
			return collider;
		}

		#if UNITY_EDITOR
		public override void EditorReset() {
			if ( Application.isEditor ) {
				Size   = IsoObject.Size;
				Offset = IsoObject.Size * 0.5f;
				EditorUtility.SetDirty(this);
			}
		}

		void OnDrawGizmosSelected() {
			if ( Application.isEditor ) {
				IsoUtils.DrawCube(
					IsoObject.Position + Offset,
					Size,
					Color.green);
			}
		}
		#endif
	}
} // namespace IsoTools