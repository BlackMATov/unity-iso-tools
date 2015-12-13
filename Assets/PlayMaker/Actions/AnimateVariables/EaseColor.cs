// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Easing Animation - Color")]
	public class EaseColor : EaseFsmAction
	{
		[RequiredField]
		public FsmColor fromValue;
		[RequiredField]
		public FsmColor toValue;
		[UIHint(UIHint.Variable)]
		public FsmColor colorVariable;
		
		private bool finishInNextStep = false;
		
		public override void Reset (){
			base.Reset();
			colorVariable = null;
			fromValue = null;
			toValue = null;
			finishInNextStep = false;
		}
		                   
		
		public override void OnEnter ()
		{
			base.OnEnter();
			fromFloats = new float[4];
			fromFloats[0] = fromValue.Value.r;
			fromFloats[1] = fromValue.Value.g;
			fromFloats[2] = fromValue.Value.b;
			fromFloats[3] = fromValue.Value.a;
			toFloats = new float[4];
			toFloats[0] = toValue.Value.r;
			toFloats[1] = toValue.Value.g;
			toFloats[2] = toValue.Value.b;
			toFloats[3] = toValue.Value.a;
			resultFloats = new float[4];
			finishInNextStep = false;
		    colorVariable.Value = fromValue.Value;
		}
		
		public override void OnExit (){
			base.OnExit();
		}
			
		public override void OnUpdate(){
			base.OnUpdate();
			if(!colorVariable.IsNone && isRunning){
				colorVariable.Value = new Color(resultFloats[0],resultFloats[1],resultFloats[2], resultFloats[3]);
			}
			
			if(finishInNextStep){
				Finish();
				if(finishEvent != null)	Fsm.Event(finishEvent);
			}
			
			if(finishAction && !finishInNextStep){
				if(!colorVariable.IsNone){
					colorVariable.Value = new Color(reverse.IsNone ? toValue.Value.r : reverse.Value ? fromValue.Value.r : toValue.Value.r, 
					                              reverse.IsNone ? toValue.Value.g : reverse.Value ? fromValue.Value.g : toValue.Value.g,
					                              reverse.IsNone ? toValue.Value.b : reverse.Value ? fromValue.Value.b : toValue.Value.b,
					                              reverse.IsNone ? toValue.Value.a : reverse.Value ? fromValue.Value.a : toValue.Value.a
					                              );
				}
				finishInNextStep = true;
			}
		}
	}
}