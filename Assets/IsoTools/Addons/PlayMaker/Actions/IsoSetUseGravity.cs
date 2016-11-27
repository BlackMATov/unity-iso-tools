#if PLAYMAKER
using UnityEngine;
using IsoTools.Physics;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools.Physics")]
	[HutongGames.PlayMaker.Tooltip(
		"Sets the UseGravity of a IsoRigidbody.")]
	public class IsoSetUseGravity : IsoComponentAction<IsoRigidbody> {
		[RequiredField]
		[CheckForComponent(typeof(IsoRigidbody))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmBool useGravity;

		public override void Reset() {
			gameObject = null;
			useGravity = true;
		}

		public override void OnEnter() {
			DoAction();
			Finish();
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				isoRigidbody.useGravity = useGravity.Value;
			}
		}
	}
}
#endif