using UnityEngine;

namespace IsoTools {
	public struct IsoRaycastHit {

		public IsoCollider  collider  { get; private set; }
		public float        distance  { get; private set; }
		public Vector3      normal    { get; private set; }
		public Vector3      point     { get; private set; }
		public IsoRigidbody rigidbody { get; private set; }

		public IsoRaycastHit(RaycastHit hit_info) {
			collider  = IsoUtils.IsoConvertCollider(hit_info.collider);
			distance  = hit_info.distance;
			normal    = hit_info.normal;
			point     = hit_info.point;
			rigidbody = IsoUtils.IsoConvertRigidbody(hit_info.rigidbody);
		}
	}
} // namespace IsoTools
