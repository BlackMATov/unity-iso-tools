// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Sets the Rotation of a Game Object. To leave any axis unchanged, set variable to 'None'.")]
	public class SetRotation : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to rotate.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("Use a stored quaternion, or vector angles below.")]
		public FsmQuaternion quaternion;
		
		[UIHint(UIHint.Variable)]
		[Title("Euler Angles")]
		[Tooltip("Use euler angles stored in a Vector3 variable, and/or set each axis below.")]
		public FsmVector3 vector;
		
		public FsmFloat xAngle;
		public FsmFloat yAngle;
		public FsmFloat zAngle;

		[Tooltip("Use local or world space.")]
		public Space space;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		[Tooltip("Perform in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
		public bool lateUpdate;	

		public override void Reset()
		{
			gameObject = null;
			quaternion = null;
			vector = null;
			// default axis to variable dropdown with None selected.
			xAngle = new FsmFloat { UseVariable = true };
			yAngle = new FsmFloat { UseVariable = true };
			zAngle = new FsmFloat { UseVariable = true };
			space = Space.World;
			everyFrame = false;
			lateUpdate = false;
		}

		public override void OnEnter()
		{
			if (!everyFrame && !lateUpdate)
			{
				DoSetRotation();
				Finish();
			}	
		}

		public override void OnUpdate()
		{
			if (!lateUpdate)
			{
				DoSetRotation();
			}
		}

		public override void OnLateUpdate()
		{
			if (lateUpdate)
			{
				DoSetRotation();
			}

			if (!everyFrame)
			{
				Finish();
			}
		}

		void DoSetRotation()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			// Individual angle axis can override Quaternion and Vector angles
			// So we build up the final rotation in steps

			Vector3 rotation;

			if (!quaternion.IsNone)
			{
				rotation = quaternion.Value.eulerAngles;
			}
			else if (!vector.IsNone)
			{
				rotation = vector.Value;
			}
			else
			{
				// use current rotation of the game object

				rotation = space == Space.Self ? go.transform.localEulerAngles : go.transform.eulerAngles;
			}	
			
			// Override each axis
			
			if (!xAngle.IsNone)
			{
				rotation.x = xAngle.Value;
			}
			
			if (!yAngle.IsNone)
			{
				rotation.y = yAngle.Value;
			}
			
			if (!zAngle.IsNone)
			{
				rotation.z = zAngle.Value;
			}

			// apply rotation

			if (space == Space.Self)
			{
				go.transform.localEulerAngles = rotation;
			}
			else
			{
				go.transform.eulerAngles = rotation;
			}
		}
	}
}