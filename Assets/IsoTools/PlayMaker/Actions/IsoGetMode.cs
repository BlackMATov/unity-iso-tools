using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip("Gets the Mode of a IsoObject and stores it in a Enum Variable")]
	public class IsoGetMode : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[ObjectType(typeof(IsoObject.Mode))]
		[UIHint(UIHint.Variable)]
		public FsmEnum mode;

		public override void Reset() {
			gameObject = null;
			mode       = null;
		}

		public override void OnEnter() {
			DoAction();
			Finish();
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				mode.Value = isoObject.mode;
			}
		}
	}
} // IsoTools.PlayMaker.Actions