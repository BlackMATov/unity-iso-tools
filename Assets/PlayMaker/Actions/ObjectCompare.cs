// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Compare 2 Object Variables and send events based on the result.")]
	public class ObjectCompare : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmObject objectVariable;
		
		[RequiredField]
		public FsmObject compareTo;

		//[ActionSection("")]

		[Tooltip("Event to send if the 2 object values are equal.")]
		public FsmEvent equalEvent;
		
		[Tooltip("Event to send if the 2 object values are not equal.")]
		public FsmEvent notEqualEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a variable.")]
		public FsmBool storeResult;

		//[ActionSection("")]

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			objectVariable = null;
			compareTo = null;
			storeResult = null;
			equalEvent = null;
			notEqualEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoObjectCompare();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoObjectCompare();
		}

		void DoObjectCompare()
		{
			var result = objectVariable.Value == compareTo.Value;

			storeResult.Value = result;

			Fsm.Event(result ? equalEvent : notEqualEvent);
		}
	}
}