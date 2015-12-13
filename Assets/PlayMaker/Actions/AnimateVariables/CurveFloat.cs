// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Animates the value of a Float Variable FROM-TO with assistance of Deformation Curve.")]
	public class CurveFloat: CurveFsmAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;
		[RequiredField]
		public FsmFloat fromValue;
		[RequiredField]
		public FsmFloat toValue;
		[RequiredField]
		public FsmAnimationCurve animCurve;
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue and toValue.")]
		public Calculation calculation;
		
		
		private bool finishInNextStep = false;
				
		public override void Reset()
		{
			base.Reset();
			floatVariable = new FsmFloat{UseVariable=true};
			toValue = new FsmFloat{UseVariable=true};
			fromValue = new FsmFloat{UseVariable=true};
		}

		public override void OnEnter()
		{
			base.OnEnter();
			finishInNextStep = false;
			resultFloats = new float[1];
			fromFloats = new float[1];
			fromFloats[0] = fromValue.IsNone ? 0f : fromValue.Value;
			toFloats = new float[1];
			toFloats[0] = toValue.IsNone ? 0f : toValue.Value;
			calculations = new Calculation[1];
			calculations[0] = calculation;
			curves = new AnimationCurve[1];
			curves[0] = animCurve.curve;
			//call Init after you have initialized curves array
			Init();
		}
		
		public override void OnExit()
		{
				
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if(!floatVariable.IsNone && isRunning){
				floatVariable.Value =  resultFloats[0];
			}
			
			if(finishInNextStep){
				if(!looping) {
					Finish();
					if(finishEvent != null)	Fsm.Event(finishEvent);
				}
				
			}
			
			if(finishAction && !finishInNextStep){
				if(!floatVariable.IsNone){
					floatVariable.Value =  resultFloats[0];
				}
				finishInNextStep = true;
			}
		}
	}
}