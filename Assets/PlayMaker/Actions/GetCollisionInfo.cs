// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets info on the last collision event and store in variables. See Unity Physics docs.")]
	public class GetCollisionInfo : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
        [Tooltip("Get the GameObject hit.")]
		public FsmGameObject gameObjectHit;

		[UIHint(UIHint.Variable)]
        [Tooltip("Get the relative velocity of the collision.")]
		public FsmVector3 relativeVelocity;

		[UIHint(UIHint.Variable)]
        [Tooltip("Get the relative speed of the collision. Useful for controlling reactions. E.g., selecting an appropriate sound fx.")]
		public FsmFloat relativeSpeed;

		[UIHint(UIHint.Variable)]
		[Tooltip("Get the world position of the collision contact. Useful for spawning effects etc.")]
        public FsmVector3 contactPoint;

		[UIHint(UIHint.Variable)]
        [Tooltip("Get the collision normal vector. Useful for aligning spawned effects etc.")]
		public FsmVector3 contactNormal;

		[UIHint(UIHint.Variable)]
		[Tooltip("Get the name of the physics material of the colliding GameObject. Useful for triggering different effects. Audio, particles...")]
		public FsmString physicsMaterialName;

		public override void Reset()
		{
			gameObjectHit = null;
			relativeVelocity = null;
			relativeSpeed = null;
			contactPoint = null;
			contactNormal = null;
			physicsMaterialName = null;
		}

		void StoreCollisionInfo()
		{
			if (Fsm.CollisionInfo == null)
			{
			    return;
			}
			
			gameObjectHit.Value = Fsm.CollisionInfo.gameObject;
			relativeSpeed.Value = Fsm.CollisionInfo.relativeVelocity.magnitude;
			relativeVelocity.Value = Fsm.CollisionInfo.relativeVelocity;
			physicsMaterialName.Value = Fsm.CollisionInfo.collider.material.name;

			if (Fsm.CollisionInfo.contacts != null && Fsm.CollisionInfo.contacts.Length > 0)
			{
				contactPoint.Value = Fsm.CollisionInfo.contacts[0].point;
				contactNormal.Value = Fsm.CollisionInfo.contacts[0].normal;
			}
		}

		public override void OnEnter()
		{
			StoreCollisionInfo();
			
			Finish();
		}
	}
}