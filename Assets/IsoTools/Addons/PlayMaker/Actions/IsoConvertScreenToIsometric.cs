#if PLAYMAKER
using UnityEngine;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Convert ScreenSpace Vector2 Variable to Isometric Vector3 Variable")]
	public class IsoConvertScreenToIsometric : IsoComponentAction<IsoWorld> {
		[RequiredField]
		[CheckForComponent(typeof(IsoWorld))]
		[HutongGames.PlayMaker.Title("IsoWorld (In)")]
		[HutongGames.PlayMaker.Tooltip("The IsoWorld for convertation.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Screen Vector (In)")]
		public FsmVector2 screenVector;

		[HutongGames.PlayMaker.Title("Specific Isometric Z (In)")]
		[HutongGames.PlayMaker.Tooltip("Specific Isometric Z or 0.0f for 'None'")]
		public FsmFloat specificIsometricZ;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Isometric Vector (Out)")]
		public FsmVector3 storeIsometricVector;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Isometric X (Out)")]
		public FsmFloat storeIsometricX;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Isometric Y (Out)")]
		public FsmFloat storeIsometricY;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Store Isometric Z (Out)")]
		public FsmFloat storeIsometricZ;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset() {
			gameObject           = null;
			screenVector         = null;
			specificIsometricZ   = 0.0f;
			storeIsometricVector = null;
			storeIsometricX      = null;
			storeIsometricY      = null;
			storeIsometricZ      = null;
			everyFrame           = false;
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
				var value = specificIsometricZ.IsNone
					? isoWorld.ScreenToIso(screenVector.Value)
					: isoWorld.ScreenToIso(screenVector.Value, specificIsometricZ.Value);
				storeIsometricVector.Value = value;
				storeIsometricX.Value      = value.x;
				storeIsometricY.Value      = value.y;
				storeIsometricZ.Value      = value.z;
			}
		}
	}
}
#endif