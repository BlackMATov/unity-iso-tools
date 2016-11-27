#if PLAYMAKER
using UnityEngine;
using IsoTools.Physics;
using HutongGames.PlayMaker;
using IsoTools.PlayMaker.Internal;

namespace IsoTools.PlayMaker.Actions {
	[ActionCategory("IsoTools.Physics")]
	[HutongGames.PlayMaker.Tooltip(
		"Applies a force to a IsoRigidbody that simulates explosion effects. " +
		"The explosion force will fall off linearly with distance.")]
	public class IsoAddExplosionForce : IsoComponentAction<IsoRigidbody> {
		[RequiredField]
		[CheckForComponent(typeof(IsoRigidbody))]
		[HutongGames.PlayMaker.Tooltip(
			"The IsoRigidbody to add the explosion force to.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[HutongGames.PlayMaker.Tooltip(
			"The center of the explosion.")]
		public FsmVector3 center;

		[RequiredField]
		[HutongGames.PlayMaker.Tooltip(
			"The strength of the explosion.")]
		public FsmFloat force;

		[RequiredField]
		[HutongGames.PlayMaker.Tooltip(
			"The radius of the explosion.")]
		public FsmFloat radius;

		[HutongGames.PlayMaker.Tooltip(
			"Applies the force as if it was applied from beneath the object.")]
		public FsmFloat upwardsModifier;

		[HutongGames.PlayMaker.Tooltip(
			"The type of force to apply.")]
		public ForceMode forceMode;

		[HutongGames.PlayMaker.Tooltip(
			"Repeat every frame.")]
		public bool everyFrame;

		public override void Reset() {
			gameObject      = null;
			center          = null;
			force           = null;
			radius          = null;
			upwardsModifier = 0.0f;
			forceMode       = ForceMode.Force;
			everyFrame      = false;
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
				isoRigidbody.AddExplosionForce(
					force.Value, center.Value, radius.Value,
					upwardsModifier.Value, forceMode);
			}
		}
	}
}
#endif