// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("AnimateVariables")]
	[Tooltip("Easing Animation - Rect.")]
	public class EaseRect : EaseFsmAction
	{
		[RequiredField]
		public FsmRect fromValue;
		[RequiredField]
		public FsmRect toValue;
		[UIHint(UIHint.Variable)]
		public FsmRect rectVariable;
		
		private bool finishInNextStep = false;
		
		public override void Reset (){
			base.Reset();
			rectVariable = null;
			fromValue = null;
			toValue = null;
			finishInNextStep = false;
		}
		                   
		
		public override void OnEnter ()
		{
			base.OnEnter();
			fromFloats = new float[4];
			fromFloats[0] = fromValue.Value.x;
			fromFloats[1] = fromValue.Value.y;
			fromFloats[2] = fromValue.Value.width;
			fromFloats[3] = fromValue.Value.height;
			toFloats = new float[4];
			toFloats[0] = toValue.Value.x;
			toFloats[1] = toValue.Value.y;
			toFloats[2] = toValue.Value.width;
			toFloats[3] = toValue.Value.height;
			resultFloats = new float[4];
			finishInNextStep = false;
		    rectVariable.Value = fromValue.Value;
		}
		
		public override void OnExit (){
			base.OnExit();
		}
			
		public override void OnUpdate(){
			base.OnUpdate();
			if(!rectVariable.IsNone && isRunning){
				rectVariable.Value = new Rect(resultFloats[0],resultFloats[1],resultFloats[2], resultFloats[3]);
			}
			
			if(finishInNextStep){
				Finish();
				if(finishEvent != null)	Fsm.Event(finishEvent);
			}
			
			if(finishAction && !finishInNextStep){
				if(!rectVariable.IsNone){
					rectVariable.Value = new Rect(reverse.IsNone ? toValue.Value.x : reverse.Value ? fromValue.Value.x : toValue.Value.x, 
					                              reverse.IsNone ? toValue.Value.y : reverse.Value ? fromValue.Value.y : toValue.Value.y,
					                              reverse.IsNone ? toValue.Value.width : reverse.Value ? fromValue.Value.width : toValue.Value.width,
					                              reverse.IsNone ? toValue.Value.height : reverse.Value ? fromValue.Value.height : toValue.Value.height
					                              );
				}
				finishInNextStep = true;
			}
		}
	}
}