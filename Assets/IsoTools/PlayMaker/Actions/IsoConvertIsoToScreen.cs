using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Convert Isometric Vector3 Variable to ScreenSpace Vector2 Variable")]
	public class IsoConvertScreenToIso : IsoComponentAction<IsoWorld> {
		[RequiredField]
		[CheckForComponent(typeof(IsoWorld))]
		[HutongGames.PlayMaker.Tooltip("The IsoWorld for convertation.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("The Isometric Vector3 variable to convert to a ScreenSpace Vector2.")]
		public FsmVector3 fromIsometricVector;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Store the result in a ScreenSpace Vector2 variable.")]
		public FsmVector2 toScreenVector;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset() {
			gameObject          = null;
			fromIsometricVector = null;
			toScreenVector      = null;
			everyFrame          = false;
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
				toScreenVector.Value =
					isoWorld.IsoToScreen(fromIsometricVector.Value);
			}
		}
	}
} // IsoTools.PlayMaker.Actions