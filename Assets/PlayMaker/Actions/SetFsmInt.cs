// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of an Integer Variable in another FSM.")]
	public class SetFsmInt : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmInt)]
        [Tooltip("The name of the FSM variable.")]
		public FsmString variableName;

		[RequiredField]
        [Tooltip("Set the value of the variable.")]
		public FsmInt setValue;

        [Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;

		GameObject goLastFrame;
		string fsmNameLastFrame;

		PlayMakerFSM fsm;
		
		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			setValue = null;
		}

		public override void OnEnter()
		{
			DoSetFsmInt();
			
			if (!everyFrame)
			{
			    Finish();
			}		
		}

		void DoSetFsmInt()
		{
			if (setValue == null)
			{
			    return;
			}

			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			
			// FIX: must check as well that the fsm name is different.
			if (go != goLastFrame || fsmName.Value != fsmNameLastFrame)
			{
				goLastFrame = go;
				fsmNameLastFrame = fsmName.Value;
				// only get the fsm component if go or fsm name has changed
				
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			}	
			
			if (fsm == null)
			{
                LogWarning("Could not find FSM: " + fsmName.Value);
			    return;
			}
			
			var fsmInt = fsm.FsmVariables.GetFsmInt(variableName.Value);
			
			if (fsmInt != null)
		    {
		        fsmInt.Value = setValue.Value;
		    }
            else
            {
                LogWarning("Could not find variable: " + variableName.Value);
            }
		}

		public override void OnUpdate()
		{
			DoSetFsmInt();
		}

	}
}