// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts an Integer value to a Float value.")]
	public class ConvertIntToFloat : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("The Integer variable to convert to a float.")]
		public FsmInt intVariable;

		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a Float variable.")]
		public FsmFloat floatVariable;

        [Tooltip("Repeat every frame. Useful if the Integer variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			intVariable = null;
			floatVariable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertIntToFloat();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoConvertIntToFloat();
		}
		
		void DoConvertIntToFloat()
		{
			floatVariable.Value = intVariable.Value;
		}
	}
}