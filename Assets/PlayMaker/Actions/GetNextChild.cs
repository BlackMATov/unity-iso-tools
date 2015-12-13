// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Each time this action is called it gets the next child of a GameObject. This lets you quickly loop through all the children of an object to perform actions on them. NOTE: To find a specific child use Find Child.")]
	public class GetNextChild : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The parent GameObject. Note, if GameObject changes, this action will reset and start again at the first child.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the next child in a GameObject variable.")]
		public FsmGameObject storeNextChild;

		[Tooltip("Event to send to get the next child.")]
		public FsmEvent loopEvent;

		[Tooltip("Event to send when there are no more children.")]
		public FsmEvent finishedEvent;

		public override void Reset()
		{
			gameObject = null;
			storeNextChild = null;
			loopEvent = null;
			finishedEvent = null;
		}

		// cache the gameObject so we no if it changes
		private GameObject go;

		// increment a child index as we loop through children
		private int nextChildIndex;

		public override void OnEnter()
		{

			DoGetNextChild(Fsm.GetOwnerDefaultTarget(gameObject));

			Finish();
		}

		void DoGetNextChild(GameObject parent)
		{
			if (parent == null)
			{
				return;
			}

			// reset?

			if (go != parent)
			{
				go = parent;
				nextChildIndex = 0;
			}

			// no more children?
			// check first to avoid errors.

			if (nextChildIndex >= go.transform.childCount)
			{
				nextChildIndex = 0;
				Fsm.Event(finishedEvent);
				return;
			}

			// get next child

			storeNextChild.Value = parent.transform.GetChild(nextChildIndex).gameObject;


			// no more children?
			// check a second time to avoid process lock and possible infinite loop if the action is called again.
			// Practically, this enabled calling again this state and it will start again iterating from the first child.

			if (nextChildIndex >= go.transform.childCount)
			{
				nextChildIndex = 0;
				Fsm.Event(finishedEvent);
				return;
			}

			// iterate the next child
			nextChildIndex++;

			if (loopEvent != null)
			{
				Fsm.Event(loopEvent);
			}
		}
	}
}