// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Character)]
	[Tooltip("Gets info on the last Character Controller collision and store in variables.")]
	public class GetControllerHitInfo : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;
		[UIHint(UIHint.Variable)]
		public FsmVector3 contactPoint;
		[UIHint(UIHint.Variable)]
		public FsmVector3 contactNormal;
		[UIHint(UIHint.Variable)]
		public FsmVector3 moveDirection;
		[UIHint(UIHint.Variable)]
		public FsmFloat moveLength;
		[UIHint(UIHint.Variable)]
		[Tooltip("Useful for triggering different effects. Audio, particles...")]
		public FsmString physicsMaterialName;

		public override void Reset()
		{
			gameObjectHit = null;
			contactPoint = null;
			contactNormal = null;
			moveDirection = null;
			moveLength = null;
			physicsMaterialName = null;
		}

		void StoreTriggerInfo()
		{
			if (Fsm.ControllerCollider == null) return;
			
			gameObjectHit.Value = Fsm.ControllerCollider.gameObject;
			contactPoint.Value = Fsm.ControllerCollider.point;
			contactNormal.Value = Fsm.ControllerCollider.normal;
			moveDirection.Value = Fsm.ControllerCollider.moveDirection;
			moveLength.Value = Fsm.ControllerCollider.moveLength;
			physicsMaterialName.Value = Fsm.ControllerCollider.collider.material.name;
		}

		public override void OnEnter()
		{
			StoreTriggerInfo();
			
			Finish();
		}

		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysicsSetup(Owner);
		}
	}
}