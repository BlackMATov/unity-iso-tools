using UnityEngine;
using IsoTools.Physics.Internal;

namespace IsoTools.Physics {
	public struct IsoContactPoint {

		public Vector3     normal        { get; private set; }
		public IsoCollider otherCollider { get; private set; }
		public Vector3     point         { get; private set; }
		public float       separation    { get; private set; }
		public IsoCollider thisCollider  { get; private set; }

		public IsoContactPoint(ContactPoint contact_point) : this() {
			normal        = contact_point.normal;
			otherCollider = IsoPhysicsUtils.IsoConvertCollider(contact_point.otherCollider);
			point         = contact_point.point;
			separation    = contact_point.separation;
			thisCollider  = IsoPhysicsUtils.IsoConvertCollider(contact_point.thisCollider);
		}
	}
}