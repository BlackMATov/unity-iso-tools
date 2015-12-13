// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Set an item in an Array Variable in another FSM.")]
	public class FsmArraySet : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object.")]
		public FsmString fsmName;
		
		[RequiredField]
		[Tooltip("The name of the FSM variable.")]
		public FsmString variableName;
		
		[Tooltip("Set the value of the variable.")]
		public FsmString setValue;
		
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;
		
		GameObject goLastFrame;
		PlayMakerFSM fsm;
		
		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			setValue = null;
		}
		
		public override void OnEnter()
		{
			DoSetFsmString();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}
		
		void DoSetFsmString()
		{
			if (setValue == null)
			{
				return;
			}
			
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}
			
			if (go != goLastFrame)
			{
				goLastFrame = go;
				
				// only get the fsm component if go has changed
				
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			}			
			
			if (fsm == null)
			{
				LogWarning("Could not find FSM: " + fsmName.Value);
				return;
			}
			
			var fsmString = fsm.FsmVariables.GetFsmString(variableName.Value);
			
			if (fsmString != null)
			{
				fsmString.Value = setValue.Value;
			}
			else
			{
				LogWarning("Could not find variable: " + variableName.Value);
			}			
		}
		
		public override void OnUpdate()
		{
			DoSetFsmString();
		}
		
	}
}