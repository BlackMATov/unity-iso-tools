using UnityEngine;

namespace IsoTools.Internal {
	public class IsoFakeTriggerListener : MonoBehaviour {
		GameObject _realGameObject = null;

		public IsoFakeTriggerListener Init(IsoTriggerListener iso_listener) {
			_realGameObject = iso_listener.gameObject;
			return this;
		}

		void OnTriggerEnter(Collider collider) {
			_realGameObject.SendMessage(
				"OnIsoTriggerEnter",
				IsoUtils.IsoConvertCollider(collider),
				SendMessageOptions.DontRequireReceiver);
		}

		void OnTriggerExit(Collider collider) {
			_realGameObject.SendMessage(
				"OnIsoTriggerExit",
				IsoUtils.IsoConvertCollider(collider),
				SendMessageOptions.DontRequireReceiver);
		}
	}
}