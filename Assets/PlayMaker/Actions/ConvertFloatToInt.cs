// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts a Float value to an Integer value.")]
	public class ConvertFloatToInt : FsmStateAction
	{
		public enum FloatRounding
		{
			RoundDown,
			RoundUp,
			Nearest
		}
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("The Float variable to convert to an integer.")]
		public FsmFloat floatVariable;

		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the result in an Integer variable.")]
		public FsmInt intVariable;
		
		public FloatRounding rounding;
		
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			intVariable = null;
			rounding = FloatRounding.Nearest;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertFloatToInt();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoConvertFloatToInt();
		}
		
		void DoConvertFloatToInt()
		{
			switch (rounding) 
			{
			
			case FloatRounding.Nearest:
				intVariable.Value = Mathf.RoundToInt(floatVariable.Value);
				break;
				
			case FloatRounding.RoundDown:
				intVariable.Value = Mathf.FloorToInt(floatVariable.Value);
				break;
				
			case FloatRounding.RoundUp:
				intVariable.Value = Mathf.CeilToInt(floatVariable.Value);
				break;
			}			
		}
	}
}