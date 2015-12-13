using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip("Sets the Position of a IsoObject. To leave any axis unchanged, set variable to 'None'.")]
	public class IsoSetPosition : IsoComponentAction<IsoObject> {
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
			if ( UpdateCache(go) ) {
				var position = vector.IsNone
					? isoObject.position
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

				isoObject.position = position;
			}
		}
	}
} // IsoTools.PlayMaker.Actions