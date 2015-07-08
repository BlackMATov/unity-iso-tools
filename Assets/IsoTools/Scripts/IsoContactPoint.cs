using UnityEngine;

namespace IsoTools {
	public struct IsoContactPoint {

		public Vector3     Normal           { get; private set; }
		public IsoCollider OtherIsoCollider { get; private set; }
		public Vector3     Point            { get; private set; }
		public IsoCollider ThisIsoCollider  { get; private set; }

		public IsoContactPoint(ContactPoint contact_point) {
			Normal           = contact_point.normal;
			OtherIsoCollider = IsoUtils.IsoConvertCollider(contact_point.otherCollider);
			Point            = contact_point.point;
			ThisIsoCollider  = IsoUtils.IsoConvertCollider(contact_point.thisCollider);
		}
	}
} // namespace IsoTools