#if PLAYMAKER
using UnityEngine;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Gets an options of a IsoWorld.")]
	public class IsoGetWorldProps : IsoComponentAction<IsoWorld> {
		[RequiredField]
		[CheckForComponent(typeof(IsoWorld))]
		[HutongGames.PlayMaker.Title("IsoWorld (In)")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Tile Size (Out)")]
		public FsmFloat tileSize;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Tile Ratio (Out)")]
		public FsmFloat tileRatio;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Tile Angle (Out)")]
		public FsmFloat tileAngle;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Tile Height (Out)")]
		public FsmFloat tileHeight;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Step Depth (Out)")]
		public FsmFloat stepDepth;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Title("Start Depth (Out)")]
		public FsmFloat startDepth;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset() {
			gameObject = null;
			tileSize   = null;
			tileRatio  = null;
			tileAngle  = null;
			tileHeight = null;
			stepDepth  = null;
			startDepth = null;
			everyFrame = false;
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
				tileSize.Value   = isoWorld.tileSize;
				tileRatio.Value  = isoWorld.tileRatio;
				tileAngle.Value  = isoWorld.tileAngle;
				tileHeight.Value = isoWorld.tileHeight;
				stepDepth.Value  = isoWorld.stepDepth;
				startDepth.Value = isoWorld.startDepth;
			}
		}
	}
}
#endif