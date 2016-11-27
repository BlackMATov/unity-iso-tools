#if PLAYMAKER
using UnityEngine;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Translates a IsoObject. " +
		"Use a Vector3 variable and/or XYZ components. " +
		"To leave any axis unchanged, set variable to 'None'.")]
	public class IsoTranslate : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		[HutongGames.PlayMaker.Title("IsoObject (In)")]
		[HutongGames.PlayMaker.Tooltip("The IsoObject to translate.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Vector (In)")]
		[HutongGames.PlayMaker.Tooltip(
			"Use a stored translation Vector3, " +
			"and/or set individual axis below.")]
		public FsmVector3 vector;

		[HutongGames.PlayMaker.Title("X (In)")]
		public FsmFloat x;

		[HutongGames.PlayMaker.Title("Y (In)")]
		public FsmFloat y;

		[HutongGames.PlayMaker.Title("Z (In)")]
		public FsmFloat z;

		[HutongGames.PlayMaker.Tooltip("Translate over one second")]
		public bool perSecond;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		[HutongGames.PlayMaker.Tooltip("Perform the translate in LateUpdate.")]
		public bool lateUpdate;

		[HutongGames.PlayMaker.Tooltip("Perform the translate in FixedUpdate.")]
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
				var value = vector.IsNone ? Vector3.zero : vector.Value;
				if ( !x.IsNone ) { value.x = x.Value; }
				if ( !y.IsNone ) { value.y = y.Value; }
				if ( !z.IsNone ) { value.z = z.Value; }
				isoObject.position +=
					perSecond ? value * Time.deltaTime : value;
			}
		}
	}
}
#endif