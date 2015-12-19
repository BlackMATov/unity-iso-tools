using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	public class IsoFSMEvents : MonoBehaviour {
		IsoComponentAction<IsoObject> _action  = null;
		bool                          _started = false;

		public void Init(IsoComponentAction<IsoObject> action) {
			_action = action;
		}

		void Start() {
			Debug.Assert(_action != null, this);
			_started = true;
		}

		void OnIsoTriggerEnter(IsoCollider collider) {
			if ( _action != null && _started ) {
				_action.DoIsoTriggerEnter(collider);
			}
		}

		void OnIsoTriggerExit(IsoCollider collider) {
			if ( _action != null && _started ) {
				_action.DoIsoTriggerExit(collider);
			}
		}

		void OnIsoCollisionEnter(IsoCollision collision) {
			if ( _action != null && _started ) {
				_action.DoIsoCollisionEnter(collision);
			}
		}

		void OnIsoCollisionExit(IsoCollision collision) {
			if ( _action != null && _started ) {
				_action.DoIsoCollisionExit(collision);
			}
		}
	}
}