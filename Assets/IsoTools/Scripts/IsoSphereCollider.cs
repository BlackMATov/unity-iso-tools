using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[RequireComponent(typeof(IsoObject))]
	public class IsoSphereCollider : IsoCollider {

		[SerializeField]
		public float _radius = 0.0f;
		public float Radius {
			get { return _radius; }
			set {
				_radius = value;
				if ( RealSphereCollider ) {
					RealSphereCollider.radius = value;
				}
			}
		}

		[SerializeField]
		public Vector3 _offset = Vector3.zero;
		public Vector3 Offset {
			get { return _offset; }
			set {
				_offset = value;
				if ( RealSphereCollider ) {
					RealSphereCollider.center = value;
				}
			}
		}

		protected override Collider CreateRealCollider(GameObject target) {
			var collider    = target.AddComponent<SphereCollider>();
			collider.radius = Radius;
			collider.center = Offset;
			return collider;
		}

		public SphereCollider RealSphereCollider {
			get { return RealCollider as SphereCollider; }
		}

		#if UNITY_EDITOR
		protected override void Reset() {
			base.Reset();
			var iso_object = GetComponent<IsoObject>();
			Radius = iso_object ? IsoUtils.Vec3MinF(iso_object.Size) * 0.5f : 0.0f;
			Offset = iso_object ? iso_object.Size * 0.5f : Vector3.zero;
			EditorUtility.SetDirty(this);
		}

		protected override void OnValidate() {
			base.OnValidate();
			if ( RealSphereCollider ) {
				RealSphereCollider.radius = Radius;
				RealSphereCollider.center = Offset;
			}
		}

		void OnDrawGizmosSelected() {
			var iso_object = GetComponent<IsoObject>();
			if ( iso_object && iso_object.IsoWorld ) {
				IsoUtils.DrawSphere(
					iso_object.IsoWorld,
					iso_object.Position + Offset,
					Radius,
					Color.green);
			}
		}
		#endif
	}
} // namespace IsoTools