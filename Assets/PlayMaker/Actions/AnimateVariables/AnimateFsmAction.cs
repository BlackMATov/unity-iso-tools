// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    /// <summary>
    /// Base action for AnimateXXX actions
    /// </summary>
	public abstract class AnimateFsmAction : FsmStateAction
	{
		[Tooltip("Define time to use your curve scaled to be stretched or shrinked.")]
		public FsmFloat time;
		
        [Tooltip("If you define speed, your animation will be speeded up or slowed down.")]
		public FsmFloat speed;
		
        [Tooltip("Delayed animimation start.")]
		public FsmFloat delay;
		
        [Tooltip("Animation curve start from any time. If IgnoreCurveOffset is true the animation starts right after the state become entered.")]
		public FsmBool ignoreCurveOffset;
		
        [Tooltip("Optionally send an Event when the animation finishes.")]
		public FsmEvent finishEvent;
		
        [Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;

		private float startTime;
		private float currentTime;
		private float[] endTimes;
		
		private float lastTime;
		private float deltaTime;
		private float delayTime;
		private float[] keyOffsets;
		
		protected AnimationCurve[] curves;
		protected Calculation[] calculations;
		
		protected float[] resultFloats;
		protected float[] fromFloats;
		protected float[] toFloats;
				
		protected bool finishAction = false;
		protected bool isRunning;
		protected bool looping;
		private bool start = false;
		private float largestEndTime = 0f;
		
		public enum Calculation{
			None,
			SetValue,
			AddToValue,
			SubtractFromValue,
			SubtractValueFromCurve,
			MultiplyValue,
			DivideValue,
			DivideCurveByValue,
		}
		
		public override void Reset()
		{
			finishEvent = null;
			realTime = false;
			time = new FsmFloat { UseVariable = true };
			speed = new FsmFloat { UseVariable = true };
			delay = new FsmFloat { UseVariable = true };
			ignoreCurveOffset = new FsmBool{ Value = true};
			resultFloats = new float[0];
			fromFloats = new float[0];
			toFloats = new float[0];
			endTimes = new float[0];
			keyOffsets = new float[0];
			curves = new AnimationCurve[0];
			finishAction = false;
			start = false;
		}

		public override void OnEnter()
		{
			startTime = FsmTime.RealtimeSinceStartup;
			lastTime = FsmTime.RealtimeSinceStartup - startTime;
			deltaTime = 0f;
			currentTime = 0f;
			isRunning = false;
			finishAction = false;
			looping = false;
			delayTime = delay.IsNone ? 0f : delayTime = delay.Value;
			start = true;
		}
		
		protected void Init()
        {
			endTimes = new float[curves.Length];
			keyOffsets = new float[curves.Length];
			largestEndTime = 0f;
			for(int i = 0; i<curves.Length;i++)
            {
				if (curves[i] != null && curves[i].keys.Length > 0)
				{
					keyOffsets[i] = curves[i].keys.Length > 0 ? (time.IsNone ? curves[i].keys[0].time : (time.Value/curves[i].keys[curves[i].length-1].time)*curves[i].keys[0].time) : 0f;
					currentTime = ignoreCurveOffset.IsNone ? 0f : (ignoreCurveOffset.Value ? keyOffsets[i] : 0f);
					if(!time.IsNone) endTimes[i] = time.Value;
					else endTimes[i] = curves[i].keys[curves[i].length-1].time;
					if(largestEndTime < endTimes[i]) largestEndTime = endTimes[i];
					if(!looping) looping = ActionHelpers.IsLoopingWrapMode(curves[i].postWrapMode);
				} 
                else 
                {
					endTimes[i] = -1f;	
				}
			}
			for(int i = 0; i<curves.Length;i++)
            {
				if(largestEndTime > 0f && endTimes[i] == -1f) endTimes[i] = largestEndTime;
				else {
					if(largestEndTime == 0f && endTimes[i] == -1f) 
                    {
						if(time.IsNone) endTimes[i] = 1f;
						else endTimes[i] = time.Value;
					}
				}
			}

            // set initial result value
            UpdateAnimation();
		}

		public override void OnUpdate()
		{
		    CheckStart();

		    if(isRunning)
            {
				UpdateTime();				
				UpdateAnimation();
			    CheckFinished();
			}
		}

	    private void CheckStart()
	    {
	        if (!isRunning && start)
	        {
	            if (delayTime >= 0)
	            {
	                if (realTime)
	                {
	                    deltaTime = (FsmTime.RealtimeSinceStartup - startTime) - lastTime;
	                    lastTime = FsmTime.RealtimeSinceStartup - startTime;
	                    delayTime -= deltaTime;
	                }
	                else
	                {
	                    delayTime -= Time.deltaTime;
	                }
	            }
	            else
	            {
	                isRunning = true;
	                start = false;
	            }
	        }
	    }

	    private void UpdateTime()
	    {
	        if (realTime)
	        {
	            deltaTime = (FsmTime.RealtimeSinceStartup - startTime) - lastTime;
	            lastTime = FsmTime.RealtimeSinceStartup - startTime;

	            if (!speed.IsNone) currentTime += deltaTime*speed.Value;
	            else currentTime += deltaTime;
	        }
	        else
	        {
	            if (!speed.IsNone) currentTime += Time.deltaTime*speed.Value;
	            else currentTime += Time.deltaTime;
	        }
	    }

	    public void UpdateAnimation()
	    {
	        for (var k = 0; k < curves.Length; k++)
	        {
	            if (curves[k] != null && curves[k].keys.Length > 0)
	            {
	                if (calculations[k] != AnimateFsmAction.Calculation.None)
	                {
	                    switch (calculations[k])
	                    {
	                        case Calculation.SetValue:
	                            if (!time.IsNone)
	                                resultFloats[k] =
	                                    curves[k].Evaluate((currentTime/time.Value)*curves[k].keys[curves[k].length - 1].time);
	                            else resultFloats[k] = curves[k].Evaluate(currentTime);
	                            break;
	                        case Calculation.AddToValue:
	                            if (!time.IsNone)
	                                resultFloats[k] = fromFloats[k] +
	                                                  curves[k].Evaluate((currentTime/time.Value)*
	                                                                     curves[k].keys[curves[k].length - 1].time);
	                            else resultFloats[k] = fromFloats[k] + curves[k].Evaluate(currentTime);
	                            break;
	                        case Calculation.SubtractFromValue:
	                            if (!time.IsNone)
	                                resultFloats[k] = fromFloats[k] -
	                                                  curves[k].Evaluate((currentTime/time.Value)*
	                                                                     curves[k].keys[curves[k].length - 1].time);
	                            else resultFloats[k] = fromFloats[k] - curves[k].Evaluate(currentTime);
	                            break;
	                        case Calculation.SubtractValueFromCurve:
	                            if (!time.IsNone)
	                                resultFloats[k] =
	                                    curves[k].Evaluate((currentTime/time.Value)*curves[k].keys[curves[k].length - 1].time) -
	                                    fromFloats[k];
	                            else resultFloats[k] = curves[k].Evaluate(currentTime) - fromFloats[k];
	                            break;
	                        case Calculation.MultiplyValue:
	                            if (!time.IsNone)
	                                resultFloats[k] =
	                                    curves[k].Evaluate((currentTime/time.Value)*curves[k].keys[curves[k].length - 1].time)*
	                                    fromFloats[k];
	                            else resultFloats[k] = curves[k].Evaluate(currentTime)*fromFloats[k];
	                            break;
	                        case Calculation.DivideValue:
	                            if (!time.IsNone)
	                                resultFloats[k] =
	                                    curves[k].Evaluate((currentTime/time.Value)*curves[k].keys[curves[k].length - 1].time) !=
	                                    0f
	                                        ? fromFloats[k]/
	                                          curves[k].Evaluate((currentTime/time.Value)*
	                                                             curves[k].keys[curves[k].length - 1].time)
	                                        : float.MaxValue;
	                            else
	                                resultFloats[k] = curves[k].Evaluate(currentTime) != 0
	                                    ? fromFloats[k]/curves[k].Evaluate(currentTime)
	                                    : float.MaxValue;
	                            break;
	                        case Calculation.DivideCurveByValue:
	                            if (!time.IsNone)
	                                resultFloats[k] = fromFloats[k] != 0f
	                                    ? curves[k].Evaluate((currentTime/time.Value)*curves[k].keys[curves[k].length - 1].time)/
	                                      fromFloats[k]
	                                    : float.MaxValue;
	                            else
	                                resultFloats[k] = fromFloats[k] != 0
	                                    ? curves[k].Evaluate(currentTime)/fromFloats[k]
	                                    : float.MaxValue;
	                            break;
	                    }
	                }
	                else
	                {
	                    resultFloats[k] = fromFloats[k];
	                }
	            }
	            else
	            {
	                resultFloats[k] = fromFloats[k];
	            }
	        }
	    }
        
        private void CheckFinished()
        {
            if (isRunning && !looping)
            {
                finishAction = true;
                for (int i = 0; i < endTimes.Length; i++)
                {
                    //Debug.Log(i.ToString() + "| " +endTimes[i].ToString() + " " + currentTime.ToString());
                    if (currentTime < endTimes[i]) finishAction = false;
                }
                isRunning = !finishAction;
            }
        }
    }
}