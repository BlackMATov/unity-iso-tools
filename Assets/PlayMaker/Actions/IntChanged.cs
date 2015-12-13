// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if the value of an integer variable changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
	public class IntChanged : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;
		public FsmEvent changedEvent;
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
		
		int previousValue;

		public override void Reset()
		{
			intVariable = null;
			changedEvent = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			if (intVariable.IsNone)
			{
				Finish();
				return;
			}
			
			previousValue = intVariable.Value;
		}
		
		public override void OnUpdate()
		{
			storeResult.Value = false;
			
			if (intVariable.Value != previousValue)
			{
				previousValue = intVariable.Value;
				storeResult.Value = true;
				Fsm.Event(changedEvent);
			}
		}
	}
}

