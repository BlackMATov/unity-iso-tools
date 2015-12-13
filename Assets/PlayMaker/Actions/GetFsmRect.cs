// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Get the value of a Rect Variable from another FSM.")]
	public class GetFsmRect : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmRect)]
		public FsmString variableName;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmRect storeValue;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		GameObject goLastFrame;
		protected PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			variableName = "";
			storeValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetFsmVariable();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetFsmVariable();
		}

		void DoGetFsmVariable()
		{
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

			if (fsm == null || storeValue == null)
			{
				return;
			}

			var fsmVar = fsm.FsmVariables.GetFsmRect(variableName.Value);

			if (fsmVar != null)
			{
				storeValue.Value = fsmVar.Value;
			}
		}

	}
}