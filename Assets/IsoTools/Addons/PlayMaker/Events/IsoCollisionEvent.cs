#if PLAYMAKER
using UnityEngine;
using IsoTools.Physics;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Events {
	public enum IsoCollisionType {
		IsoCollisionEnter,
		IsoCollisionExit
	}
	[ActionCategory("IsoTools.Physics")]
	[HutongGames.PlayMaker.Tooltip(
		"Detect physics collision events.")]
	public class IsoCollisionEvent : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		[HutongGames.PlayMaker.Title("IsoObject (In)")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[HutongGames.PlayMaker.Title("Collision Type (In)")]
		public IsoCollisionType collisionType;

		[RequiredField]
		[UIHint(UIHint.Tag)]
		[HutongGames.PlayMaker.Title("Collide Tag (In)")]
		public FsmString collideTag;

		[HutongGames.PlayMaker.Title("Send Event (In)")]
		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Iso Collider (Out)")]
		public FsmGameObject storeIsoCollider;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Force (Out)")]
		public FsmFloat storeForce;

		IsoFSMEvents isoFSMEvents = null;

		public override void Reset() {
			gameObject       = null;
			collisionType    = IsoCollisionType.IsoCollisionEnter;
			collideTag       = "Untagged";
			sendEvent        = null;
			storeIsoCollider = null;
			storeForce       = null;
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

		public override void DoIsoCollisionEnter(IsoCollision collision) {
			if ( collisionType == IsoCollisionType.IsoCollisionEnter ) {
				DoAction(collision);
			}
		}

		public override void DoIsoCollisionExit(IsoCollision collision) {
			if ( collisionType == IsoCollisionType.IsoCollisionExit ) {
				DoAction(collision);
			}
		}

		void DoAction(IsoCollision collision) {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				if ( collision.collider.gameObject.tag == collideTag.Value ) {
					storeIsoCollider.Value = collision.collider.gameObject;
					storeForce.Value = collision.relativeVelocity.magnitude;
					Fsm.Event(sendEvent);
				}
			}
		}
	}
}
#endif