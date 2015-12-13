// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends Events based on the comparison of 2 Integers.")]
	public class IntCompare : FsmStateAction
	{
		[RequiredField]
		public FsmInt integer1;
		[RequiredField]
		public FsmInt integer2;
		[Tooltip("Event sent if Int 1 equals Int 2")]
		public FsmEvent equal;
		[Tooltip("Event sent if Int 1 is less than Int 2")]
		public FsmEvent lessThan;
		[Tooltip("Event sent if Int 1 is greater than Int 2")]
		public FsmEvent greaterThan;
		public bool everyFrame;

		public override void Reset()
		{
			integer1 = 0;
			integer2 = 0;
			equal = null;
			lessThan = null;
			greaterThan = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIntCompare();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoIntCompare();
		}		

		void DoIntCompare()
		{
			if (integer1.Value == integer2.Value)
			{
				Fsm.Event(equal);
				return;
			}

			if (integer1.Value < integer2.Value)
			{
				Fsm.Event(lessThan);
				return;
			}

			if (integer1.Value > integer2.Value)
			{
				Fsm.Event(greaterThan);
			}

		}

		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(equal) &&
				FsmEvent.IsNullOrEmpty(lessThan) &&
				FsmEvent.IsNullOrEmpty(greaterThan))
				return "Action sends no events!";
			return "";
		}
	}
}