using UnityEngine;

namespace IsoTools.Internal {
	public class IsoFakeCollisionListener : MonoBehaviour {
		IsoCollisionListener _isoListener = null;

		public IsoFakeCollisionListener Init(IsoCollisionListener iso_listener) {
			_isoListener = iso_listener;
			return this;
		}

		IsoCollisionListener isoListener {
			get { return _isoListener; }
		}

		void OnCollisionEnter(Collision collision) {
			isoListener.gameObject.SendMessage(
				"OnIsoCollisionEnter",
				new IsoCollision(collision),
				SendMessageOptions.DontRequireReceiver);
		}

		void OnCollisionExit(Collision collision) {
			isoListener.gameObject.SendMessage(
				"OnIsoCollisionExit",
				new IsoCollision(collision),
				SendMessageOptions.DontRequireReceiver);
		}
	}
}