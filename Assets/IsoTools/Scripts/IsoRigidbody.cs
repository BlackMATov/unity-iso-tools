using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[RequireComponent(typeof(IsoObject))]
	public class IsoRigidbody : MonoBehaviour {

		public bool                   IsTrigger      = false;
		public bool                   IsKinematic    = false;
		public RigidbodyInterpolation Interpolation  = RigidbodyInterpolation.None;
		public CollisionDetectionMode CollisionMode  = CollisionDetectionMode.Discrete;
		public PhysicMaterial         PhysicMaterial = null;

		IsoObject  _isoObject    = null;
		GameObject _fakeObject   = null;
		Vector3    _lastPosition = Vector3.zero;

		public IsoObject IsoObject {
			get {
				if ( !_isoObject ) {
					_isoObject = GetComponent<IsoObject>();
				}
				if ( !_isoObject ) {
					throw new UnityException("IsoRigidbody. IsoObject not found!");
				}
				return _isoObject;
			}
		}

		public GameObject FakeGameObject {
			get { return _fakeObject; }
		}

		public Rigidbody Rigidbody {
			get { return FakeGameObject.GetComponent<Rigidbody>(); }
		}

		void AddBoxCollider() {
			var collider       = FakeGameObject.AddComponent<BoxCollider>();
			collider.center    = IsoObject.Size / 2.0f;
			collider.size      = IsoObject.Size;
			collider.isTrigger = IsTrigger;
			collider.material  = PhysicMaterial;
		}

		void AddHelperSphere(Vector3 pos, float radius) {
			var collider       = FakeGameObject.AddComponent<SphereCollider>();
			collider.center    = pos;
			collider.radius    = radius;
			collider.isTrigger = IsTrigger;
			collider.material  = PhysicMaterial;
		}

		void AddHelperSpheres() {
			var radius = 0.1f;
			var rdelta = radius * 0.1f;
			var size = IsoObject.Size;
			if ( size.x > radius && size.y > radius && size.z > radius ) {
				AddHelperSphere(new Vector3(radius - rdelta         , radius - rdelta         , radius - rdelta         ), radius);
				AddHelperSphere(new Vector3(size.x - radius + rdelta, radius - rdelta         , radius - rdelta         ), radius);
				AddHelperSphere(new Vector3(radius - rdelta         , size.y - radius + rdelta, radius - rdelta         ), radius);
				AddHelperSphere(new Vector3(size.x - radius + rdelta, size.y - radius + rdelta, radius - rdelta         ), radius);
				AddHelperSphere(new Vector3(radius - rdelta         , radius - rdelta         , size.z - radius + rdelta), radius);
				AddHelperSphere(new Vector3(size.x - radius + rdelta, radius - rdelta         , size.z - radius + rdelta), radius);
				AddHelperSphere(new Vector3(radius - rdelta         , size.y - radius + rdelta, size.z - radius + rdelta), radius);
				AddHelperSphere(new Vector3(size.x - radius + rdelta, size.y - radius + rdelta, size.z - radius + rdelta), radius);
			}
		}

		void Awake() {
			_fakeObject                       = new GameObject();
			FakeGameObject.name               = "_Fake" + gameObject.name;
			FakeGameObject.hideFlags          = HideFlags.HideInHierarchy;

			var rigidbody                     = FakeGameObject.AddComponent<Rigidbody>();
			rigidbody.freezeRotation          = true;
			rigidbody.isKinematic             = IsKinematic;
			rigidbody.interpolation           = Interpolation;
			rigidbody.collisionDetectionMode  = CollisionMode;

			AddBoxCollider();
			AddHelperSpheres();

			_lastPosition                     = IsoObject.Position;
			FakeGameObject.transform.position = IsoObject.Position;
		}

		void FixedUpdate() {
			var fake_transform = FakeGameObject.transform;
			if ( !IsoUtils.Vec3Approximately(_lastPosition, IsoObject.Position) ) {
				fake_transform.position = IsoObject.Position;
			} else {
				IsoObject.Position = fake_transform.position;
			}
			_lastPosition = IsoObject.Position;
		}
	}
} // namespace IsoTools