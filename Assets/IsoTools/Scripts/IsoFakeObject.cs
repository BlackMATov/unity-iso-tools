using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace IsoTools {
	public class IsoFakeObject : MonoBehaviour {

		IsoObject _isoObject    = null;
		Vector3   _lastPosition = Vector3.zero;

		public void Init(IsoObject iso_object) {
			_isoObject         = iso_object;
			_lastPosition      = iso_object.Position;
			transform.position = iso_object.Position;
		}

		public IsoObject IsoObject {
			get { return _isoObject; }
		}

		IsoCollider ConvertCollider(Collider collider) {
			var fake_collider = collider.GetComponent<IsoFakeCollider>();
			return fake_collider ? fake_collider.IsoCollider : null;
		}
		
		IsoRigidbody ConvertRigidbody(Rigidbody rigidbody) {
			var fake_object = rigidbody.GetComponent<IsoFakeObject>();
			var iso_object = fake_object ? fake_object.IsoObject : null;
			return iso_object ? iso_object.GetComponent<IsoRigidbody>() : null;
		}
		
		GameObject ConvertGameObject(GameObject game_object) {
			var fake_object = game_object.GetComponent<IsoFakeObject>();
			var iso_object = fake_object ? fake_object.IsoObject : null;
			return iso_object ? iso_object.gameObject : null;
		}
		
		IsoContactPoint ConvertContactPoint(ContactPoint contact_point) {
			return new IsoContactPoint(
				contact_point.normal,
				ConvertCollider(contact_point.otherCollider),
				contact_point.point,
				ConvertCollider(contact_point.thisCollider));
		}
		
		IsoContactPoint[] ConvertContactPoints(ContactPoint[] points) {
			var iso_points = new IsoContactPoint[points.Length];
			for ( var i = 0; i < points.Length; ++i ) {
				iso_points[i] = ConvertContactPoint(points[i]);
			}
			return iso_points;
		}
		
		IsoCollision ConvertCollision(Collision collision) {
			return new IsoCollision(
				ConvertCollider(collision.collider),
				ConvertContactPoints(collision.contacts),
				ConvertGameObject(collision.gameObject),
				collision.relativeVelocity,
				ConvertRigidbody(collision.rigidbody));
		}

		void FixedUpdate() {
			if ( !IsoUtils.Vec3Approximately(_lastPosition, IsoObject.Position) ) {
				transform.position = IsoObject.Position;
			} else {
				IsoObject.Position = transform.position;
			}
			_lastPosition = IsoObject.Position;
		}

		void OnTriggerEnter(Collider collider) {
			var iso_collider = ConvertCollider(collider);
			IsoObject.gameObject.SendMessage(
				"OnIsoTriggerEnter", iso_collider, SendMessageOptions.DontRequireReceiver);
		}

		void OnTriggerExit(Collider collider) {
			var iso_collider = ConvertCollider(collider);
			IsoObject.gameObject.SendMessage(
				"OnIsoTriggerExit", iso_collider, SendMessageOptions.DontRequireReceiver);
		}

		void OnCollisionEnter(Collision collision) {
			var iso_collision = ConvertCollision(collision);
			IsoObject.gameObject.SendMessage(
				"OnIsoCollisionEnter", iso_collision, SendMessageOptions.DontRequireReceiver);
		}
		
		void OnCollisionExit(Collision collision) {
			var iso_collision = ConvertCollision(collision);
			IsoObject.gameObject.SendMessage(
				"OnIsoCollisionExit", iso_collision, SendMessageOptions.DontRequireReceiver);
		}
	}
} // namespace IsoTools