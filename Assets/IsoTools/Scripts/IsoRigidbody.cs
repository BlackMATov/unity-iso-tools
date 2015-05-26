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

		IsoObject   _iso_object   = null;
		GameObject  _fake_object  = null;
		Rigidbody   _rigid_body   = null;
		BoxCollider _box_collider = null;

		public Rigidbody Rigidbody {
			get { return _rigid_body; }
		}

		void Awake() {
			_iso_object = GetComponent<IsoObject>();
			if ( !_iso_object ) {
				throw new UnityException("IsoRigidbody. IsoObject not found!");
			}
			_fake_object                       = new GameObject();
			_fake_object.name                  = "_Fake" + gameObject.name;

			_rigid_body                        = _fake_object.AddComponent<Rigidbody>();
			_rigid_body.freezeRotation         = true;
			_rigid_body.isKinematic            = IsKinematic;
			_rigid_body.interpolation          = Interpolation;
			_rigid_body.collisionDetectionMode = CollisionMode;

			_box_collider                      = _fake_object.AddComponent<BoxCollider>();
			_box_collider.center               = IsoUtils.Vec3SwapYZ(_iso_object.Size / 2.0f);
			_box_collider.size                 = IsoUtils.Vec3SwapYZ(_iso_object.Size);
			_box_collider.isTrigger            = IsTrigger;
			_box_collider.material             = PhysicMaterial;

			_fake_object.transform.position    = IsoUtils.Vec3SwapYZ(_iso_object.Position);
		}

		void FixedUpdate() {
			if ( _iso_object ) {
				_iso_object.Position = IsoUtils.Vec3SwapYZ(_fake_object.transform.position);
			}
		}
	}
} // namespace IsoTools