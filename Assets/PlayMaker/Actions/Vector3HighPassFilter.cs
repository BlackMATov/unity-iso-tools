// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Use a high pass filter to isolate sudden changes in a Vector3 Variable. Useful when working with Get Device Acceleration to remove the constant effect of gravity.")]
	public class Vector3HighPassFilter : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Vector3 Variable to filter. Should generally come from some constantly updated input, e.g., acceleration.")]
		public FsmVector3 vector3Variable;
		[Tooltip("Determines how much influence new changes have.")]
		public FsmFloat filteringFactor;		
		
		Vector3 filteredVector;
		
		public override void Reset()
		{
			vector3Variable = null;
			filteringFactor = 0.1f;
		}

		public override void OnEnter()
		{
			filteredVector = new Vector3(vector3Variable.Value.x, vector3Variable.Value.y, vector3Variable.Value.z);
		}

		public override void OnUpdate()
		{
			// Subtract the low-pass value from the current value to get a simplified high-pass filter
			
			filteredVector.x = vector3Variable.Value.x - ( (vector3Variable.Value.x * filteringFactor.Value) + (filteredVector.x * (1.0f - filteringFactor.Value)) );
			filteredVector.y = vector3Variable.Value.y - ( (vector3Variable.Value.y * filteringFactor.Value) + (filteredVector.y * (1.0f - filteringFactor.Value)) );
			filteredVector.z = vector3Variable.Value.z - ( (vector3Variable.Value.z * filteringFactor.Value) + (filteredVector.z * (1.0f - filteringFactor.Value)) );

			vector3Variable.Value = new Vector3(filteredVector.x, filteredVector.y, filteredVector.z);
		}
	}
}

