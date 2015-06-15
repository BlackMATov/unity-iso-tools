using UnityEngine;

namespace IsoTools {
	[RequireComponent(typeof(IsoPhysicHelper))]
	public class IsoRigidbody : MonoBehaviour {

		public bool                   IsKinematic   = false;
		public RigidbodyInterpolation Interpolation = RigidbodyInterpolation.None;
		public CollisionDetectionMode CollisionMode = CollisionDetectionMode.Discrete;

		public IsoObject IsoObject {
			get { return GetComponent<IsoPhysicHelper>().IsoObject; }
		}
		
		public IsoFakeObject IsoFakeObject {
			get { return GetComponent<IsoPhysicHelper>().IsoFakeObject; }
		}

		public Rigidbody RealRigidbody {
			get { return IsoFakeObject.GetComponent<Rigidbody>(); }
		}
		
		void Awake() {
			var rigidbody                    = IsoFakeObject.gameObject.AddComponent<Rigidbody>();
			rigidbody.freezeRotation         = true;
			rigidbody.isKinematic            = IsKinematic;
			rigidbody.interpolation          = Interpolation;
			rigidbody.collisionDetectionMode = CollisionMode;
		}
	}
} // namespace IsoTools