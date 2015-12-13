// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Animates the value of a Vector3 Variable FROM-TO with assistance of Deformation Curves.")]
	public class CurveVector3: CurveFsmAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vectorVariable;
		[RequiredField]
		public FsmVector3 fromValue;
		[RequiredField]
		public FsmVector3 toValue;
		[RequiredField]
		public FsmAnimationCurve curveX;
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.x and toValue.x.")]
		public Calculation calculationX;
		[RequiredField]
		public FsmAnimationCurve curveY;
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.y and toValue.y.")]
		public Calculation calculationY;
		[RequiredField]
		public FsmAnimationCurve curveZ;
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.z and toValue.z.")]
		public Calculation calculationZ;
				
		Vector3 vct;
		
		private bool finishInNextStep = false;
				
		public override void Reset()
		{
			base.Reset();
			vectorVariable = new FsmVector3{UseVariable=true};
			toValue = new FsmVector3{UseVariable=true};
			fromValue = new FsmVector3{UseVariable=true};
		}

		public override void OnEnter()
		{
			base.OnEnter();
			finishInNextStep = false;
			resultFloats = new float[3];
			fromFloats = new float[3];
			fromFloats[0] = fromValue.IsNone ? 0f : fromValue.Value.x;
			fromFloats[1] = fromValue.IsNone ? 0f : fromValue.Value.y;
			fromFloats[2] = fromValue.IsNone ? 0f : fromValue.Value.z;
			toFloats = new float[3];
			toFloats[0] = toValue.IsNone ? 0f : toValue.Value.x;
			toFloats[1] = toValue.IsNone ? 0f : toValue.Value.y;
			toFloats[2] = toValue.IsNone ? 0f : toValue.Value.z;
			
			curves = new AnimationCurve[3];
			curves[0] = curveX.curve;
			curves[1] = curveY.curve;
			curves[2] = curveZ.curve;
			calculations = new Calculation[3];
			calculations[0] = calculationX;
			calculations[1] = calculationY;
			calculations[2] = calculationZ;
			//call Init after you have initialized curves array
			Init();
		}
		
		public override void OnExit()
		{
				
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if(!vectorVariable.IsNone && isRunning){
				vct = new Vector3(resultFloats[0], resultFloats[1], resultFloats[2]);
				vectorVariable.Value = vct;
			}
			
			if(finishInNextStep){
				if(!looping) {
					Finish();
					if(finishEvent != null)	Fsm.Event(finishEvent);
				}
			}
			
			if(finishAction && !finishInNextStep){
				if(!vectorVariable.IsNone){
					vct = new Vector3(resultFloats[0], resultFloats[1], resultFloats[2]);
					vectorVariable.Value = vct;
				}
				finishInNextStep = true;
			}
		}
	}
}