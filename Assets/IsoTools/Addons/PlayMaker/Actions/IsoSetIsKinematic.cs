#if PLAYMAKER
using UnityEngine;
using IsoTools.Physics;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools.Physics")]
	[HutongGames.PlayMaker.Tooltip(
		"Sets the IsKinematic of a IsoRigidbody.")]
	public class IsoSetIsKinematic : IsoComponentAction<IsoRigidbody> {
		[RequiredField]
		[CheckForComponent(typeof(IsoRigidbody))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmBool isKinematic;

		public override void Reset() {
			gameObject  = null;
			isKinematic = true;
		}

		public override void OnEnter() {
			DoAction();
			Finish();
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				isoRigidbody.isKinematic = isKinematic.Value;
			}
		}
	}
}
#endif