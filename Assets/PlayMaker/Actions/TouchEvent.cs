// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Sends events based on Touch Phases. Optionally filter by a fingerID.")]
	public class TouchEvent : FsmStateAction
	{
		public FsmInt fingerId;
		public TouchPhase touchPhase;
		public FsmEvent sendEvent;
		[UIHint(UIHint.Variable)]
		public FsmInt storeFingerId;
		
		public override void Reset()
		{
			fingerId = new FsmInt { UseVariable = true } ;
			storeFingerId = null;
		}

		public override void OnUpdate()
		{
			if (Input.touchCount > 0)
			{
				foreach (var touch in Input.touches)
				{

					if (fingerId.IsNone || touch.fingerId == fingerId.Value)
					{
						if (touch.phase == touchPhase)
						{
							storeFingerId.Value = touch.fingerId;
							Fsm.Event(sendEvent);
						}
					}
				}
			}
		}
	}
}