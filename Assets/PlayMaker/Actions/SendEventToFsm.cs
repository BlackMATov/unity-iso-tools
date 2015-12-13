// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Obsolete("This action is obsolete; use Send Event with Event Target instead.")]
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends an Event to another Fsm after an optional delay. Specify an Fsm Name or use the first Fsm on the object.")]
	public class SendEventToFsm : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of Fsm on Game Object")]
		public FsmString fsmName;
		[RequiredField]
		[UIHint(UIHint.FsmEvent)]
		public FsmString sendEvent;
		[HasFloatSlider(0, 10)]
		public FsmFloat delay;
		bool requireReceiver;

		private GameObject go;
		private DelayedEvent delayedEvent;

		public override void Reset()
		{
			gameObject = null;
			fsmName = null;
			sendEvent = null;
			delay = null;
			requireReceiver = false;
		}

		public override void OnEnter()
		{
			go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go == null)
			{
				Finish();
				return;
			}
			
			var sendToFsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			
			if (sendToFsm == null)
			{
				if (requireReceiver)
				{
					LogError("GameObject doesn't have FsmComponent: " + go.name + " " + fsmName.Value);
				}

				return;
			}

			if (delay.Value < 0.001)
			{
				sendToFsm.Fsm.Event(sendEvent.Value);
				Finish();
			}
			else
			{
				delayedEvent = sendToFsm.Fsm.DelayedEvent(FsmEvent.GetFsmEvent(sendEvent.Value), delay.Value);
			}
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