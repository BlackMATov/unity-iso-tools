using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Gets an options of a IsoWorld.")]
	public class IsoGetWorldProps : IsoComponentAction<IsoWorld> {
		[RequiredField]
		[CheckForComponent(typeof(IsoWorld))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		public FsmFloat tileSize;

		[UIHint(UIHint.Variable)]
		public FsmFloat tileRatio;

		[UIHint(UIHint.Variable)]
		public FsmFloat tileAngle;

		[UIHint(UIHint.Variable)]
		public FsmFloat tileHeight;

		[UIHint(UIHint.Variable)]
		public FsmFloat stepDepth;

		[UIHint(UIHint.Variable)]
		public FsmFloat startDepth;

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
} // IsoTools.PlayMaker.Actions