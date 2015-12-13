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