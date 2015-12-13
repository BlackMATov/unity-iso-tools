// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends an Event based on a Game Object's Tag.")]
	public class GameObjectTagSwitch : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("The GameObject to test.")]
		public FsmGameObject gameObject;

		[CompoundArray("Tag Switches", "Compare Tag", "Send Event")]
		[UIHint(UIHint.Tag)]
		public FsmString[] compareTo;
		public FsmEvent[] sendEvent;
		
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			compareTo = new FsmString[1];
			sendEvent = new FsmEvent[1];
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoTagSwitch();
			
			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			DoTagSwitch();
		}
		
		void DoTagSwitch()
		{
			var go = gameObject.Value;
			if (go == null)
			{
			    return;
			}
			
			for (var i = 0; i < compareTo.Length; i++) 
			{
				if (go.tag == compareTo[i].Value)
				{
					Fsm.Event(sendEvent[i]);
					return;
				}
			}
		}
	}
}