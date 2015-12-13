// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Rotates a Game Object around each Axis. Use a Vector3 Variable and/or XYZ components. To leave any axis unchanged, set variable to 'None'.")]
	public class Rotate : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The game object to rotate.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("A rotation vector. NOTE: You can override individual axis below.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;
		
		[Tooltip("Rotation around x axis.")]
		public FsmFloat xAngle;

		[Tooltip("Rotation around y axis.")]
		public FsmFloat yAngle;

		[Tooltip("Rotation around z axis.")]
		public FsmFloat zAngle;
		
		[Tooltip("Rotate in local or world space.")]
		public Space space;
		
		[Tooltip("Rotate over one second")]
		public bool perSecond;
		
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
		
		[Tooltip("Perform the rotation in LateUpdate. This is useful if you want to override the rotation of objects that are animated or otherwise rotated in Update.")]
		public bool lateUpdate;

        [Tooltip("Perform the rotation in FixedUpdate. This is useful when working with rigid bodies and physics.")]
        public bool fixedUpdate;	
		
		public override void Reset()
		{
			gameObject = null;
			vector = null;
			// default axis to variable dropdown with None selected.
			xAngle = new FsmFloat { UseVariable = true };
			yAngle = new FsmFloat { UseVariable = true };
			zAngle = new FsmFloat { UseVariable = true };
			space = Space.Self;
			perSecond = false;
			everyFrame = true;
			lateUpdate = false;
		    fixedUpdate = false;
		}

        public override void OnPreprocess()
        {
            Fsm.HandleFixedUpdate = true;
        }

		public override void OnEnter()
		{
			if(!everyFrame && !lateUpdate && !fixedUpdate)
			{
				DoRotate();
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (!lateUpdate && !fixedUpdate)
			{
				DoRotate();
			}
		}

		public override void OnLateUpdate()
		{
			if (lateUpdate)
			{
				DoRotate();
			}

			if (!everyFrame)
			{
				Finish();
			}
		}

        public override void OnFixedUpdate()
        {
            if (fixedUpdate)
            {
                DoRotate();
            }

            if (!everyFrame)
            {
                Finish();
            }
        }

		void DoRotate()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			// Use vector if specified

			var rotate = vector.IsNone ? new Vector3(xAngle.Value, yAngle.Value, zAngle.Value) : vector.Value;

			// override any axis

			if (!xAngle.IsNone) rotate.x = xAngle.Value;
			if (!yAngle.IsNone) rotate.y = yAngle.Value;
			if (!zAngle.IsNone) rotate.z = zAngle.Value;
		
			// apply
			
			if (!perSecond)
			{
				go.transform.Rotate(rotate, space);
			}
			else
			{
				go.transform.Rotate(rotate * Time.deltaTime, space);
			}
		}

	}
}