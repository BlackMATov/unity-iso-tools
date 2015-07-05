using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[RequireComponent(typeof(IsoObject))]
	public class IsoBoxCollider : IsoCollider {

		[SerializeField]
		public Vector3 _size = Vector3.zero;
		public Vector3 Size {
			get { return _size; }
			set {
				_size = value;
				if ( RealBoxCollider ) {
					RealBoxCollider.size = value;
				}
			}
		}

		[SerializeField]
		public Vector3 _offset = Vector3.zero;
		public Vector3 Offset {
			get { return _offset; }
			set {
				_offset = value;
				if ( RealBoxCollider ) {
					RealBoxCollider.center = value;
				}
			}
		}

		protected override Collider CreateCollider(GameObject target) {
			var collider    = target.AddComponent<BoxCollider>();
			collider.size   = Size;
			collider.center = Offset;
			return collider;
		}

		public BoxCollider RealBoxCollider {
			get { return RealCollider as BoxCollider; }
		}

		#if UNITY_EDITOR
		protected override void Reset() {
			base.Reset();
			var iso_object = GetComponent<IsoObject>();
			Size   = iso_object ? iso_object.Size : Vector3.zero;
			Offset = iso_object ? iso_object.Size * 0.5f : Vector3.zero;
			EditorUtility.SetDirty(this);
		}

		protected override void OnValidate() {
			base.OnValidate();
			if ( RealBoxCollider ) {
				RealBoxCollider.size   = Size;
				RealBoxCollider.center = Offset;
			}
		}

		void OnDrawGizmosSelected() {
			var iso_object = GetComponent<IsoObject>();
			if ( iso_object && iso_object.IsoWorld ) {
				IsoUtils.DrawCube(
					iso_object.IsoWorld,
					iso_object.Position + Offset,
					Size,
					Color.green);
			}
		}
		#endif
	}
} // namespace IsoTools