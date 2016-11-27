#if PLAYMAKER
using UnityEngine;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Sets an options of a IsoWorld.")]
	public class IsoSetWorldProps : IsoComponentAction<IsoWorld> {
		[RequiredField]
		[CheckForComponent(typeof(IsoWorld))]
		[HutongGames.PlayMaker.Title("IsoWorld (In)")]
		public FsmOwnerDefault gameObject;

		[HutongGames.PlayMaker.Title("Tile Size (In)")]
		public FsmFloat tileSize;

		[HutongGames.PlayMaker.Title("Tile Ratio (In)")]
		public FsmFloat tileRatio;

		[HutongGames.PlayMaker.Title("Tile Angle (In)")]
		public FsmFloat tileAngle;

		[HutongGames.PlayMaker.Title("Tile Height (In)")]
		public FsmFloat tileHeight;

		[HutongGames.PlayMaker.Title("Step Depth (In)")]
		public FsmFloat stepDepth;

		[HutongGames.PlayMaker.Title("Start Depth (In)")]
		public FsmFloat startDepth;

		public override void Reset() {
			gameObject = null;
			tileSize   = new FsmFloat{UseVariable = true};
			tileRatio  = new FsmFloat{UseVariable = true};
			tileAngle  = new FsmFloat{UseVariable = true};
			tileHeight = new FsmFloat{UseVariable = true};
			stepDepth  = new FsmFloat{UseVariable = true};
			startDepth = new FsmFloat{UseVariable = true};
		}

		public override string ErrorCheck() {
			if ( !tileSize.IsNone && IsErrorVarClamp(tileSize.Value, IsoWorld.MinTileSize, IsoWorld.MaxTileSize) ) {
				return ErrorVarClampMsg("TileSize", IsoWorld.MinTileSize, IsoWorld.MaxTileSize);
			}
			if ( !tileRatio.IsNone && IsErrorVarClamp(tileRatio.Value, IsoWorld.MinTileRatio, IsoWorld.MaxTileRatio) ) {
				return ErrorVarClampMsg("TileRatio", IsoWorld.MinTileRatio, IsoWorld.MaxTileRatio);
			}
			if ( !tileAngle.IsNone && IsErrorVarClamp(tileAngle.Value, IsoWorld.MinTileAngle, IsoWorld.MaxTileAngle)) {
				return ErrorVarClampMsg("TileAngle", IsoWorld.MinTileAngle, IsoWorld.MaxTileAngle);
			}
			if ( !tileHeight.IsNone && IsErrorVarClamp(tileHeight.Value, IsoWorld.MinTileHeight, IsoWorld.MaxTileHeight) ) {
				return ErrorVarClampMsg("TileHeight", IsoWorld.MinTileHeight, IsoWorld.MaxTileHeight);
			}
			if ( !stepDepth.IsNone && IsErrorVarClamp(stepDepth.Value, IsoWorld.MinStepDepth, IsoWorld.MaxStepDepth) ) {
				return ErrorVarClampMsg("StepDepth", IsoWorld.MinStepDepth, IsoWorld.MaxStepDepth);
			}
			if ( !startDepth.IsNone && IsErrorVarClamp(startDepth.Value, IsoWorld.MinStartDepth, IsoWorld.MaxStartDepth) ) {
				return ErrorVarClampMsg("StartDepth", IsoWorld.MinStartDepth, IsoWorld.MaxStartDepth);
			}
			return "";
		}

		public override void OnEnter() {
			DoAction();
			Finish();
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				if ( !tileSize  .IsNone ) { isoWorld.tileSize   = tileSize  .Value; }
				if ( !tileRatio .IsNone ) { isoWorld.tileRatio  = tileRatio .Value; }
				if ( !tileAngle .IsNone ) { isoWorld.tileAngle  = tileAngle .Value; }
				if ( !tileHeight.IsNone ) { isoWorld.tileHeight = tileHeight.Value; }
				if ( !stepDepth .IsNone ) { isoWorld.stepDepth  = stepDepth .Value; }
				if ( !startDepth.IsNone ) { isoWorld.startDepth = startDepth.Value; }
			}
		}
	}
}
#endif