// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Animates the value of a Float Variable using an Animation Curve.")]
	public class AnimateFloat : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The animation curve to use.")]
		public FsmAnimationCurve animCurve;
		
        [RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The float variable to set.")]
        public FsmFloat floatVariable;
		
        [Tooltip("Optionally send an Event when the animation finishes.")]
		public FsmEvent finishEvent;
		
        [Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;

		private float startTime;
		private float currentTime;
		private float endTime;
		private bool looping;

		public override void Reset()
		{
			animCurve = null;
			floatVariable = null;
			finishEvent = null;
			realTime = false;
		}

		public override void OnEnter()
		{
			startTime = FsmTime.RealtimeSinceStartup;
			currentTime = 0f;
			
			if (animCurve != null && animCurve.curve != null && animCurve.curve.keys.Length > 0)
			{
				endTime = animCurve.curve.keys[animCurve.curve.length-1].time;
				looping = ActionHelpers.IsLoopingWrapMode(animCurve.curve.postWrapMode);
			}
			else
			{
				Finish();
				return;
			}
			
			floatVariable.Value = animCurve.curve.Evaluate(0);
		}

		public override void OnUpdate()
		{
			// update time
			
			if (realTime)
			{
				currentTime = FsmTime.RealtimeSinceStartup - startTime;
			}
			else
			{
				currentTime += Time.deltaTime;
			}
			
			// update animation
			
			if (animCurve != null && animCurve.curve != null && floatVariable != null)
			{
				floatVariable.Value = animCurve.curve.Evaluate(currentTime);
			}
			
			// send finish event
			
			if (currentTime >= endTime)
			{
				if (!looping)
				{
					Finish();
				}

				if (finishEvent != null)
				{
					Fsm.Event(finishEvent);
				}
			}
		}
	}
}