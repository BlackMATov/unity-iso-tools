// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts a Bool value to a Float value.")]
	public class ConvertBoolToFloat : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Bool variable to test.")]
		public FsmBool boolVariable;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Float variable to set based on the Bool variable value.")]
		public FsmFloat floatVariable;

		[Tooltip("Float value if Bool variable is false.")]
		public FsmFloat falseValue;

		[Tooltip("Float value if Bool variable is true.")]
		public FsmFloat trueValue;

		[Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			floatVariable = null;
			falseValue = 0;
			trueValue = 1;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertBoolToFloat();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoConvertBoolToFloat();
		}
		
		void DoConvertBoolToFloat()
		{
			floatVariable.Value = boolVariable.Value ? trueValue.Value : falseValue.Value;
		}
	}
}