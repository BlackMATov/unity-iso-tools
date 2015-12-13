// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Draws a state label for this FSM in the Game View. The label is drawn on the game object that owns the FSM. Use this to override the global setting in the PlayMaker Debug menu.")]
	public class DrawStateLabel : FsmStateAction
	{
		[RequiredField]
        [Tooltip("Set to True to show State labels, or Fals to hide them.")]
		public FsmBool showLabel;

		public override void Reset()
		{
			showLabel = true;
		}

		public override void OnEnter()
		{
			Fsm.ShowStateLabel = showLabel.Value;
			
			Finish();
		}
	}
}