using UnityEngine;
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

		void FixedUpdate() {
			if ( !IsoUtils.Vec3Approximately(_lastPosition, IsoObject.Position) ) {
				transform.position = IsoObject.Position;
			} else {
				IsoObject.Position = transform.position;
			}
			_lastPosition = IsoObject.Position;
		}

		void OnTriggerEnter(Collider collider) {
			Debug.LogFormat("OnTriggerEnter: {0}-{1}", gameObject.name, collider.gameObject.name);
		}

		void OnTriggerExit(Collider collider) {
			Debug.LogFormat("OnTriggerExit: {0}-{1}", gameObject.name, collider.gameObject.name);
		}

		void OnTriggerStay(Collider collider) {
			Debug.LogFormat("OnTriggerStay: {0}-{1}", gameObject.name, collider.gameObject.name);
		}

		void OnCollisionEnter(Collision collision) {
			Debug.LogFormat("OnCollisionEnter: {0}-{1}", gameObject.name, collision.gameObject.name);
		}

		void OnCollisionExit(Collision collision) {
			Debug.LogFormat("OnCollisionExit: {0}-{1}", gameObject.name, collision.gameObject.name);
		}

		void OnCollisionStay(Collision collision) {
			Debug.LogFormat("OnCollisionStay: {0}-{1}", gameObject.name, collision.gameObject.name);
		}
	}
} // namespace IsoTools