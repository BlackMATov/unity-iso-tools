// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Convert)]
    [Tooltip("Converts a Float value to a String value with optional format.")]
	public class ConvertFloatToString : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The float variable to convert.")]
		public FsmFloat floatVariable;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("A string variable to store the converted value.")]
		public FsmString stringVariable;
        
		[Tooltip("Optional Format, allows for leading zeroes. E.g., 0000")]
        public FsmString format;

		[Tooltip("Repeat every frame. Useful if the float variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			stringVariable = null;
			everyFrame = false;
            format = null;
		}

		public override void OnEnter()
		{
			DoConvertFloatToString();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoConvertFloatToString();
		}
		
		void DoConvertFloatToString()
		{
			if (format.IsNone || string.IsNullOrEmpty(format.Value))
			{
            	stringVariable.Value = floatVariable.Value.ToString();
            }
            else
            {
            	stringVariable.Value = floatVariable.Value.ToString(format.Value);
            }
		}
	}
}