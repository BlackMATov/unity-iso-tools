// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Get the value of an Enum Variable from another FSM.")]
	public class GetFsmEnum : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The target FSM")]
		public FsmOwnerDefault gameObject;
		
        [UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;
		
        [RequiredField]
		[UIHint(UIHint.FsmBool)]
		public FsmString variableName;
		
        [RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmEnum storeValue;
		
        [Tooltip("Repeat every frame")]
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
			DoGetFsmEnum();
			
			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetFsmEnum();
		}

		void DoGetFsmEnum()
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
			
			var fsmEnum = fsm.FsmVariables.GetFsmEnum(variableName.Value);
            if (fsmEnum == null) return;
			
			storeValue.Value = fsmEnum.Value;
		}

	}
}