// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Animates the value of a Vector3 Variable using an Animation Curve.")]
	public class AnimateVector3 : AnimateFsmAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vectorVariable;
		
        [RequiredField]
		public FsmAnimationCurve curveX;
		
        [Tooltip("Calculation lets you set a type of curve deformation that will be applied to vectorVariable.x.")]
		public Calculation calculationX;
		
        [RequiredField]
		public FsmAnimationCurve curveY;
		
        [Tooltip("Calculation lets you set a type of curve deformation that will be applied to vectorVariable.y.")]
		public Calculation calculationY;
		
        [RequiredField]
		public FsmAnimationCurve curveZ;
		
        [Tooltip("Calculation lets you set a type of curve deformation that will be applied to vectorVariable.z.")]
		public Calculation calculationZ;
				
		private bool finishInNextStep;
					
		public override void Reset()
		{
			base.Reset();
			vectorVariable = new FsmVector3{UseVariable=true};
		}

		public override void OnEnter()
		{
			base.OnEnter();

			finishInNextStep = false;
			resultFloats = new float[3];
			fromFloats = new float[3];
			fromFloats[0] = vectorVariable.IsNone ? 0f : vectorVariable.Value.x;
			fromFloats[1] = vectorVariable.IsNone ? 0f : vectorVariable.Value.y;
			fromFloats[2] = vectorVariable.IsNone ? 0f : vectorVariable.Value.z;
			curves = new AnimationCurve[3];
			curves[0] = curveX.curve;
			curves[1] = curveY.curve;
			curves[2] = curveZ.curve;
			calculations = new Calculation[3];
			calculations[0] = calculationX;
			calculations[1] = calculationY;
			calculations[2] = calculationZ;
			
            Init();

            // Set initial value
            if (Math.Abs(delay.Value) < 0.01f)
            {
                UpdateVariableValue();
            }
        }

	    private void UpdateVariableValue()
	    {
            if (!vectorVariable.IsNone)
            {
                vectorVariable.Value = new Vector3(resultFloats[0], resultFloats[1], resultFloats[2]);
            }	        
	    }

		public override void OnUpdate()
		{
			base.OnUpdate();

			if(isRunning)
            {
				UpdateVariableValue();
			}
			
			if(finishInNextStep)
            {
				if(!looping) 
                {
					Finish();
                    Fsm.Event(finishEvent);
				}
				
			}
			
			if(finishAction && !finishInNextStep)
            {
				UpdateVariableValue();
				finishInNextStep = true;
			}
		}
	}
}