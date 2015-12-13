using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip("Gets the Mode of a IsoObject and stores it in a Enum Variable")]
	public class IsoGetMode : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		public FsmOwnerDefault gameObject;

		[ObjectType(typeof(IsoObject.Mode))]
		[UIHint(UIHint.Variable)]
		public FsmEnum mode;

		public bool everyFrame;

		public override void Reset() {
			gameObject = null;
			mode       = null;
			everyFrame = false;
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
				var value  = isoObject.mode;
				mode.Value = value;
			}
		}
	}
} // IsoTools.PlayMaker.Actions