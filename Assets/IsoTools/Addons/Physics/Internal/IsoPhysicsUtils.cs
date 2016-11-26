using UnityEngine;

namespace IsoTools.Physics.Internal {
	public static class IsoPhysicsUtils {
		public static IsoCollider IsoConvertCollider(Collider collider) {
			var fake_collider = collider ? collider.GetComponent<IsoFakeCollider>() : null;
			return fake_collider ? fake_collider.isoCollider : null;
		}

		public static IsoRigidbody IsoConvertRigidbody(Rigidbody rigidbody) {
			var fake_rigidbody = rigidbody ? rigidbody.GetComponent<IsoFakeRigidbody>() : null;
			return fake_rigidbody ? fake_rigidbody.isoRigidbody : null;
		}

		public static GameObject IsoConvertGameObject(GameObject game_object) {
			var fake_object = game_object ? game_object.GetComponent<IsoFakeObject>() : null;
			var iso_object = fake_object ? fake_object.isoObject : null;
			return iso_object ? iso_object.gameObject : null;
		}

		public static IsoContactPoint[] IsoConvertContactPoints(ContactPoint[] points) {
			var iso_points = new IsoContactPoint[points.Length];
			for ( int i = 0, e = points.Length; i < e; ++i ) {
				iso_points[i] = new IsoContactPoint(points[i]);
			}
			return iso_points;
		}

		public static IsoRaycastHit[] IsoConvertRaycastHits(RaycastHit[] hits) {
			var iso_hits = new IsoRaycastHit[hits.Length];
			for ( int i = 0, e = hits.Length; i < e; ++i ) {
				iso_hits[i] = new IsoRaycastHit(hits[i]);
			}
			return iso_hits;
		}
	}
}