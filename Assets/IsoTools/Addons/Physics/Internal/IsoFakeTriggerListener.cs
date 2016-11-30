using UnityEngine;

namespace IsoTools.Physics.Internal {
	[AddComponentMenu("")]
	public class IsoFakeTriggerListener : MonoBehaviour {

		GameObject _realGameObject = null;

		public IsoFakeTriggerListener Init(IsoTriggerListener iso_listener) {
			_realGameObject = iso_listener.gameObject;
			return this;
		}

		void OnTriggerEnter(Collider collider) {
			_realGameObject.SendMessage(
				"OnIsoTriggerEnter",
				IsoPhysicsUtils.IsoConvertCollider(collider),
				SendMessageOptions.DontRequireReceiver);
		}

		void OnTriggerExit(Collider collider) {
			_realGameObject.SendMessage(
				"OnIsoTriggerExit",
				IsoPhysicsUtils.IsoConvertCollider(collider),
				SendMessageOptions.DontRequireReceiver);
		}
	}
}