// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends an Event after an optional delay. NOTE: To send events between FSMs they must be marked as Global in the Events Browser.")]
	public class SendEvent : FsmStateAction
	{
		[Tooltip("Where to send the event.")]
		public FsmEventTarget eventTarget;
		
		[RequiredField]
		[Tooltip("The event to send. NOTE: Events must be marked Global to send between FSMs.")]
		public FsmEvent sendEvent;
		
		[HasFloatSlider(0, 10)]
		[Tooltip("Optional delay in seconds.")]
		public FsmFloat delay;

		[Tooltip("Repeat every frame. Rarely needed, but can be useful when sending events to other FSMs.")]
		public bool everyFrame;

		private DelayedEvent delayedEvent;

		public override void Reset()
		{
			eventTarget = null;
			sendEvent = null;
			delay = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			if (delay.Value < 0.001f)
			{
				Fsm.Event(eventTarget, sendEvent);
			    if (!everyFrame)
			    {
			        Finish();
			    }
			}
			else
			{
				delayedEvent = Fsm.DelayedEvent(eventTarget, sendEvent, delay.Value);
			}
		}

		public override void OnUpdate()
		{
			if (!everyFrame)
			{
				if (DelayedEvent.WasSent(delayedEvent))
				{
					Finish();
				}
			}
			else
			{
                Fsm.Event(eventTarget, sendEvent);
			}
		}
	}
}