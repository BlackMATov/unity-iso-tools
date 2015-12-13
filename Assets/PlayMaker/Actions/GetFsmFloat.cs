// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Get the value of a Float Variable from another FSM.")]
	public class GetFsmFloat : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;
		[RequiredField]
		[UIHint(UIHint.FsmFloat)]
		public FsmString variableName;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeValue;
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
			DoGetFsmFloat();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoGetFsmFloat();
		}

		void DoGetFsmFloat()
		{
			if (storeValue.IsNone) return;

			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

			// only get the fsm component if go has changed
	
			if (go != goLastFrame)
			{
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
				goLastFrame = go;
			}
			
			if (fsm == null) return;
			
			FsmFloat fsmFloat = fsm.FsmVariables.GetFsmFloat(variableName.Value);
			
			if (fsmFloat == null) return;
			
			storeValue.Value = fsmFloat.Value;
		}

	}
}