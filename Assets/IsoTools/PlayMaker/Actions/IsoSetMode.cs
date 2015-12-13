using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip("Sets the Mode of a IsoObject.")]
	public class IsoSetMode : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		public FsmOwnerDefault gameObject;

		[ObjectType(typeof(IsoObject.Mode))]
		public FsmEnum mode;

		public bool everyFrame;
		public bool lateUpdate;
		public bool fixedUpdate;

		public override void Reset() {
			gameObject  = null;
			mode        = new FsmEnum{UseVariable = true};
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
				isoObject.mode = (IsoObject.Mode)mode.Value;
			}
		}
	}
} // IsoTools.PlayMaker.Actions