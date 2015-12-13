// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
// JeanFabre: This version allow setting the variable to null. 

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a Game Object Variable in another FSM. Accept null reference")]
	public class SetFsmGameObject : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;
		
		[RequiredField]
		[UIHint(UIHint.FsmGameObject)]
        [Tooltip("The name of the FSM variable.")]
		public FsmString variableName;

        [Tooltip("Set the value of the variable.")]
		public FsmGameObject setValue;

        [Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;

		private GameObject goLastFrame;
		string fsmNameLastFrame;

		private PlayMakerFSM fsm;
		
		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			setValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFsmGameObject();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}

		void DoSetFsmGameObject()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}
			
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
				return;
			}
			
			var fsmGameObject = fsm.FsmVariables.FindFsmGameObject(variableName.Value);
			
			if (fsmGameObject != null)
			{
				fsmGameObject.Value = setValue == null ? null : setValue.Value;
			}
            else
            {
                LogWarning("Could not find variable: " + variableName.Value);
            }
		}

		public override void OnUpdate()
		{
			DoSetFsmGameObject();
		}

	}
}