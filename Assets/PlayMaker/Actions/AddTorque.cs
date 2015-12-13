// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Adds torque (rotational force) to a Game Object.")]
	public class AddTorque : ComponentAction<Rigidbody>
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
        [Tooltip("The GameObject to add torque to.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("A Vector3 torque. Optionally override any axis with the X, Y, Z parameters.")]
		public FsmVector3 vector;

		[Tooltip("Torque around the X axis. To leave unchanged, set to 'None'.")]
		public FsmFloat x;

        [Tooltip("Torque around the Y axis. To leave unchanged, set to 'None'.")]
		public FsmFloat y;

        [Tooltip("Torque around the Z axis. To leave unchanged, set to 'None'.")]
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
			DoAddTorque();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}

		public override void OnFixedUpdate()
		{
			DoAddTorque();
		}

		void DoAddTorque()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (!UpdateCache(go))
			{
				return;
			}

			var torque = vector.IsNone ? new Vector3(x.Value, y.Value, z.Value) : vector.Value;
			
			// override any axis

			if (!x.IsNone) torque.x = x.Value;
			if (!y.IsNone) torque.y = y.Value;
			if (!z.IsNone) torque.z = z.Value;
					
			// apply
			
			if (space == Space.World)
			{
				rigidbody.AddTorque(torque, forceMode);
			}
			else
			{
				rigidbody.AddRelativeTorque(torque, forceMode);
			}
		}


	}
}