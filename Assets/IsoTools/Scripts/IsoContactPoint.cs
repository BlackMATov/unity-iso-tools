using UnityEngine;

namespace IsoTools {
	public class IsoContactPoint {

		public Vector3     Normal           { get; private set; }
		public IsoCollider OtherIsoCollider { get; private set; }
		public Vector3     Point            { get; private set; }
		public IsoCollider ThisIsoCollider  { get; private set; }

		public IsoContactPoint(
			Vector3     normal,
			IsoCollider other_iso_collider,
			Vector3     point,
			IsoCollider this_iso_collider)
		{
			Normal           = normal;
			OtherIsoCollider = other_iso_collider;
			Point            = point;
			ThisIsoCollider  = this_iso_collider;
		}
	}
} // namespace IsoTools