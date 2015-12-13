// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a GameObject has children.")]
	public class GameObjectHasChildren : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject to test.")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Event to send if the GameObject has children.")]
		public FsmEvent trueEvent;
		
		[Tooltip("Event to send if the GameObject does not have children.")]
		public FsmEvent falseEvent;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a bool variable.")]
		public FsmBool storeResult;
		
		[Tooltip("Repeat every frame.")]
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
			DoHasChildren();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoHasChildren();
		}

		void DoHasChildren()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			var hasChildren = go.transform.childCount > 0;
			
			storeResult.Value = hasChildren;

			Fsm.Event(hasChildren ? trueEvent : falseEvent);
		}
	}
}