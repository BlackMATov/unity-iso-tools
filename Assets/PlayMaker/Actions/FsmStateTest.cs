// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if an FSM is in the specified State.")]
	public class FsmStateTest : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject that owns the FSM.")]
		public FsmGameObject gameObject;

		[UIHint(UIHint.FsmName)]
        [Tooltip("Optional name of Fsm on Game Object. Useful if there is more than one FSM on the GameObject.")]
		public FsmString fsmName;
		
        [RequiredField]
        [Tooltip("Check to see if the FSM is in this state.")]
        public FsmString stateName;

        [Tooltip("Event to send if the FSM is in the specified state.")]
		public FsmEvent trueEvent;

        [Tooltip("Event to send if the FSM is NOT in the specified state.")]
		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
        [Tooltip("Store the result of this test in a bool variable. Useful if other actions depend on this test.")]
		public FsmBool storeResult;

        [Tooltip("Repeat every frame. Useful if you're waiting for a particular state.")]
		public bool everyFrame;
		
		// store game object last frame so we know when it's changed
		// and have to cache a new fsm
		private GameObject previousGo;
		
		// cach the fsm component since that's an expensive operation
        private PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = null;
			stateName = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFsmStateTest();
			
			if (!everyFrame)
			{
			    Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoFsmStateTest();
		}	
		
		void DoFsmStateTest()
		{
			var go = gameObject.Value;
			if (go == null) return;
			
			if (go != previousGo)
			{
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
				previousGo = go;
			}
			
			if (fsm == null)
			{
			    return;
			}
			
			var isState = false;
			
			if (fsm.ActiveStateName == stateName.Value)
			{
				Fsm.Event(trueEvent);
				isState = true;
			}
			else 
			{
				Fsm.Event(falseEvent);
			}

			storeResult.Value = isState;
		}


	}
}