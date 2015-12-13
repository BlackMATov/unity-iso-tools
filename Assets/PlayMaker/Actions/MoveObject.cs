// Thanks to A3DStudio

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
    [HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=4758.0")]
	[Tooltip("Move a GameObject to another GameObject. Works like iTween Move To, but with better performance.")]
	public class MoveObject: EaseFsmAction
	{
		[RequiredField]
		public FsmOwnerDefault objectToMove;
		[RequiredField]
		public FsmGameObject destination;
		
        private FsmVector3 fromValue;
		private FsmVector3 toVector;
		private FsmVector3 fromVector;
		
		private bool finishInNextStep;
		
		public override void Reset ()
        {
			fromValue = null;
			toVector = null;
			finishInNextStep = false;
			fromVector = null;
		}
		                   
		
		public override void OnEnter ()
		{
			var go = Fsm.GetOwnerDefaultTarget(objectToMove);
			fromVector = go.transform.position;
			toVector = destination.Value.transform.position;
			
			fromFloats = new float[3];
			fromFloats[0] = fromVector.Value.x;
			fromFloats[1] = fromVector.Value.y;
			fromFloats[2] = fromVector.Value.z;
			
			toFloats = new float[3];
			toFloats[0] = toVector.Value.x;
			toFloats[1] = toVector.Value.y;
			toFloats[2] = toVector.Value.z;
			resultFloats = new float[3];
			
			resultFloats[0] = fromVector.Value.x;
			resultFloats[1] = fromVector.Value.y;
			resultFloats[2] = fromVector.Value.z;
			
			finishInNextStep = false;
		}
			
		public override void OnUpdate()
        {
			var go = Fsm.GetOwnerDefaultTarget(objectToMove);
			go.transform.position = new Vector3(resultFloats[0],resultFloats[1],resultFloats[2]);
			
			if(finishInNextStep){
				Finish();
				if(finishEvent != null)	Fsm.Event(finishEvent);
			}
			
			if(finishAction && !finishInNextStep)
            {
				go.transform.position = new Vector3(reverse.IsNone ? toVector.Value.x : reverse.Value ? fromValue.Value.x : toVector.Value.x, 
				reverse.IsNone ? toVector.Value.y : reverse.Value ? fromValue.Value.y : toVector.Value.y,
				reverse.IsNone ? toVector.Value.z : reverse.Value ? fromValue.Value.z : toVector.Value.z
				);
				finishInNextStep = true;
			}
		}
	}
}