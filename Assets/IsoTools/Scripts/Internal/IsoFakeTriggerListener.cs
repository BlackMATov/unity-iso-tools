using UnityEngine;

namespace IsoTools.Internal {
	public class IsoFakeTriggerListener : MonoBehaviour {
		IsoTriggerListener _isoListener = null;

		public IsoFakeTriggerListener Init(IsoTriggerListener iso_listener) {
			_isoListener = iso_listener;
			return this;
		}

		IsoTriggerListener isoListener {
			get { return _isoListener; }
		}

		void OnTriggerEnter(Collider collider) {
			isoListener.gameObject.SendMessage(
				"OnIsoTriggerEnter",
				IsoUtils.IsoConvertCollider(collider),
				SendMessageOptions.DontRequireReceiver);
		}

		void OnTriggerExit(Collider collider) {
			isoListener.gameObject.SendMessage(
				"OnIsoTriggerExit",
				IsoUtils.IsoConvertCollider(collider),
				SendMessageOptions.DontRequireReceiver);
		}
	}
}