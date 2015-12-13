using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip("Change IsoWorld options.")]
	public class IsoChangeWorld : FsmStateAction {

		public FsmFloat tileSize;
		public FsmFloat tileRatio;
		public FsmFloat tileAngle;
		public FsmFloat tileHeight;
		public FsmFloat stepDepth;
		public FsmFloat startDepth;

		public override void Reset() {
			tileSize   = new FsmFloat{UseVariable = true};
			tileRatio  = new FsmFloat{UseVariable = true};
			tileAngle  = new FsmFloat{UseVariable = true};
			tileHeight = new FsmFloat{UseVariable = true};
			stepDepth  = new FsmFloat{UseVariable = true};
			startDepth = new FsmFloat{UseVariable = true};
		}

		public override void OnEnter() {
			DoAction();
			Finish();
		}

		void DoAction() {
			var iso_world = GameObject.FindObjectOfType<IsoWorld>();
			if ( iso_world ) {
				if ( !tileSize.IsNone ) {
					iso_world.tileSize = tileSize.Value;
				}
				if ( !tileRatio.IsNone ) {
					iso_world.tileRatio = tileRatio.Value;
				}
				if ( !tileAngle.IsNone ) {
					iso_world.tileAngle = tileAngle.Value;
				}
				if ( !tileHeight.IsNone ) {
					iso_world.tileHeight = tileHeight.Value;
				}
				if ( !stepDepth.IsNone ) {
					iso_world.stepDepth = stepDepth.Value;
				}
				if ( !startDepth.IsNone ) {
					iso_world.startDepth = startDepth.Value;
				}
			}
		}
	}
} // IsoTools.PlayMaker.Actions