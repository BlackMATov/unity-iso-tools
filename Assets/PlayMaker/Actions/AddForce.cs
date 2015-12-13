// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Adds a force to a Game Object. Use Vector3 variable and/or Float variables for each axis.")]
	public class AddForce : ComponentAction<Rigidbody>
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
        [Tooltip("The GameObject to apply the force to.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally apply the force at a position on the object. This will also add some torque. The position is often returned from MousePick or GetCollisionInfo actions.")]
		public FsmVector3 atPosition;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("A Vector3 force to add. Optionally override any axis with the X, Y, Z parameters.")]
		public FsmVector3 vector;

        [Tooltip("Force along the X axis. To leave unchanged, set to 'None'.")]
		public FsmFloat x;

        [Tooltip("Force along the Y axis. To leave unchanged, set to 'None'.")]
		public FsmFloat y;

        [Tooltip("Force along the Z axis. To leave unchanged, set to 'None'.")]
		public FsmFloat z;

        [Tooltip("Apply the force in world or local space.")]
		public Space space;

        [Tooltip("The type of force to apply. See Unity Physics docs.")]
        public ForceMode forceMode;

        [Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			atPosition = new FsmVector3 { UseVariable = true };
			vector = null;
			// default axis to variable dropdown with None selected.
			x = new FsmFloat { UseVariable = true };
			y = new FsmFloat { UseVariable = true };
			z = new FsmFloat { UseVariable = true };
			space = Space.World;
			forceMode = ForceMode.Force;
			everyFrame = false;
		}

        public override void OnPreprocess()
        {
            Fsm.HandleFixedUpdate = true;
        }

		public override void OnEnter()
		{
			DoAddForce();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}

		public override void OnFixedUpdate()
		{
			DoAddForce();
		}

		void DoAddForce()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (!UpdateCache(go))
			{
				return;
			}

			var force = vector.IsNone ? new Vector3() : vector.Value;
			
			// override any axis

			if (!x.IsNone) force.x = x.Value;
			if (!y.IsNone) force.y = y.Value;
			if (!z.IsNone) force.z = z.Value;
					
			// apply force			
				
			if (space == Space.World)
			{
				if (!atPosition.IsNone)
				{
					rigidbody.AddForceAtPosition(force, atPosition.Value, forceMode);
				}
				else
				{
					rigidbody.AddForce(force, forceMode);
				}
			}
			else
			{
				rigidbody.AddRelativeForce(force,forceMode);
			}
		}
	}
}