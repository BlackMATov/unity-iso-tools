using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	public class IsoSphereCollider : IsoCollider {
		public float   Radius = 0.0f;
		public Vector3 Offset = Vector3.zero;

		protected override Collider CreateCollider() {
			var collider    = IsoFakeObject.gameObject.AddComponent<SphereCollider>();
			collider.radius = Radius;
			collider.center = Offset;
			return collider;
		}

		#if UNITY_EDITOR
		public override void EditorReset() {
			if ( Application.isEditor ) {
				Radius = IsoUtils.Vec3MinF(IsoObject.Size) * 0.5f;
				Offset = IsoObject.Size * 0.5f;
				EditorUtility.SetDirty(this);
			}
		}

		void OnDrawGizmosSelected() {
			if ( Application.isEditor ) {
				IsoUtils.DrawSphere(
					IsoObject.Position + Offset,
					Radius,
					Color.green);
			}
		}
		#endif
	}
} // namespace IsoTools