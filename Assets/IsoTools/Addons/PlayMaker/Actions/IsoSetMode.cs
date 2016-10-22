#if PLAYMAKER
using UnityEngine;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Sets the Mode of a IsoObject.")]
	public class IsoSetMode : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		[HutongGames.PlayMaker.Title("IsoObject (In)")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[ObjectType(typeof(IsoObject.Mode))]
		[HutongGames.PlayMaker.Title("Mode (In)")]
		public FsmEnum mode;

		public override void Reset() {
			gameObject  = null;
			mode        = IsoObject.Mode.Mode2d;
		}

		public override void OnEnter() {
			DoAction();
			Finish();
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				isoObject.mode = (IsoObject.Mode)mode.Value;
			}
		}
	}
} // IsoTools.PlayMaker.Actions
#endif
