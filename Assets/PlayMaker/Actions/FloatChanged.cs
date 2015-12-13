// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if the value of a Float variable changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
	public class FloatChanged : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("The Float variable to watch for a change.")]
		public FsmFloat floatVariable;

        [Tooltip("Event to send if the float variable changes.")]
		public FsmEvent changedEvent;
		
        [UIHint(UIHint.Variable)]
        [Tooltip("Set to True if the float variable changes.")]
		public FsmBool storeResult;
		
		float previousValue;

		public override void Reset()
		{
			floatVariable = null;
			changedEvent = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			if (floatVariable.IsNone)
			{
				Finish();
				return;
			}
			
			previousValue = floatVariable.Value;
		}
		
		public override void OnUpdate()
		{
			storeResult.Value = false;
			
			if (floatVariable.Value != previousValue)
			{
				previousValue = floatVariable.Value;
				storeResult.Value = true;
				Fsm.Event(changedEvent);
			}
		}
	}
}

