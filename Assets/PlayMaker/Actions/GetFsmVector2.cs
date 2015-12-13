// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Get the value of a Vector2 Variable from another FSM.")]
	public class GetFsmVector2 : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;
		[RequiredField]
		[UIHint(UIHint.FsmVector2)]
		public FsmString variableName;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector2 storeValue;
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
			DoGetFsmVector2();

			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoGetFsmVector2();
		}
		
		void DoGetFsmVector2()
		{
			if (storeValue == null)
			{
			    return;
			}

			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			
            if (go == null)
			{
			    return;
			}

			// only get the fsm component if go has changed

			if (go != goLastFrame)
			{
				goLastFrame = go;
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			}
			
			if (fsm == null)
			{
			    return;
			}
			
			var fsmVector2 = fsm.FsmVariables.GetFsmVector2(variableName.Value);
			
			if (fsmVector2 == null)
			{
			    return;
			}
			
			storeValue.Value = fsmVector2.Value;
		}

	}
}