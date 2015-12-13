using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip("Resizes a IsoObject. Use a Vector3 variable and/or XYZ components. To leave any axis unchanged, set variable to 'None'.")]
	public class IsoResize : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		public FsmFloat x;
		public FsmFloat y;
		public FsmFloat z;

		public bool perSecond;
		public bool everyFrame;
		public bool lateUpdate;
		public bool fixedUpdate;

		public override void Reset() {
			gameObject  = null;
			vector      = null;
			x           = new FsmFloat{UseVariable = true};
			y           = new FsmFloat{UseVariable = true};
			z           = new FsmFloat{UseVariable = true};
			perSecond   = true;
			everyFrame  = true;
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
				var value = vector.IsNone
					? Vector3.zero
					: vector.Value;

				if ( !x.IsNone ) {
					value.x = x.Value;
				}
				if ( !y.IsNone ) {
					value.y = y.Value;
				}
				if ( !z.IsNone ) {
					value.z = z.Value;
				}

				isoObject.size +=
					value * (perSecond ? Time.deltaTime : 1.0f);
			}
		}
	}
} // IsoTools.PlayMaker.Actions