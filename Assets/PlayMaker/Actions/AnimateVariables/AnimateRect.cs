// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("AnimateVariables")]
	[Tooltip("Animates the value of a Rect Variable using an Animation Curve.")]
	public class AnimateRect : AnimateFsmAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmRect rectVariable;
		
        [RequiredField]
		public FsmAnimationCurve curveX;
		
        [Tooltip("Calculation lets you set a type of curve deformation that will be applied to rectVariable.x.")]
		public Calculation calculationX;
		
        [RequiredField]
		public FsmAnimationCurve curveY;
		
        [Tooltip("Calculation lets you set a type of curve deformation that will be applied to rectVariable.y.")]
		public Calculation calculationY;
		
        [RequiredField]
		public FsmAnimationCurve curveW;
		
        [Tooltip("Calculation lets you set a type of curve deformation that will be applied to rectVariable.width.")]
		public Calculation calculationW;
		
        [RequiredField]
		public FsmAnimationCurve curveH;
		
        [Tooltip("Calculation lets you set a type of curve deformation that will be applied to rectVariable.height.")]
		public Calculation calculationH;
				
		private bool finishInNextStep;
						
		public override void Reset()
		{
			base.Reset();

			rectVariable = new FsmRect{UseVariable=true};
		}

		public override void OnEnter()
		{
			base.OnEnter();

			finishInNextStep = false;
			resultFloats = new float[4];
			fromFloats = new float[4];
			fromFloats[0] = rectVariable.IsNone ? 0f : rectVariable.Value.x;
			fromFloats[1] = rectVariable.IsNone ? 0f : rectVariable.Value.y;
			fromFloats[2] = rectVariable.IsNone ? 0f : rectVariable.Value.width;
			fromFloats[3] = rectVariable.IsNone ? 0f : rectVariable.Value.height;
			curves = new AnimationCurve[4];
			curves[0] = curveX.curve;
			curves[1] = curveY.curve;
			curves[2] = curveW.curve;
			curves[3] = curveH.curve;
			calculations = new Calculation[4];
			calculations[0] = calculationX;
			calculations[1] = calculationY;
			calculations[2] = calculationW;
			calculations[3] = calculationH;
			
            Init();

            if (Math.Abs(delay.Value) < 0.01f)
            {
                UpdateVariableValue();
            }
			
		}

	    private void UpdateVariableValue()
	    {
	        if (!rectVariable.IsNone)
	        {
	            rectVariable.Value = new Rect(resultFloats[0], resultFloats[1], resultFloats[2], resultFloats[3]);
	        }
	    }

		public override void OnUpdate()
		{
			base.OnUpdate();

		    if (isRunning)
		    {
		        UpdateVariableValue();
		    }


		    if (finishInNextStep)
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