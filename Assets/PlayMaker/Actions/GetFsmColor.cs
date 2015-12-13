// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Get the value of a Color Variable from another FSM.")]
	public class GetFsmColor : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;
		[RequiredField]
		[UIHint(UIHint.FsmColor)]
		public FsmString variableName;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmColor storeValue;
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
			DoGetFsmColor();

			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoGetFsmColor();
		}
		
		void DoGetFsmColor()
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
			
			FsmColor fsmColor = fsm.FsmVariables.GetFsmColor(variableName.Value);
			
			if (fsmColor == null) return;
			
			storeValue.Value = fsmColor.Value;
		}

	}
}