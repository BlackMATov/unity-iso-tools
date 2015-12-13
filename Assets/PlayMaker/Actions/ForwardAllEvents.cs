// (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.StateMachine)]
    [Tooltip("Forwards all event recieved by this FSM to another target. Optionally specify a list of events to ignore.")]
    public class ForwardAllEvents : FsmStateAction
    {
        [Tooltip("Forward to this target.")]
        public FsmEventTarget forwardTo;

        [Tooltip("Don't forward these events.")]
        public FsmEvent[] exceptThese;

        [Tooltip("Should this action eat the events or pass them on.")]
        public bool eatEvents;

        public override void Reset()
        {
            forwardTo = new FsmEventTarget {target = FsmEventTarget.EventTarget.FSMComponent};
            exceptThese = new[] {FsmEvent.Finished};
            eatEvents = true;
        }

        /// <summary>
        /// Return true to eat the event
        /// </summary>
        public override bool Event(FsmEvent fsmEvent)
        {
            if (exceptThese != null)
            {
                foreach (var e in exceptThese)
                {
                    if (e == fsmEvent)
                    {
                        return false;
                    }
                }
            }

            Fsm.Event(forwardTo, fsmEvent);

            return eatEvents;
        }
    }
}
