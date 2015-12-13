// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts a Bool value to an Integer value.")]
	public class ConvertBoolToInt : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Bool variable to test.")]
		public FsmBool boolVariable;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Integer variable to set based on the Bool variable value.")]
		public FsmInt intVariable;

		[Tooltip("Integer value if Bool variable is false.")]
		public FsmInt falseValue;

		[Tooltip("Integer value if Bool variable is false.")]
		public FsmInt trueValue;

		[Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			intVariable = null;
			falseValue = 0;
			trueValue = 1;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertBoolToInt();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoConvertBoolToInt();
		}
		
		void DoConvertBoolToInt()
		{
			intVariable.Value = boolVariable.Value ? trueValue.Value : falseValue.Value;
		}
	}
}