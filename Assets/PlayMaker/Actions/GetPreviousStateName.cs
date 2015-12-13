// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Gets the name of the previously active state and stores it in a String Variable.")]
	public class GetPreviousStateName : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmString storeName;
		
		public override void Reset()
		{
			storeName = null;
		}

		public override void OnEnter()
		{
			storeName.Value = Fsm.PreviousActiveState == null ? null : Fsm.PreviousActiveState.Name;

			Finish();
		}
	}
}