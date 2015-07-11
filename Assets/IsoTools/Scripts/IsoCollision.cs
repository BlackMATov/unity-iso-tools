using UnityEngine;

namespace IsoTools {
	public class IsoCollision {

		public IsoCollider       collider         { get; private set; }
		public IsoContactPoint[] contacts         { get; private set; }
		public GameObject        gameObject       { get; private set; }
		public Vector3           relativeVelocity { get; private set; }
		public IsoRigidbody      rigidbody        { get; private set; }

		public IsoCollision(Collision collision) {
			collider         = IsoUtils.IsoConvertCollider(collision.collider);
			contacts         = IsoUtils.IsoConvertContactPoints(collision.contacts);
			gameObject       = IsoUtils.IsoConvertGameObject(collision.gameObject);
			relativeVelocity = collision.relativeVelocity;
			rigidbody        = IsoUtils.IsoConvertRigidbody(collision.rigidbody);
		}
	}
} // namespace IsoTools