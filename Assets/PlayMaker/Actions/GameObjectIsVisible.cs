// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a Game Object is visible.")]
	public class GameObjectIsVisible : ComponentAction<Renderer>
	{
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
        [Tooltip("The GameObject to test.")]
		public FsmOwnerDefault gameObject;
		
        [Tooltip("Event to send if the GameObject is visible.")]
		public FsmEvent trueEvent;
		
        [Tooltip("Event to send if the GameObject is NOT visible.")]
		public FsmEvent falseEvent;
		
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a bool variable.")]
		public FsmBool storeResult;
		
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoIsVisible();
			
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsVisible();
		}

		void DoIsVisible()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);			
			if (UpdateCache(go))
			{
                var isVisible = renderer.isVisible;
                storeResult.Value = isVisible;
                Fsm.Event(isVisible ? trueEvent : falseEvent);
			}
		}
	}
}

