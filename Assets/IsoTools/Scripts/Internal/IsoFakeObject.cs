using UnityEngine;

namespace IsoTools.Internal {
	public class IsoFakeObject : MonoBehaviour {

		IsoObject _isoObject    = null;
		Vector3   _lastPosition = Vector3.zero;

		public void Init(IsoObject iso_object) {
			_isoObject         = iso_object;
			_lastPosition      = iso_object.position;
			transform.position = iso_object.position;
		}

		public IsoObject isoObject {
			get { return _isoObject; }
		}

		void CheckLayers() {
			var iso_object_layer = isoObject.gameObject.layer;
			if ( gameObject.layer != iso_object_layer ) {
				gameObject.layer = iso_object_layer;
				for ( int i = 0, e = transform.childCount; i < e; ++i ) {
					var child = transform.GetChild(i);
					child.gameObject.layer = iso_object_layer;
				}
			}
		}

		void FixedUpdate() {
			CheckLayers();
			if ( !IsoUtils.Vec3Approximately(_lastPosition, isoObject.position) ) {
				_lastPosition = transform.position = isoObject.position;
			} else if ( !IsoUtils.Vec3Approximately(_lastPosition, transform.position) ) {
				_lastPosition = isoObject.position = transform.position;
			}
		}

		void OnTriggerEnter(Collider collider) {
			isoObject.gameObject.SendMessage(
				"OnIsoTriggerEnter",
				IsoUtils.IsoConvertCollider(collider),
				SendMessageOptions.DontRequireReceiver);
		}

		void OnTriggerExit(Collider collider) {
			isoObject.gameObject.SendMessage(
				"OnIsoTriggerExit",
				IsoUtils.IsoConvertCollider(collider),
				SendMessageOptions.DontRequireReceiver);
		}

		void OnCollisionEnter(Collision collision) {
			isoObject.gameObject.SendMessage(
				"OnIsoCollisionEnter",
				new IsoCollision(collision),
				SendMessageOptions.DontRequireReceiver);
		}
		
		void OnCollisionExit(Collision collision) {
			isoObject.gameObject.SendMessage(
				"OnIsoCollisionExit",
				new IsoCollision(collision),
				SendMessageOptions.DontRequireReceiver);
		}
	}
}
