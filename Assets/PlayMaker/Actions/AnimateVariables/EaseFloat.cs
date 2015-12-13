// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Easing Animation - Float")]
	public class EaseFloat : EaseFsmAction
	{
		[RequiredField]
		public FsmFloat fromValue;
		[RequiredField]
		public FsmFloat toValue;
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;
		
		private bool finishInNextStep = false;
		
		public override void Reset (){
			base.Reset();
			floatVariable = null;
			fromValue = null;
			toValue = null;
			finishInNextStep = false;
		}
		                   
		
		public override void OnEnter ()
		{
			base.OnEnter();
			fromFloats = new float[1];
			fromFloats[0] = fromValue.Value;
			toFloats = new float[1];
			toFloats[0] = toValue.Value;
			resultFloats = new float[1];
			finishInNextStep = false;
            floatVariable.Value = fromValue.Value;
		}
		
		public override void OnExit (){
			base.OnExit();
		}
			
		public override void OnUpdate(){
			base.OnUpdate();
			if(!floatVariable.IsNone && isRunning){
				floatVariable.Value = resultFloats[0];
			}
			
			if(finishInNextStep){
				Finish();
				if(finishEvent != null)	Fsm.Event(finishEvent);
			}
			
			if(finishAction && !finishInNextStep){
				if(!floatVariable.IsNone){
					floatVariable.Value = reverse.IsNone ? toValue.Value : reverse.Value ? fromValue.Value : toValue.Value; 
				}
				finishInNextStep = true;
			}
		}
	}
}