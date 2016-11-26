using UnityEngine;
using IsoTools.Physics.Internal;

namespace IsoTools.Physics {
	public class IsoCollision {

		public IsoCollider       collider         { get; private set; }
		public IsoContactPoint[] contacts         { get; private set; }
		public GameObject        gameObject       { get; private set; }
		public Vector3           impulse          { get; private set; }
		public Vector3           relativeVelocity { get; private set; }
		public IsoRigidbody      rigidbody        { get; private set; }

		public IsoCollision(Collision collision) {
			collider         = IsoPhysicsUtils.IsoConvertCollider(collision.collider);
			contacts         = IsoPhysicsUtils.IsoConvertContactPoints(collision.contacts);
			gameObject       = IsoPhysicsUtils.IsoConvertGameObject(collision.gameObject);
			impulse          = collision.impulse;
			relativeVelocity = collision.relativeVelocity;
			rigidbody        = IsoPhysicsUtils.IsoConvertRigidbody(collision.rigidbody);
		}
	}
}