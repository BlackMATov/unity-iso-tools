using UnityEngine;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Gets the Mode of a IsoObject and stores it in a Enum Variable")]
	public class IsoGetMode : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		[HutongGames.PlayMaker.Title("IsoObject (In)")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[ObjectType(typeof(IsoObject.Mode))]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Mode (Out)")]
		public FsmEnum storeMode;

		public override void Reset() {
			gameObject = null;
			storeMode  = null;
		}

		public override void OnEnter() {
			DoAction();
			Finish();
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				storeMode.Value = isoObject.mode;
			}
		}
	}
} // IsoTools.PlayMaker.Actions