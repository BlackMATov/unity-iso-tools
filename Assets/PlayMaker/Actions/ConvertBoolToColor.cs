// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts a Bool value to a Color.")]
	public class ConvertBoolToColor : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Bool variable to test.")]
		public FsmBool boolVariable;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Color variable to set based on the bool variable value.")]
		public FsmColor colorVariable;

		[Tooltip("Color if Bool variable is false.")]
		public FsmColor falseColor;
		
		[Tooltip("Color if Bool variable is true.")]
		public FsmColor trueColor;
		
		[Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			colorVariable = null;
			falseColor = Color.black;
			trueColor = Color.white;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertBoolToColor();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoConvertBoolToColor();
		}
		
		void DoConvertBoolToColor()
		{
			colorVariable.Value = boolVariable.Value ? trueColor.Value : falseColor.Value;
		}
	}
}