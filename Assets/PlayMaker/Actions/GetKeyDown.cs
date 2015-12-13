// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Sends an Event when a Key is pressed.")]
	public class GetKeyDown : FsmStateAction
	{
		[RequiredField]
		public KeyCode key;
		public FsmEvent sendEvent;
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
		
		public override void Reset()
		{
			sendEvent = null;
			key = KeyCode.None;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			bool keyDown = Input.GetKeyDown(key);
			
			if (keyDown)
				Fsm.Event(sendEvent);
			
			storeResult.Value = keyDown;
		}
	}
}