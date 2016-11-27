#if PLAYMAKER
using UnityEngine;
using IsoTools.Physics;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools.Physics")]
	[HutongGames.PlayMaker.Tooltip(
		"Adds a force to a IsoRigidbody. " +
		"Use Vector3 variable and/or Float variables for each axis.")]
	public class IsoAddForce : IsoComponentAction<IsoRigidbody> {
		[RequiredField]
		[CheckForComponent(typeof(IsoRigidbody))]
		[HutongGames.PlayMaker.Tooltip(
			"The IsoRigidbody to apply the force to.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip(
			"Optionally apply the force at a position on the object.")]
		public FsmVector3 atPosition;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip(
			"A Vector3 force to add. " +
			"Optionally override any axis with the X, Y, Z parameters.")]
		public FsmVector3 vector;

		[HutongGames.PlayMaker.Tooltip(
			"Force along the X axis. To leave unchanged, set to 'None'.")]
		public FsmFloat x;

		[HutongGames.PlayMaker.Tooltip(
			"Force along the Y axis. To leave unchanged, set to 'None'.")]
		public FsmFloat y;

		[HutongGames.PlayMaker.Tooltip(
			"Force along the Z axis. To leave unchanged, set to 'None'.")]
		public FsmFloat z;

		[HutongGames.PlayMaker.Tooltip(
			"Apply the force in world or local space.")]
		public Space space;

		[HutongGames.PlayMaker.Tooltip(
			"The type of force to apply.")]
		public ForceMode forceMode;

		[HutongGames.PlayMaker.Tooltip(
			"Repeat every frame.")]
		public bool everyFrame;

		public override void Reset() {
			gameObject = null;
			atPosition = new FsmVector3{UseVariable = true};
			vector     = null;
			x          = new FsmFloat{UseVariable = true};
			y          = new FsmFloat{UseVariable = true};
			z          = new FsmFloat{UseVariable = true};
			space      = Space.World;
			forceMode  = ForceMode.Force;
			everyFrame = false;
		}

		public override void OnPreprocess() {
			Fsm.HandleFixedUpdate = true;
		}

		public override void OnEnter() {
			DoAction();
			if ( !everyFrame ) {
				Finish();
			}
		}

		public override void OnFixedUpdate() {
			DoAction();
		}

		void DoAction() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if ( UpdateCache(go) ) {
				var value = vector.IsNone ? Vector3.zero : vector.Value;
				if ( !x.IsNone ) { value.x = x.Value; }
				if ( !y.IsNone ) { value.y = y.Value; }
				if ( !z.IsNone ) { value.z = z.Value; }
				if ( space == Space.World ) {
					if ( atPosition.IsNone ) {
						isoRigidbody.AddForce(value, forceMode);
					} else {
						isoRigidbody.AddForceAtPosition(
							value, atPosition.Value, forceMode);
					}
				} else {
					isoRigidbody.AddRelativeForce(value, forceMode);
				}
			}
		}
	}
}
#endif