using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[RequireComponent(typeof(IsoObject))]
	public class IsoSphereCollider : IsoCollider {

		[SerializeField]
		public float _radius = 0.0f;
		public float radius {
			get { return _radius; }
			set {
				_radius = value;
				if ( realSphereCollider ) {
					realSphereCollider.radius = value;
				}
			}
		}

		[SerializeField]
		public Vector3 _offset = Vector3.zero;
		public Vector3 offset {
			get { return _offset; }
			set {
				_offset = value;
				if ( realSphereCollider ) {
					realSphereCollider.center = value;
				}
			}
		}

		protected override Collider CreateRealCollider(GameObject target) {
			var collider    = target.AddComponent<SphereCollider>();
			collider.radius = radius;
			collider.center = offset;
			return collider;
		}

		public SphereCollider realSphereCollider {
			get { return realCollider as SphereCollider; }
		}

		#if UNITY_EDITOR
		protected override void Reset() {
			base.Reset();
			var iso_object = GetComponent<IsoObject>();
			radius = iso_object ? IsoUtils.Vec3MinF(iso_object.size) * 0.5f : 0.0f;
			offset = iso_object ? iso_object.size * 0.5f : Vector3.zero;
			EditorUtility.SetDirty(this);
		}

		protected override void OnValidate() {
			base.OnValidate();
			if ( realSphereCollider ) {
				realSphereCollider.radius = radius;
				realSphereCollider.center = offset;
			}
		}

		void OnDrawGizmosSelected() {
			var iso_object = GetComponent<IsoObject>();
			if ( iso_object && iso_object.isoWorld ) {
				IsoUtils.DrawSphere(
					iso_object.isoWorld,
					iso_object.position + offset,
					radius,
					Color.green);
			}
		}
		#endif
	}
} // namespace IsoTools