using UnityEngine;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools")]
	[HutongGames.PlayMaker.Tooltip(
		"Gets the Position of a IsoObject and stores it " +
		"in a Vector3 Variable or each Axis in a Float Variable")]
	public class IsoGetPosition : IsoComponentAction<IsoObject> {
		[RequiredField]
		[CheckForComponent(typeof(IsoObject))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		[UIHint(UIHint.Variable)]
		public FsmFloat x;

		[UIHint(UIHint.Variable)]
		public FsmFloat y;

		[UIHint(UIHint.Variable)]
		public FsmFloat z;

		public bool everyFrame;

		public override void Reset() {
			gameObject = null;
			vector     = null;
			x          = null;
			y          = null;
			z          = null;
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
				var value    = isoObject.position;
				vector.Value = value;
				x.Value      = value.x;
				y.Value      = value.y;
				z.Value      = value.z;
			}
		}
	}
} // IsoTools.PlayMaker.Actions