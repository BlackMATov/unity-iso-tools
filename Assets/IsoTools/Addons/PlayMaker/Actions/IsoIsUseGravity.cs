#if PLAYMAKER
using UnityEngine;
using IsoTools.Physics;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools.Physics")]
	[HutongGames.PlayMaker.Tooltip(
		"Tests if a IsoRigidbody use gravity.")]
	public class IsoIsUseGravity : IsoComponentAction<IsoRigidbody> {
		[RequiredField]
		[CheckForComponent(typeof(IsoRigidbody))]
		public FsmOwnerDefault gameObject;

		public FsmEvent trueEvent;
		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		public bool everyFrame;

		public override void Reset() {
			gameObject  = null;
			trueEvent   = null;
			falseEvent  = null;
			storeResult = null;
			everyFrame  = false;
		}

		public override void OnEnter() {
			DoAction();
			if ( !everyFrame ) {
				Finish();
			}
		}

		public override void OnUpdate() {
			DoAction();
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				var value = isoRigidbody.useGravity;
				storeResult.Value = value;
				Fsm.Event(value ? trueEvent : falseEvent);
			}
		}
	}
}
#endif