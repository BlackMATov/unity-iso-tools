using UnityEngine;

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

		void FixedUpdate() {
			if ( !IsoUtils.Vec3Approximately(_lastPosition, IsoObject.Position) ) {
				transform.position = IsoObject.Position;
			} else {
				IsoObject.Position = transform.position;
			}
			_lastPosition = IsoObject.Position;
		}

		void OnTriggerEnter(Collider collider) {
			IsoObject.gameObject.SendMessage(
				"OnIsoTriggerEnter",
				IsoUtils.IsoConvertCollider(collider),
				SendMessageOptions.DontRequireReceiver);
		}

		void OnTriggerExit(Collider collider) {
			IsoObject.gameObject.SendMessage(
				"OnIsoTriggerExit",
				IsoUtils.IsoConvertCollider(collider),
				SendMessageOptions.DontRequireReceiver);
		}

		void OnCollisionEnter(Collision collision) {
			IsoObject.gameObject.SendMessage(
				"OnIsoCollisionEnter",
				new IsoCollision(collision),
				SendMessageOptions.DontRequireReceiver);
		}
		
		void OnCollisionExit(Collision collision) {
			IsoObject.gameObject.SendMessage(
				"OnIsoCollisionExit",
				new IsoCollision(collision),
				SendMessageOptions.DontRequireReceiver);
		}
	}
} // namespace IsoTools