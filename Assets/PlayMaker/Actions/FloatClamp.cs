// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Clamps the value of Float Variable to a Min/Max range.")]
	public class FloatClamp : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Float variable to clamp.")]
		public FsmFloat floatVariable;

		[RequiredField]
        [Tooltip("The minimum value.")]
		public FsmFloat minValue;

		[RequiredField]
        [Tooltip("The maximum value.")]
		public FsmFloat maxValue;

        [Tooltip("Repeate every frame. Useful if the float variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			minValue = null;
			maxValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoClamp();
			
			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			DoClamp();
		}
		
		void DoClamp()
		{
			floatVariable.Value = Mathf.Clamp(floatVariable.Value, minValue.Value, maxValue.Value);
		}
	}
}