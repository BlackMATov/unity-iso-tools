// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a Game Object has a tag.")]
	public class GameObjectCompareTag : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject to test.")]
		public FsmGameObject gameObject;

		[RequiredField]
		[UIHint(UIHint.Tag)]
        [Tooltip("The Tag to check for.")]
		public FsmString tag;

        [Tooltip("Event to send if the GameObject has the Tag.")]
		public FsmEvent trueEvent;

        [Tooltip("Event to send if the GameObject does not have the Tag.")]
		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a Bool variable.")]
        public FsmBool storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			tag = "Untagged";
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoCompareTag();
				
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoCompareTag();
		}

		void DoCompareTag()
		{
			var hasTag = false;

			if (gameObject.Value != null)
			{
				hasTag = gameObject.Value.CompareTag(tag.Value);
			}

			storeResult.Value = hasTag;

			Fsm.Event(hasTag ? trueEvent : falseEvent);
		}
	}
}