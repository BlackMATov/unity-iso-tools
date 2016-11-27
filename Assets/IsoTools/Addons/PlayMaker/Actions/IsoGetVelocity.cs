#if PLAYMAKER
using UnityEngine;
using IsoTools.Physics;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools.Physics")]
	[HutongGames.PlayMaker.Tooltip(
		"Gets the Velocity of a IsoRigidbody and stores it " +
		"in a Vector3 Variable or each Axis in a Float Variable")]
	public class IsoGetVelocity : IsoComponentAction<IsoRigidbody> {
		[RequiredField]
		[CheckForComponent(typeof(IsoRigidbody))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		public FsmVector3 storeVector;

		[UIHint(UIHint.Variable)]
		public FsmFloat storeX;

		[UIHint(UIHint.Variable)]
		public FsmFloat storeY;

		[UIHint(UIHint.Variable)]
		public FsmFloat storeZ;

		public bool everyFrame;

		public override void Reset() {
			gameObject  = null;
			storeVector = null;
			storeX      = null;
			storeY      = null;
			storeZ      = null;
			everyFrame  = false;
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
				var value         = isoRigidbody.velocity;
				storeVector.Value = value;
				storeX.Value      = value.x;
				storeY.Value      = value.y;
				storeZ.Value      = value.z;
			}
		}
	}
}
#endif