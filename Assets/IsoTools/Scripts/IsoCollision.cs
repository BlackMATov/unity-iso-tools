using UnityEngine;

namespace IsoTools {
	public class IsoCollision {

		public IsoCollider       Collider         { get; private set; }
		public IsoContactPoint[] Contacts         { get; private set; }
		public GameObject        GameObject       { get; private set; }
		public Vector3           RelativeVelocity { get; private set; }
		public IsoRigidbody      IsoRigidbody     { get; private set; }

		public IsoCollision(
			IsoCollider       collider,
			IsoContactPoint[] contacts,
			GameObject        game_object,
			Vector3           relative_velocity,
			IsoRigidbody      iso_rigidbody)
		{
			Collider         = collider;
			Contacts         = contacts;
			GameObject       = game_object;
			RelativeVelocity = relative_velocity;
			IsoRigidbody     = iso_rigidbody;
		}
	}
} // namespace IsoTools