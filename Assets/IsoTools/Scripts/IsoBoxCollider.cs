using UnityEngine;
using IsoTools.Internal;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[RequireComponent(typeof(IsoObject))]
	public class IsoBoxCollider : IsoCollider {

		[SerializeField]
		public Vector3 _size = Vector3.zero;
		public Vector3 size {
			get { return _size; }
			set {
				_size = value;
				if ( realBoxCollider ) {
					realBoxCollider.size = value;
				}
			}
		}

		[SerializeField]
		public Vector3 _offset = Vector3.zero;
		public Vector3 offset {
			get { return _offset; }
			set {
				_offset = value;
				if ( realBoxCollider ) {
					realBoxCollider.center = value;
				}
			}
		}

		protected override Collider CreateRealCollider(GameObject target) {
			var collider    = target.AddComponent<BoxCollider>();
			collider.size   = size;
			collider.center = offset;
			return collider;
		}

		public BoxCollider realBoxCollider {
			get { return realCollider as BoxCollider; }
		}

		#if UNITY_EDITOR
		protected override void Reset() {
			base.Reset();
			var iso_object = GetComponent<IsoObject>();
			size   = iso_object ? iso_object.size : Vector3.zero;
			offset = iso_object ? iso_object.size * 0.5f : Vector3.zero;
			EditorUtility.SetDirty(this);
		}

		protected override void OnValidate() {
			base.OnValidate();
			if ( realBoxCollider ) {
				realBoxCollider.size   = size;
				realBoxCollider.center = offset;
			}
		}

		void OnDrawGizmosSelected() {
			var iso_object = GetComponent<IsoObject>();
			if ( iso_object && iso_object.isoWorld ) {
				IsoUtils.DrawCube(
					iso_object.isoWorld,
					iso_object.position + offset,
					size,
					Color.green);
			}
		}
		#endif
	}
} // namespace IsoTools