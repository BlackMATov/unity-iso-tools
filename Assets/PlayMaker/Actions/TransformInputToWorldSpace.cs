// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Transforms 2d input into a 3d world space vector. E.g., can be used to transform input from a touch joystick to a movement vector.")]
	public class TransformInputToWorldSpace : FsmStateAction
	{
		public enum AxisPlane
		{
			XZ,
			XY,
			YZ
		}
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The horizontal input.")]
		public FsmFloat horizontalInput;

		[UIHint(UIHint.Variable)]
		[Tooltip("The vertical input.")]
		public FsmFloat verticalInput;
		
		[Tooltip("Input axis are reported in the range -1 to 1, this multiplier lets you set a new range.")]
		public FsmFloat multiplier;
		
		[RequiredField]
		[Tooltip("The world plane to map the 2d input onto.")]
		public AxisPlane mapToPlane;
		
		[Tooltip("Make the result relative to a GameObject, typically the main camera.")]
		public FsmGameObject relativeTo;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the direction vector.")]
		public FsmVector3 storeVector;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the length of the direction vector.")]
		public FsmFloat storeMagnitude;

		public override void Reset()
		{
			horizontalInput = null;
			verticalInput = null;
			multiplier = 1.0f;
			mapToPlane = AxisPlane.XZ;
			storeVector = null;
			storeMagnitude = null;
		}

		public override void OnUpdate()
		{
			var forward = new Vector3();
			var right = new Vector3();
			
			if (relativeTo.Value == null)
			{
				switch (mapToPlane) 
				{
				case AxisPlane.XZ:
					forward = Vector3.forward;
					right = Vector3.right;
					break;
					
				case AxisPlane.XY:
					forward = Vector3.up;
					right = Vector3.right;
					break;
					
				case AxisPlane.YZ:
					forward = Vector3.up;
					right = Vector3.forward;
					break;
				}
			}
			else
			{
				var transform = relativeTo.Value.transform;
				
				switch (mapToPlane) 
				{
				case AxisPlane.XZ:
					forward = transform.TransformDirection(Vector3.forward);
					forward.y = 0;
					forward = forward.normalized;
					right = new Vector3(forward.z, 0, -forward.x);
					break;
					
				case AxisPlane.XY:
				case AxisPlane.YZ:
					// NOTE: in relative mode XY ans YZ are the same!
					forward = Vector3.up;
					forward.z = 0;
					forward = forward.normalized;
					right = transform.TransformDirection(Vector3.right);
					break;
				}
				
				// Right vector relative to the object
				// Always orthogonal to the forward vector
				
			}
			
			// get individual axis
			// leaving an axis blank or set to None sets it to 0

			var h = horizontalInput.IsNone ? 0f : horizontalInput.Value;
			var v = verticalInput.IsNone ? 0f : verticalInput.Value;
			
			// calculate resulting direction vector

			var direction = h * right + v * forward;
			direction *= multiplier.Value;
			
			storeVector.Value = direction;
			
			if (!storeMagnitude.IsNone)
			{
				storeMagnitude.Value = direction.magnitude;
			}
		}
	}
}

