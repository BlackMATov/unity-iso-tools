#if PLAYMAKER
using UnityEngine;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Sets the Renderers Mode of a IsoObject.")]
	public class IsoSetMode : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		[HutongGames.PlayMaker.Title("IsoObject (In)")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[ObjectType(typeof(IsoObject.RenderersMode))]
		[HutongGames.PlayMaker.Title("Renderers Mode (In)")]
		public FsmEnum renderersMode;

		public override void Reset() {
			gameObject    = null;
			renderersMode = IsoObject.RenderersMode.Mode2d;
		}

		public override void OnEnter() {
			DoAction();
			Finish();
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				isoObject.renderersMode = (IsoObject.RenderersMode)renderersMode.Value;
			}
		}
	}
}
#endif