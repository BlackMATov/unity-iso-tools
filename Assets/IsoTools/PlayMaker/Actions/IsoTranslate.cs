using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip("Translates a IsoObject. Use a Vector3 variable and/or XYZ components. To leave any axis unchanged, set variable to 'None'.")]
	public class IsoTranslate : FsmStateAction {
		[RequiredField]
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
				DoTranlate();
				Finish();
			}
		}

		public override void OnUpdate() {
			if ( !lateUpdate && !fixedUpdate ) {
				DoTranlate();
			}
		}

		public override void OnLateUpdate() {
			if ( lateUpdate ) {
				DoTranlate();
			}
			if ( !everyFrame ) {
				Finish();
			}
		}

		public override void OnFixedUpdate() {
			if ( fixedUpdate ) {
				DoTranlate();
			}
			if ( !everyFrame ) {
				Finish();
			}
		}

		void DoTranlate() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			var iso_object = go ? go.GetComponent<IsoObject>() : null;
			if ( iso_object ) {
				var translate = vector.IsNone
					? new Vector3(x.Value, y.Value, z.Value)
					: vector.Value;

				if ( !x.IsNone ) {
					translate.x = x.Value;
				}
				if ( !y.IsNone ) {
					translate.y = y.Value;
				}
				if ( !z.IsNone ) {
					translate.z = z.Value;
				}

				iso_object.position +=
					translate * (perSecond ? Time.deltaTime : 1.0f);
			}
		}
	}
} // IsoTools.PlayMaker.Actions