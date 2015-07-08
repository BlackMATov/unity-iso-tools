using UnityEngine;

namespace IsoTools {
	public class IsoCollision {

		public IsoCollider       IsoCollider      { get; private set; }
		public IsoContactPoint[] IsoContacts      { get; private set; }
		public GameObject        GameObject       { get; private set; }
		public Vector3           RelativeVelocity { get; private set; }
		public IsoRigidbody      IsoRigidbody     { get; private set; }

		public IsoCollision(Collision collision) {
			IsoCollider      = IsoUtils.IsoConvertCollider(collision.collider);
			IsoContacts      = IsoUtils.IsoConvertContactPoints(collision.contacts);
			GameObject       = IsoUtils.IsoConvertGameObject(collision.gameObject);
			RelativeVelocity = collision.relativeVelocity;
			IsoRigidbody     = IsoUtils.IsoConvertRigidbody(collision.rigidbody);
		}
	}
} // namespace IsoTools