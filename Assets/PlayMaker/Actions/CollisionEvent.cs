// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Detect collisions between the Owner of this FSM and other Game Objects that have RigidBody components.\nNOTE: The system events, COLLISION ENTER, COLLISION STAY, and COLLISION EXIT are sent automatically on collisions with any object. Use this action to filter collisions by Tag.")]
	public class CollisionEvent : FsmStateAction
	{
        [Tooltip("The type of collision to detect.")]
		public CollisionType collision;
		
        [UIHint(UIHint.Tag)]
		[Tooltip("Filter by Tag.")]
        public FsmString collideTag;
		
        [Tooltip("Event to send if a collision is detected.")]
        public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
        [Tooltip("Store the GameObject that collided with the Owner of this FSM.")]
		public FsmGameObject storeCollider;

		[UIHint(UIHint.Variable)]
        [Tooltip("Store the force of the collision. NOTE: Use Get Collision Info to get more info about the collision.")]
		public FsmFloat storeForce;
		
		public override void Reset()
		{
			collision = CollisionType.OnCollisionEnter;
			collideTag = "Untagged";
			sendEvent = null;
			storeCollider = null;
			storeForce = null;
		}

	    public override void OnPreprocess()
	    {
            switch (collision)
            {
                case CollisionType.OnCollisionEnter:
                    Fsm.HandleCollisionEnter = true;
                    break;
                case CollisionType.OnCollisionStay:
                    Fsm.HandleCollisionStay = true;
                    break;
                case CollisionType.OnCollisionExit:
                    Fsm.HandleCollisionExit = true;
                    break;
            }

	    }

		void StoreCollisionInfo(Collision collisionInfo)
		{
			storeCollider.Value = collisionInfo.gameObject;
			storeForce.Value = collisionInfo.relativeVelocity.magnitude;
		}

		public override void DoCollisionEnter(Collision collisionInfo)
		{
			if (collision == CollisionType.OnCollisionEnter)
			{
				if (collisionInfo.collider.gameObject.tag == collideTag.Value)
				{
					StoreCollisionInfo(collisionInfo);
					Fsm.Event(sendEvent);
				}
			}
		}

		public override void DoCollisionStay(Collision collisionInfo)
		{
			if (collision == CollisionType.OnCollisionStay)
			{
				if (collisionInfo.collider.gameObject.tag == collideTag.Value)
				{
					StoreCollisionInfo(collisionInfo);
					Fsm.Event(sendEvent);
				}
			}
		}

		public override void DoCollisionExit(Collision collisionInfo)
		{
			if (collision == CollisionType.OnCollisionExit)
			{
				if (collisionInfo.collider.gameObject.tag == collideTag.Value)
				{
					StoreCollisionInfo(collisionInfo);
					Fsm.Event(sendEvent);
				}
			}
		}

		public override void DoControllerColliderHit(ControllerColliderHit collisionInfo)
		{
			if (collision == CollisionType.OnControllerColliderHit)
			{
				if (collisionInfo.collider.gameObject.tag == collideTag.Value)
				{
					if (storeCollider != null)
						storeCollider.Value = collisionInfo.gameObject;

					storeForce.Value = 0f; //TODO: impact force?
					Fsm.Event(sendEvent);
				}
			}
		}

		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysicsSetup(Owner);
		}
	}
}