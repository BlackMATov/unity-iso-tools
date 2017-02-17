#if PLAYMAKER
using UnityEngine;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Gets the Renderers Mode of a IsoObject and stores it in a Enum Variable")]
	public class IsoGetMode : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		[HutongGames.PlayMaker.Title("IsoObject (In)")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[ObjectType(typeof(IsoObject.RenderersMode))]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Renderers Mode (Out)")]
		public FsmEnum storeRenderersMode;

		public override void Reset() {
			gameObject         = null;
			storeRenderersMode = null;
		}

		public override void OnEnter() {
			DoAction();
			Finish();
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				storeRenderersMode.Value = isoObject.renderersMode;
			}
		}
	}
}
#endif