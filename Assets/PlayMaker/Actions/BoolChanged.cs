// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if the value of a Bool Variable has changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
	public class BoolChanged : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("The Bool variable to watch for changes.")]
		public FsmBool boolVariable;

        [Tooltip("Event to send if the variable changes.")]
		public FsmEvent changedEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Set to True if changed.")]
        public FsmBool storeResult;

		bool previousValue;

		public override void Reset()
		{
			boolVariable = null;
			changedEvent = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			if (boolVariable.IsNone)
			{
				Finish();
				return;
			}
			
			previousValue = boolVariable.Value;
		}
		
		public override void OnUpdate()
		{
			storeResult.Value = false;
			
			if (boolVariable.Value != previousValue)
			{
				storeResult.Value = true;
				Fsm.Event(changedEvent);
			}
		}
	}
}

