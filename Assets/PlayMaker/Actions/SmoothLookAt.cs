// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Smoothly Rotates a Game Object so its forward vector points at a Target. The target can be defined as a Game Object or a world Position. If you specify both, then the position will be used as a local offset from the object's position.")]
	public class SmoothLookAt : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to rotate to face a target.")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("A target GameObject.")]
		public FsmGameObject targetObject;
		
		[Tooltip("A target position. If a Target Object is defined, this is used as a local offset.")]
		public FsmVector3 targetPosition;

		[Tooltip("Used to keep the game object generally upright. If left undefined the world y axis is used.")]
		public FsmVector3 upVector;
		
		[Tooltip("Force the game object to remain vertical. Useful for characters.")]
		public FsmBool keepVertical;
		
		[HasFloatSlider(0.5f,15)]
		[Tooltip("How fast the look at moves.")]
		public FsmFloat speed;
		
		[Tooltip("Draw a line in the Scene View to the look at position.")]
		public FsmBool debug;

		[Tooltip("If the angle to the target is less than this, send the Finish Event below. Measured in degrees.")]
		public FsmFloat finishTolerance;

		[Tooltip("Event to send if the angle to target is less than the Finish Tolerance.")]
		public FsmEvent finishEvent;

		private GameObject previousGo; // track game object so we can re-initialize when it changes.
		private Quaternion lastRotation;
		private Quaternion desiredRotation;
		
		public override void Reset()
		{
			gameObject = null;
			targetObject = null;
			targetPosition = new FsmVector3 { UseVariable = true};
			upVector = new FsmVector3 { UseVariable = true};
			keepVertical = true;
			debug = false;
			speed = 5;
			finishTolerance = 1;
			finishEvent = null;
		}

		public override void OnEnter()
		{
			previousGo = null;
		}

		public override void OnLateUpdate()
		{
			DoSmoothLookAt();
		}

		void DoSmoothLookAt()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			var goTarget = targetObject.Value;
			if (goTarget == null && targetPosition.IsNone)
			{
				return;
			}

			// re-initialize if game object has changed
			
			if (previousGo != go)
			{
				lastRotation = go.transform.rotation;
				desiredRotation = lastRotation;
				previousGo = go;
			}
			
			// desired look at position

			Vector3 lookAtPos;
			if (goTarget != null)
			{
				lookAtPos = !targetPosition.IsNone ?
					goTarget.transform.TransformPoint(targetPosition.Value) : 
					goTarget.transform.position;
			}
			else
			{
				lookAtPos = targetPosition.Value;
			}
			
			if (keepVertical.Value)
			{
				lookAtPos.y = go.transform.position.y;
			}
			
			// smooth look at

            var diff = lookAtPos - go.transform.position;
            if (diff != Vector3.zero && diff.sqrMagnitude > 0)
			{
				desiredRotation = Quaternion.LookRotation(diff, upVector.IsNone ? Vector3.up : upVector.Value);
			}

			lastRotation = Quaternion.Slerp(lastRotation, desiredRotation, speed.Value * Time.deltaTime);
			go.transform.rotation = lastRotation;
			
			// debug line to target
			
			if (debug.Value)
			{
				Debug.DrawLine(go.transform.position, lookAtPos, Color.grey);
			}

			// send finish event?

			if (finishEvent != null)
			{
				var targetDir = lookAtPos - go.transform.position;
				var angle = Vector3.Angle(targetDir, go.transform.forward);

				if (Mathf.Abs(angle) <= finishTolerance.Value)
				{
					Fsm.Event(finishEvent);
				}
			}
		}

	}
}