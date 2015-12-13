// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.
// JeanFabre: This version allow setting the variable to null. 

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a Game Object Variable in another All FSM. Accept null reference")]
	public class SetAllFsmGameObject : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		//[UIHint(UIHint.FsmName)]
		//[Tooltip("Optional name of FSM on Game Object")]
		//public FsmString fsmName;
		//[RequiredField]
		//[UIHint(UIHint.FsmGameObject)]
		//public FsmString variableName;
		//public FsmGameObject setValue;
		public bool everyFrame;

		//GameObject goLastFrame;
		//PlayMakerFSM[] fsms;
		
		public override void Reset()
		{
			//gameObject = null;
			//fsmName = "";
			//setValue = null;
			
			//goLastFrame = null ;
			//fsms = null ;
		}

		public override void OnEnter()
		{
			//fsms = null ;
			
			//DoSetFsmGameObject();
			
			if (!everyFrame)
				Finish();		
		}

		void DoSetFsmGameObject()
		{
			//GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			//if (go == null) return;
			
			//if (go != goLastFrame)
			//{
			//	goLastFrame = go;
				
				//fsms = go.GetComponents<PlayMakerFSM>() ;
				// only get the fsm component if go has changed
				
				//fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			//}			
			
			//if (fsms == null) return;
			
			//for( int i = 0 ; i < fsms.Length ; i ++ )
			//{
				/*FsmGameObject fsmGameObject = fsms[i].FsmVariables.GetFsmGameObject(variableName.Value);
				
				if (fsmGameObject == null) continue;
				
				if (setValue == null) 
				{
					fsmGameObject.Value = null;
				}
				else
				{
					fsmGameObject.Value = setValue.Value;
				}*/
			//}
		}

		//public override void OnUpdate()
		//{
			//DoSetFsmGameObject();
		//}

	}
}