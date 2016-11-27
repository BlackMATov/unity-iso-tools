#if PLAYMAKER
using UnityEngine;
using IsoTools.Physics;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Events {
	public enum IsoTriggerType {
		IsoTriggerEnter,
		IsoTriggerExit
	}
	[ActionCategory("IsoTools.Physics")]
	[HutongGames.PlayMaker.Tooltip(
		"Detect physics trigger events.")]
	public class IsoTriggerEvent : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		[HutongGames.PlayMaker.Title("IsoObject (In)")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[HutongGames.PlayMaker.Title("Trigger Type (In)")]
		public IsoTriggerType triggerType;

		[RequiredField]
		[UIHint(UIHint.Tag)]
		[HutongGames.PlayMaker.Title("Collide Tag (In)")]
		public FsmString collideTag;

		[HutongGames.PlayMaker.Title("Send Event (In)")]
		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Iso Collider (Out)")]
		public FsmGameObject storeIsoCollider;

		IsoFSMEvents isoFSMEvents = null;

		public override void Reset() {
			gameObject       = null;
			triggerType      = IsoTriggerType.IsoTriggerEnter;
			collideTag       = "Untagged";
			sendEvent        = null;
			storeIsoCollider = null;
		}

		public override void OnEnter() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( go ) {
				isoFSMEvents = go.AddComponent<IsoFSMEvents>();
				isoFSMEvents.Init(this);
			}
		}

		public override void OnExit() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( go ) {
				if ( isoFSMEvents ) {
					GameObject.Destroy(isoFSMEvents);
					isoFSMEvents = null;
				}
			}
		}

		public override void DoIsoTriggerEnter(IsoCollider collider) {
			if ( triggerType == IsoTriggerType.IsoTriggerEnter ) {
				DoAction(collider);
			}
		}

		public override void DoIsoTriggerExit(IsoCollider collider) {
			if ( triggerType == IsoTriggerType.IsoTriggerExit ) {
				DoAction(collider);
			}
		}

		void DoAction(IsoCollider collider) {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				if ( collider.gameObject.tag == collideTag.Value ) {
					storeIsoCollider.Value = collider.gameObject;
					Fsm.Event(sendEvent);
				}
			}
		}
	}
}
#endif