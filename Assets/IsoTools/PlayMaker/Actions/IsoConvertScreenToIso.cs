using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Convert ScreenSpace Vector2 Variable to Isometric Vector3 Variable")]
	public class IsoConvertIsoToScreen : IsoComponentAction<IsoWorld> {
		[RequiredField]
		[CheckForComponent(typeof(IsoWorld))]
		[HutongGames.PlayMaker.Tooltip("The IsoWorld for convertation.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("The ScreenSpace Vector2 variable to convert to a Isometric Vector3.")]
		public FsmVector2 fromScreenVector;

		[HutongGames.PlayMaker.Tooltip("Specific Isometric Z.")]
		public FsmFloat specificIsometricZ;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Store the result in a Isometric Vector3 variable.")]
		public FsmVector3 toIsometricVector;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset() {
			gameObject         = null;
			fromScreenVector   = null;
			specificIsometricZ = 0.0f;
			toIsometricVector  = null;
			everyFrame         = false;
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
				toIsometricVector.Value = isoWorld.ScreenToIso(
					fromScreenVector.Value,
					specificIsometricZ.IsNone ? 0.0f : specificIsometricZ.Value);
			}
		}
	}
} // IsoTools.PlayMaker.Actions