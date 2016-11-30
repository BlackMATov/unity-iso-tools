using UnityEngine;

namespace IsoTools.Physics.Internal {
	[AddComponentMenu("")]
	public class IsoFakeCollisionListener : MonoBehaviour {

		GameObject _realGameObject = null;

		public IsoFakeCollisionListener Init(IsoCollisionListener iso_listener) {
			_realGameObject = iso_listener.gameObject;
			return this;
		}

		void OnCollisionEnter(Collision collision) {
			_realGameObject.SendMessage(
				"OnIsoCollisionEnter",
				new IsoCollision(collision),
				SendMessageOptions.DontRequireReceiver);
		}

		void OnCollisionExit(Collision collision) {
			_realGameObject.SendMessage(
				"OnIsoCollisionExit",
				new IsoCollision(collision),
				SendMessageOptions.DontRequireReceiver);
		}
	}
}