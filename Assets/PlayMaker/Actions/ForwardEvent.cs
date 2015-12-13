// (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.StateMachine)]
    [Tooltip("Forward an event recieved by this FSM to another target.")]
    public class ForwardEvent : FsmStateAction
    {
        [Tooltip("Forward to this target.")]
        public FsmEventTarget forwardTo;

        [Tooltip("The events to forward.")]
        public FsmEvent[] eventsToForward;

        [Tooltip("Should this action eat the events or pass them on.")]
        public bool eatEvents;

        public override void Reset()
        {
            forwardTo = new FsmEventTarget { target = FsmEventTarget.EventTarget.FSMComponent };
            eventsToForward = null;
            eatEvents = true;
        }

        /// <summary>
        /// Return true to eat the event
        /// </summary>
        public override bool Event(FsmEvent fsmEvent)
        {
            if (eventsToForward != null)
            {
                foreach (var e in eventsToForward)
                {
                    if (e == fsmEvent)
                    {
                        Fsm.Event(forwardTo, fsmEvent);

                        return eatEvents;
                    }
                }
            }

            return false;
        }
    }
}
