// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts an String value to an Int value.")]
	public class ConvertStringToInt : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("The String variable to convert to an integer.")]
		public FsmString stringVariable;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the result in an Int variable.")]
		public FsmInt intVariable;

        [Tooltip("Repeat every frame. Useful if the String variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			intVariable = null;
			stringVariable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertStringToInt();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoConvertStringToInt();
		}
		
		void DoConvertStringToInt()
		{
			intVariable.Value = int.Parse(stringVariable.Value);
		}
	}
}