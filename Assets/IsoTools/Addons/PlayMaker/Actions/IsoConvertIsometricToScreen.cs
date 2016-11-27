#if PLAYMAKER
using UnityEngine;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Convert Isometric Vector3 Variable to ScreenSpace Vector2 Variable")]
	public class IsoConvertIsometricToScreen : IsoComponentAction<IsoWorld> {
		[RequiredField]
		[CheckForComponent(typeof(IsoWorld))]
		[HutongGames.PlayMaker.Title("IsoWorld (In)")]
		[HutongGames.PlayMaker.Tooltip("The IsoWorld for convertation.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Isometric Vector (In)")]
		public FsmVector3 isometricVector;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Screen Vector (Out)")]
		public FsmVector2 storeScreenVector;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Screen X (Out)")]
		public FsmFloat storeScreenX;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Screen Y (Out)")]
		public FsmFloat storeScreenY;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset() {
			gameObject        = null;
			isometricVector   = null;
			storeScreenVector = null;
			storeScreenX      = null;
			storeScreenY      = null;
			everyFrame        = false;
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
				var value = isoWorld.IsoToScreen(isometricVector.Value);
				storeScreenVector.Value = value;
				storeScreenX.Value      = value.x;
				storeScreenY.Value      = value.y;
			}
		}
	}
}
#endif