using UnityEngine;

namespace IsoTools {
	public struct IsoRaycastHit {

		public IsoCollider  IsoCollider  { get; private set; }
		public float        Distance     { get; private set; }
		public Vector3      Normal       { get; private set; }
		public Vector3      Point        { get; private set; }
		public IsoRigidbody IsoRigidbody { get; private set; }

		public IsoRaycastHit(RaycastHit hit_info) {
			IsoCollider  = IsoUtils.IsoConvertCollider(hit_info.collider);
			Distance     = hit_info.distance;
			Normal       = hit_info.normal;
			Point        = hit_info.point;
			IsoRigidbody = IsoUtils.IsoConvertRigidbody(hit_info.rigidbody);
		}
	}
} // namespace IsoTools
