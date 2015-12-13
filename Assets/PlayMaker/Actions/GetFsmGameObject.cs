// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Get the value of a Game Object Variable from another FSM.")]
	public class GetFsmGameObject : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;
		[RequiredField]
		[UIHint(UIHint.FsmGameObject)]
		public FsmString variableName;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeValue;
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
			DoGetFsmGameObject();

			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoGetFsmGameObject();
		}

		void DoGetFsmGameObject()
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
			
			FsmGameObject fsmGameObject = fsm.FsmVariables.GetFsmGameObject(variableName.Value);
			
			if (fsmGameObject == null) return;
			
			storeValue.Value = fsmGameObject.Value;
		}



	}
}