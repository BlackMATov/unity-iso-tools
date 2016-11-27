#if PLAYMAKER
using UnityEngine;
using IsoTools.Physics;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools.Physics")]
	[HutongGames.PlayMaker.Tooltip(
		"Sets the Velocity of a IsoRigidbody. " +
		"To leave any axis unchanged, set variable to 'None'.")]
	public class IsoSetVelocity : IsoComponentAction<IsoRigidbody> {
		[RequiredField]
		[CheckForComponent(typeof(IsoRigidbody))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		public FsmFloat x;
		public FsmFloat y;
		public FsmFloat z;

		public bool everyFrame;

		public override void Reset() {
			gameObject  = null;
			vector      = null;
			x           = new FsmFloat{UseVariable = true};
			y           = new FsmFloat{UseVariable = true};
			z           = new FsmFloat{UseVariable = true};
			everyFrame  = false;
		}

		public override void OnPreprocess() {
			Fsm.HandleFixedUpdate = true;
		}

		public override void OnEnter() {
			DoAction();
			if ( !everyFrame ) {
				Finish();
			}
		}

		public override void OnFixedUpdate() {
			DoAction();
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				var value = vector.IsNone ? isoRigidbody.velocity : vector.Value;
				if ( !x.IsNone ) { value.x = x.Value; }
				if ( !y.IsNone ) { value.y = y.Value; }
				if ( !z.IsNone ) { value.z = z.Value; }
				isoRigidbody.velocity = value;
			}
		}
	}
}
#endif