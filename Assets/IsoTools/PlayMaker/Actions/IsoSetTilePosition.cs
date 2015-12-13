using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip("Sets the TilePosition of a IsoObject. To leave any axis unchanged, set variable to 'None'.")]
	public class IsoSetTilePosition : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		public FsmFloat x;
		public FsmFloat y;
		public FsmFloat z;

		public bool everyFrame;
		public bool lateUpdate;

		public override void Reset() {
			gameObject = null;
			vector     = null;
			x          = new FsmFloat{UseVariable = true};
			y          = new FsmFloat{UseVariable = true};
			z          = new FsmFloat{UseVariable = true};
			everyFrame = false;
			lateUpdate = false;
		}

		public override void OnEnter() {
			if ( !everyFrame && !lateUpdate ) {
				DoSetTilePosition();
				Finish();
			}
		}

		public override void OnUpdate() {
			if ( !lateUpdate ) {
				DoSetTilePosition();
			}
		}

		public override void OnLateUpdate() {
			if ( lateUpdate ) {
				DoSetTilePosition();
			}
			if ( !everyFrame ) {
				Finish();
			}
		}

		void DoSetTilePosition() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				var tile_position = vector.IsNone
					? isoObject.tilePosition
					: vector.Value;

				if ( !x.IsNone ) {
					tile_position.x = x.Value;
				}
				if ( !y.IsNone ) {
					tile_position.y = y.Value;
				}
				if ( !z.IsNone ) {
					tile_position.z = z.Value;
				}

				isoObject.tilePosition = tile_position;
			}
		}
	}
} // IsoTools.PlayMaker.Actions