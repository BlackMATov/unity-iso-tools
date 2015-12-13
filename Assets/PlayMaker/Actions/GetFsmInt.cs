// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Get the value of an Integer Variable from another FSM.")]
	public class GetFsmInt : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;
		[RequiredField]
		[UIHint(UIHint.FsmInt)]
		public FsmString variableName;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt storeValue;
		public bool everyFrame;
		
		GameObject goLastFrame;
		PlayMakerFSM fsm;
		
		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			storeValue = null;
		}

		public override void OnEnter()
		{
			DoGetFsmInt();

			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoGetFsmInt();
		}
		
		void DoGetFsmInt()
		{
			if (storeValue == null) return;

			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

			// only get the fsm component if go has changed

			if (go != goLastFrame)
			{
				goLastFrame = go;
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			}
			
			if (fsm == null) return;
			
			FsmInt fsmInt = fsm.FsmVariables.GetFsmInt(variableName.Value);
			
			if (fsmInt == null) return;
			
			storeValue.Value = fsmInt.Value;
		}

	}
}