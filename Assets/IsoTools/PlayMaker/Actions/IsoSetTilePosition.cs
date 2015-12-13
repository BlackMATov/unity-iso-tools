using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip("Sets the TilePosition of a IsoObject. To leave any axis unchanged, set variable to 'None'.")]
	public class IsoSetTilePosition : FsmStateAction {
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The GameObject to position.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Use a stored Vector3 position, and/or set individual axis below.")]
		public FsmVector3 vector;

		public FsmFloat x;
		public FsmFloat y;
		public FsmFloat z;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		[HutongGames.PlayMaker.Tooltip("Perform in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
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
				DoSetPosition();
				Finish();
			}
		}

		public override void OnUpdate() {
			if ( !lateUpdate ) {
				DoSetPosition();
			}
		}

		public override void OnLateUpdate() {
			if ( lateUpdate ) {
				DoSetPosition();
			}
			if ( !everyFrame ) {
				Finish();
			}
		}

		void DoSetPosition() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			var iso_object = go ? go.GetComponent<IsoObject>() : null;
			if ( iso_object ) {
				var position = vector.IsNone
					? iso_object.tilePosition
					: vector.Value;

				if ( !x.IsNone ) {
					position.x = x.Value;
				}
				if ( !y.IsNone ) {
					position.y = y.Value;
				}
				if ( !z.IsNone ) {
					position.z = z.Value;
				}

				iso_object.tilePosition = position;
			}
		}
	}
} // IsoTools.PlayMaker.Actions