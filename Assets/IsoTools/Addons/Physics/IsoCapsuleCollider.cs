using UnityEngine;
using IsoTools.Internal;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools.Physics {
	[RequireComponent(typeof(IsoObject))]
	public class IsoCapsuleCollider : IsoCollider {

		[SerializeField]
		public float _height = 0.0f;
		public float height {
			get { return _height; }
			set {
				_height = value;
				if ( realCapsuleCollider ) {
					realCapsuleCollider.height = value;
				}
			}
		}

		[SerializeField]
		public float _radius = 0.0f;
		public float radius {
			get { return _radius; }
			set {
				_radius = value;
				if ( realCapsuleCollider ) {
					realCapsuleCollider.radius = value;
				}
			}
		}

		[SerializeField]
		public Vector3 _offset = Vector3.zero;
		public Vector3 offset {
			get { return _offset; }
			set {
				_offset = value;
				if ( realCapsuleCollider ) {
					realCapsuleCollider.center = value;
				}
			}
		}

		protected override Collider CreateRealCollider(GameObject target) {
			var collider       = target.AddComponent<CapsuleCollider>();
			collider.height    = height;
			collider.radius    = radius;
			collider.center    = offset;
			collider.direction = 2; // z-axis
			return collider;
		}

		public CapsuleCollider realCapsuleCollider {
			get { return realCollider as CapsuleCollider; }
		}

	#if UNITY_EDITOR
		protected override void Reset() {
			base.Reset();
			var iso_object      = GetComponent<IsoObject>();
			var iso_object_size = iso_object ? iso_object.size : Vector3.zero;
			height              = iso_object.size.z;
			radius              = IsoUtils.Vec3MinF(iso_object_size) * 0.5f;
			offset              = iso_object_size * 0.5f;
			EditorUtility.SetDirty(this);
		}

		protected override void OnValidate() {
			base.OnValidate();
			if ( realCapsuleCollider ) {
				realCapsuleCollider.height = height;
				realCapsuleCollider.radius = radius;
				realCapsuleCollider.center = offset;
			}
		}

		void OnDrawGizmosSelected() {
			var iso_object = GetComponent<IsoObject>();
			if ( iso_object && iso_object.isoWorld ) {
				if ( radius * 2 < height ) {
					IsoUtils.DrawIsoCube(
						iso_object.isoWorld,
						iso_object.position + offset,
						new Vector3(radius * 2.0f, radius * 2.0f, height - radius),
						Color.green);
					IsoUtils.DrawIsoSphere(
						iso_object.isoWorld,
						iso_object.position + offset - IsoUtils.Vec3FromZ(height * 0.5f - radius),
						radius,
						Color.green);
					IsoUtils.DrawIsoSphere(
						iso_object.isoWorld,
						iso_object.position + offset + IsoUtils.Vec3FromZ(height * 0.5f - radius),
						radius,
						Color.green);
				} else {
					IsoUtils.DrawIsoSphere(
						iso_object.isoWorld,
						iso_object.position + offset,
						radius,
						Color.green);
				}
			}
		}
	#endif
	}
}