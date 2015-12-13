// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if all the given Bool Variables are True.")]
	public class BoolAllTrue : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("The Bool variables to check.")]
		public FsmBool[] boolVariables;

        [Tooltip("Event to send if all the Bool variables are True.")]
		public FsmEvent sendEvent;
		
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a Bool variable.")]
		public FsmBool storeResult;

        [Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;

		public override void Reset()
		{
			boolVariables = null;
			sendEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoAllTrue();
			
			if (!everyFrame)
			{
			    Finish();
			}		
		}
		
		public override void OnUpdate()
		{
			DoAllTrue();
		}
		
		void DoAllTrue()
		{
			if (boolVariables.Length == 0) return;
			
			var allTrue = true;
			
			for (var i = 0; i < boolVariables.Length; i++) 
			{
				if (!boolVariables[i].Value)
				{
					allTrue = false;
					break;
				}
			}

			if (allTrue)
			{
			    Fsm.Event(sendEvent);
			}
			
			storeResult.Value = allTrue;
		}
	}
}