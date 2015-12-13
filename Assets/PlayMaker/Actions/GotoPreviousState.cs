// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Immediately return to the previously active state.")]
	public class GotoPreviousState : FsmStateAction
	{
		public override void Reset()
		{
		}

		public override void OnEnter()
		{
			if (Fsm.PreviousActiveState != null)
			{
                Log("Goto Previous State: " + Fsm.PreviousActiveState.Name);

				Fsm.GotoPreviousState();
			}
			
			Finish();
		}
	}
}