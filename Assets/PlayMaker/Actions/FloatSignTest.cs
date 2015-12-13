// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends Events based on the sign of a Float.")]
	public class FloatSignTest : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("The float variable to test.")]
		public FsmFloat floatValue;

        [Tooltip("Event to send if the float variable is positive.")]
		public FsmEvent isPositive;

        [Tooltip("Event to send if the float variable is negative.")]
		public FsmEvent isNegative;

        [Tooltip("Repeat every frame. Useful if the variable is changing and you're waiting for a particular result.")]
		public bool everyFrame;

		public override void Reset()
		{
			floatValue = 0f;
			isPositive = null;
			isNegative = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSignTest();
			
			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSignTest();
		}

		void DoSignTest()
		{
			if (floatValue == null)
			{
			    return;
			}
			
			Fsm.Event(floatValue.Value < 0 ? isNegative : isPositive);
		}

		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(isPositive) &&
				FsmEvent.IsNullOrEmpty(isNegative))
				return "Action sends no events!";
			return "";
		}
	}
}