#if PLAYMAKER
using UnityEngine;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Gets the Size of a IsoObject and stores it " +
		"in a Vector3 Variable or each Axis in a Float Variable")]
	public class IsoGetSize : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		[HutongGames.PlayMaker.Title("IsoObject (In)")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Size Vector (Out)")]
		public FsmVector3 storeVector;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Size X (Out)")]
		public FsmFloat storeX;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Size Y (Out)")]
		public FsmFloat storeY;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Size Z (Out)")]
		public FsmFloat storeZ;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
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
				var value         = isoObject.size;
				storeVector.Value = value;
				storeX.Value      = value.x;
				storeY.Value      = value.y;
				storeZ.Value      = value.z;
			}
		}
	}
}
#endif