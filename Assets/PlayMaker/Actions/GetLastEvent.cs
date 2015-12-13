// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Gets the event that caused the transition to the current state, and stores it in a String Variable.")]
	public class GetLastEvent : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmString storeEvent;

		public override void Reset()
		{
			storeEvent = null;
		}

		public override void OnEnter()
		{
			storeEvent.Value = Fsm.LastTransition == null ? "START" : Fsm.LastTransition.EventName;

			Finish();
		}
	}
}