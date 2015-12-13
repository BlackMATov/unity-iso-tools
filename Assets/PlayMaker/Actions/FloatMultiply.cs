// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Multiplies one Float by another.")]
	public class FloatMultiply : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("The float variable to multiply.")]
		public FsmFloat floatVariable;

		[RequiredField]
        [Tooltip("Multiply the float variable by this value.")]
		public FsmFloat multiplyBy;

        [Tooltip("Repeat every frame. Useful if the variables are changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			multiplyBy = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			floatVariable.Value *= multiplyBy.Value;
			
			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			floatVariable.Value *= multiplyBy.Value;
		}
	}
}