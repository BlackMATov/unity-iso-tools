// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Action version of Unity's Smooth Follow script.")]
	public class SmoothFollowAction : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The game object to control. E.g. The camera.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The GameObject to follow.")]
		public FsmGameObject targetObject;

		[RequiredField]
		[Tooltip("The distance in the x-z plane to the target.")]
		public FsmFloat distance;

		[RequiredField]
		[Tooltip("The height we want the camera to be above the target")]
		public FsmFloat height;

		[RequiredField]
		[Tooltip("How much to dampen height movement.")]
		public FsmFloat heightDamping;

		[RequiredField]
		[Tooltip("How much to dampen rotation changes.")]
		public FsmFloat rotationDamping;

		// Cache for performance
		private GameObject cachedObject;
		private Transform myTransform;
		
		private GameObject cachedTarget;
		private Transform targetTransform;

		public override void Reset()
		{
			gameObject = null;
			targetObject = null;
			distance = 10f;
			height = 5f;
			heightDamping = 2f;
			rotationDamping = 3f;
		}

		public override void OnLateUpdate()
		{
			if (targetObject.Value == null)
			{
				return;
			}

			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}
			
			if (cachedObject != go)
			{
				cachedObject = go;
				myTransform = go.transform;
			}
			
			if (cachedTarget != targetObject.Value)
			{
				cachedTarget = targetObject.Value;
				targetTransform = cachedTarget.transform;
			}
			
			
			// Calculate the current rotation angles
			var wantedRotationAngle = targetTransform.eulerAngles.y;
			var wantedHeight = targetTransform.position.y + height.Value;

			var currentRotationAngle = myTransform.eulerAngles.y;
			var currentHeight = myTransform.position.y;

			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping.Value * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping.Value * Time.deltaTime);

			// Convert the angle into a rotation
			var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			myTransform.position = targetTransform.position;
			myTransform.position -= currentRotation * Vector3.forward * distance.Value;

			// Set the height of the camera
			myTransform.position = new Vector3(myTransform.position.x, currentHeight, myTransform.position.z);

			// Always look at the target
			myTransform.LookAt(targetTransform);
		}

	}
}