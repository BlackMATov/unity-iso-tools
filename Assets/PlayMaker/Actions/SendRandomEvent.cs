// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends a Random Event picked from an array of Events. Optionally set the relative weight of each event.")]
	public class SendRandomEvent : FsmStateAction
	{
		[CompoundArray("Events", "Event", "Weight")]
		public FsmEvent[] events;
		[HasFloatSlider(0, 1)]
		public FsmFloat[] weights;
		public FsmFloat delay;

		DelayedEvent delayedEvent;
		
		public override void Reset()
		{
			events = new FsmEvent[3];
			weights = new FsmFloat[] {1,1,1};
			delay = null;
		}

		public override void OnEnter()
		{
			if (events.Length > 0)
			{
				int randomIndex = ActionHelpers.GetRandomWeightedIndex(weights);
			
				if (randomIndex != -1)
				{
					if (delay.Value < 0.001f)
					{
						Fsm.Event(events[randomIndex]);
						Finish();
					}
					else
					{
						delayedEvent = Fsm.DelayedEvent(events[randomIndex], delay.Value);
					}
					
					return;
				}
			}						
			
			Finish();
		}
		
		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(delayedEvent))
			{
				Finish();
			}
		}
	}
}