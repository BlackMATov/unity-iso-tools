using UnityEngine;

namespace IsoTools {
	public struct IsoContactPoint {

		public Vector3     normal        { get; private set; }
		public IsoCollider otherCollider { get; private set; }
		public Vector3     point         { get; private set; }
		public IsoCollider thisCollider  { get; private set; }

		public IsoContactPoint(ContactPoint contact_point) {
			normal        = contact_point.normal;
			otherCollider = IsoUtils.IsoConvertCollider(contact_point.otherCollider);
			point         = contact_point.point;
			thisCollider  = IsoUtils.IsoConvertCollider(contact_point.thisCollider);
		}
	}
} // namespace IsoTools