#if PLAYMAKER
using UnityEngine;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Sets the Position of a IsoObject. " +
		"To leave any axis unchanged, set variable to 'None'.")]
	public class IsoSetPosition : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		[HutongGames.PlayMaker.Tooltip("The IsoObject to position.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip(
			"Use a stored Vector3 position, " +
			"and/or set individual axis below.")]
		public FsmVector3 vector;

		public FsmFloat x;
		public FsmFloat y;
		public FsmFloat z;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		[HutongGames.PlayMaker.Tooltip("Perform in LateUpdate.")]
		public bool lateUpdate;

		[HutongGames.PlayMaker.Tooltip("Perform in FixedUpdate.")]
		public bool fixedUpdate;

		public override void Reset() {
			gameObject  = null;
			vector      = null;
			x           = new FsmFloat{UseVariable = true};
			y           = new FsmFloat{UseVariable = true};
			z           = new FsmFloat{UseVariable = true};
			everyFrame  = false;
			lateUpdate  = false;
			fixedUpdate = false;
		}

		public override void OnPreprocess() {
			Fsm.HandleFixedUpdate = true;
		}

		public override void OnEnter() {
			if ( !everyFrame && !lateUpdate && !fixedUpdate ) {
				DoAction();
				Finish();
			}
		}

		public override void OnUpdate() {
			if ( !lateUpdate && !fixedUpdate ) {
				DoAction();
			}
		}

		public override void OnLateUpdate() {
			if ( lateUpdate ) {
				DoAction();
			}
			if ( !everyFrame ) {
				Finish();
			}
		}

		public override void OnFixedUpdate() {
			if ( fixedUpdate ) {
				DoAction();
			}
			if ( !everyFrame ) {
				Finish();
			}
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				var value = vector.IsNone ? isoObject.position : vector.Value;
				if ( !x.IsNone ) { value.x = x.Value; }
				if ( !y.IsNone ) { value.y = y.Value; }
				if ( !z.IsNone ) { value.z = z.Value; }
				isoObject.position = value;
			}
		}
	}
}
#endif