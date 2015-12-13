// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

/*
TERMS OF USE - EASING EQUATIONS
Open source under the BSD License.
Copyright (c)2001 Robert Penner
All rights reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Ease base action - don't use!")]
	public abstract class EaseFsmAction : FsmStateAction
	{
		[RequiredField]
		public FsmFloat time;
		public FsmFloat speed;
		public FsmFloat delay;
		public EaseType easeType = EaseType.linear;
		public FsmBool reverse;
		[Tooltip("Optionally send an Event when the animation finishes.")]
		public FsmEvent finishEvent;
		[Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;
				
		protected delegate float EasingFunction(float start, float end, float value);
		protected EasingFunction ease;
		protected float runningTime = 0f;
		protected float lastTime = 0f;
		protected float startTime = 0f;
		protected float deltaTime = 0f;
		protected float delayTime = 0f;
		protected float percentage = 0f;
		
		//in descendet class, please set OnEnter or Reset all these arrays to the same length
		protected float[] fromFloats = new float[0];
		protected float[] toFloats = new float[0];
		protected float[] resultFloats = new float[0];
		
		//set the end point in the descedent class and call Finish() and finishEvent in OnUpdate()
		protected bool finishAction = false;
		protected bool start = false;
		protected bool finished = false;
		protected bool isRunning = false;
		
		public override void Reset()
		{
			easeType = EaseType.linear;
			time = new FsmFloat { Value = 1f };
		    delay = new FsmFloat { UseVariable = true };
			speed = new FsmFloat { UseVariable = true };
			reverse = new FsmBool { Value = false };
			realTime = false;
			finishEvent = null;
			ease = null;
			
			runningTime = 0f;
			lastTime = 0f;
			percentage = 0f;
		
			fromFloats = new float[0];
			toFloats = new float[0];
			resultFloats = new float[0];
		
			finishAction = false;
			start= false;
			finished = false;
			isRunning = false;
		}

		public override void OnEnter()
		{
			finished = false;
			isRunning = false;
			SetEasingFunction();
			runningTime = 0f;
			percentage = reverse.IsNone ? 0f : reverse.Value ? 1f : 0f;
			finishAction = false;
			startTime = FsmTime.RealtimeSinceStartup;
			lastTime = FsmTime.RealtimeSinceStartup - startTime;
			delayTime = delay.IsNone ? 0f : delayTime = delay.Value;
			start = true;
		}
		
		public override void OnExit()
		{
					
		}
		
		public override void OnUpdate(){
			if(start && !isRunning){
				if(delayTime >= 0) {
					if(realTime){
						deltaTime = (FsmTime.RealtimeSinceStartup - startTime) - lastTime;
						lastTime = FsmTime.RealtimeSinceStartup - startTime;
						delayTime -= deltaTime;
					} else {
						delayTime -= Time.deltaTime;
					}
				} else {
					isRunning = true;
					start= false;
					startTime = FsmTime.RealtimeSinceStartup;
					lastTime = FsmTime.RealtimeSinceStartup - startTime;
				}
			}
			
			if(isRunning && !finished){
				if(!(reverse.IsNone ? false : reverse.Value)){
					UpdatePercentage();
					if(percentage<1f){
						for(int i=0; i<fromFloats.Length; i++){
							resultFloats[i] = ease(fromFloats[i],toFloats[i],percentage);
						}
					}else{
						finishAction = true;
						finished = true;
						isRunning = false;
					}
				}else{
					UpdatePercentage();
					if(percentage>0f){
						for(int i=0; i<fromFloats.Length; i++){
							resultFloats[i] = ease(fromFloats[i],toFloats[i],percentage);
						}
					}else{
						finishAction = true;
						finished = true;
						isRunning = false;
					}
				}	
			}
		}
		
		protected void UpdatePercentage(){

	        // Added by PressPlay   
	        if (realTime)
			{
				deltaTime = (FsmTime.RealtimeSinceStartup - startTime) - lastTime;
				lastTime = FsmTime.RealtimeSinceStartup - startTime;
				
				if(!speed.IsNone) runningTime += deltaTime*speed.Value;
				else runningTime += deltaTime; 
				
	        }
	        else
	        {
	            if(!speed.IsNone) runningTime += Time.deltaTime * speed.Value;
				else runningTime += Time.deltaTime;
	        }
	
			if(reverse.IsNone ? false : reverse.Value){
				percentage = 1 - runningTime/time.Value;	
			}else{
				percentage = runningTime/time.Value;	
			}
		}
		
		//instantiates a cached ease equation refrence:
		protected void SetEasingFunction(){
			switch (easeType){
			case EaseType.easeInQuad:
				ease  = new EasingFunction(easeInQuad);
				break;
			case EaseType.easeOutQuad:
				ease = new EasingFunction(easeOutQuad);
				break;
			case EaseType.easeInOutQuad:
				ease = new EasingFunction(easeInOutQuad);
				break;
			case EaseType.easeInCubic:
				ease = new EasingFunction(easeInCubic);
				break;
			case EaseType.easeOutCubic:
				ease = new EasingFunction(easeOutCubic);
				break;
			case EaseType.easeInOutCubic:
				ease = new EasingFunction(easeInOutCubic);
				break;
			case EaseType.easeInQuart:
				ease = new EasingFunction(easeInQuart);
				break;
			case EaseType.easeOutQuart:
				ease = new EasingFunction(easeOutQuart);
				break;
			case EaseType.easeInOutQuart:
				ease = new EasingFunction(easeInOutQuart);
				break;
			case EaseType.easeInQuint:
				ease = new EasingFunction(easeInQuint);
				break;
			case EaseType.easeOutQuint:
				ease = new EasingFunction(easeOutQuint);
				break;
			case EaseType.easeInOutQuint:
				ease = new EasingFunction(easeInOutQuint);
				break;
			case EaseType.easeInSine:
				ease = new EasingFunction(easeInSine);
				break;
			case EaseType.easeOutSine:
				ease = new EasingFunction(easeOutSine);
				break;
			case EaseType.easeInOutSine:
				ease = new EasingFunction(easeInOutSine);
				break;
			case EaseType.easeInExpo:
				ease = new EasingFunction(easeInExpo);
				break;
			case EaseType.easeOutExpo:
				ease = new EasingFunction(easeOutExpo);
				break;
			case EaseType.easeInOutExpo:
				ease = new EasingFunction(easeInOutExpo);
				break;
			case EaseType.easeInCirc:
				ease = new EasingFunction(easeInCirc);
				break;
			case EaseType.easeOutCirc:
				ease = new EasingFunction(easeOutCirc);
				break;
			case EaseType.easeInOutCirc:
				ease = new EasingFunction(easeInOutCirc);
				break;
			case EaseType.linear:
				ease = new EasingFunction(linear);
				break;
			case EaseType.spring:
				ease = new EasingFunction(spring);
				break;
			case EaseType.bounce:
				ease = new EasingFunction(bounce);
				break;
			case EaseType.easeInBack:
				ease = new EasingFunction(easeInBack);
				break;
			case EaseType.easeOutBack:
				ease = new EasingFunction(easeOutBack);
				break;
			case EaseType.easeInOutBack:
				ease = new EasingFunction(easeInOutBack);
				break;
			case EaseType.elastic:
				ease = new EasingFunction(elastic);
				break;
			}
		}
		
		#region EaseType
		public enum EaseType{
			easeInQuad,
			easeOutQuad,
			easeInOutQuad,
			easeInCubic,
			easeOutCubic,
			easeInOutCubic,
			easeInQuart,
			easeOutQuart,
			easeInOutQuart,
			easeInQuint,
			easeOutQuint,
			easeInOutQuint,
			easeInSine,
			easeOutSine,
			easeInOutSine,
			easeInExpo,
			easeOutExpo,
			easeInOutExpo,
			easeInCirc,
			easeOutCirc,
			easeInOutCirc,
			linear,
			spring,
			bounce,
			easeInBack,
			easeOutBack,
			easeInOutBack,
			elastic,
			punch
		}
		#endregion
		
		#region Easing Curves
		protected float linear(float start, float end, float value){
			return Mathf.Lerp(start, end, value);
		}
		
		protected float clerp(float start, float end, float value){
			float min = 0.0f;
			float max = 360.0f;
			float half = Mathf.Abs((max - min) / 2.0f);
			float retval = 0.0f;
			float diff = 0.0f;
			if ((end - start) < -half){
				diff = ((max - start) + end) * value;
				retval = start + diff;
			}else if ((end - start) > half){
				diff = -((max - end) + start) * value;
				retval = start + diff;
			}else retval = start + (end - start) * value;
			return retval;
	    }
	
		protected float spring(float start, float end, float value){
			value = Mathf.Clamp01(value);
			value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
			return start + (end - start) * value;
		}
	
		protected float easeInQuad(float start, float end, float value){
			end -= start;
			return end * value * value + start;
		}
	
		protected float easeOutQuad(float start, float end, float value){
			end -= start;
			return -end * value * (value - 2) + start;
		}
	
		protected float easeInOutQuad(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return end / 2 * value * value + start;
			value--;
			return -end / 2 * (value * (value - 2) - 1) + start;
		}
	
		protected float easeInCubic(float start, float end, float value){
			end -= start;
			return end * value * value * value + start;
		}
	
		protected float easeOutCubic(float start, float end, float value){
			value--;
			end -= start;
			return end * (value * value * value + 1) + start;
		}
	
		protected float easeInOutCubic(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return end / 2 * value * value * value + start;
			value -= 2;
			return end / 2 * (value * value * value + 2) + start;
		}
	
		protected float easeInQuart(float start, float end, float value){
			end -= start;
			return end * value * value * value * value + start;
		}
	
		protected float easeOutQuart(float start, float end, float value){
			value--;
			end -= start;
			return -end * (value * value * value * value - 1) + start;
		}
	
		protected float easeInOutQuart(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return end / 2 * value * value * value * value + start;
			value -= 2;
			return -end / 2 * (value * value * value * value - 2) + start;
		}
	
		protected float easeInQuint(float start, float end, float value){
			end -= start;
			return end * value * value * value * value * value + start;
		}
	
		protected float easeOutQuint(float start, float end, float value){
			value--;
			end -= start;
			return end * (value * value * value * value * value + 1) + start;
		}
	
		protected float easeInOutQuint(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return end / 2 * value * value * value * value * value + start;
			value -= 2;
			return end / 2 * (value * value * value * value * value + 2) + start;
		}
	
		protected float easeInSine(float start, float end, float value){
			end -= start;
			return -end * Mathf.Cos(value / 1 * (Mathf.PI / 2)) + end + start;
		}
	
		protected float easeOutSine(float start, float end, float value){
			end -= start;
			return end * Mathf.Sin(value / 1 * (Mathf.PI / 2)) + start;
		}
	
		protected float easeInOutSine(float start, float end, float value){
			end -= start;
			return -end / 2 * (Mathf.Cos(Mathf.PI * value / 1) - 1) + start;
		}
	
		protected float easeInExpo(float start, float end, float value){
			end -= start;
			return end * Mathf.Pow(2, 10 * (value / 1 - 1)) + start;
		}
	
		protected float easeOutExpo(float start, float end, float value){
			end -= start;
			return end * (-Mathf.Pow(2, -10 * value / 1) + 1) + start;
		}
	
		protected float easeInOutExpo(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return end / 2 * Mathf.Pow(2, 10 * (value - 1)) + start;
			value--;
			return end / 2 * (-Mathf.Pow(2, -10 * value) + 2) + start;
		}
	
		protected float easeInCirc(float start, float end, float value){
			end -= start;
			return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
		}
	
		protected float easeOutCirc(float start, float end, float value){
			value--;
			end -= start;
			return end * Mathf.Sqrt(1 - value * value) + start;
		}
	
		protected float easeInOutCirc(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return -end / 2 * (Mathf.Sqrt(1 - value * value) - 1) + start;
			value -= 2;
			return end / 2 * (Mathf.Sqrt(1 - value * value) + 1) + start;
		}
	
		protected float bounce(float start, float end, float value){
			value /= 1f;
			end -= start;
			if (value < (1 / 2.75f)){
				return end * (7.5625f * value * value) + start;
			}else if (value < (2 / 2.75f)){
				value -= (1.5f / 2.75f);
				return end * (7.5625f * (value) * value + .75f) + start;
			}else if (value < (2.5 / 2.75)){
				value -= (2.25f / 2.75f);
				return end * (7.5625f * (value) * value + .9375f) + start;
			}else{
				value -= (2.625f / 2.75f);
				return end * (7.5625f * (value) * value + .984375f) + start;
			}
		}
	
		protected float easeInBack(float start, float end, float value){
			end -= start;
			value /= 1;
			float s = 1.70158f;
			return end * (value) * value * ((s + 1) * value - s) + start;
		}
	
		protected float easeOutBack(float start, float end, float value){
			float s = 1.70158f;
			end -= start;
			value = (value / 1) - 1;
			return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
		}
	
		protected float easeInOutBack(float start, float end, float value){
			float s = 1.70158f;
			end -= start;
			value /= .5f;
			if ((value) < 1){
				s *= (1.525f);
				return end / 2 * (value * value * (((s) + 1) * value - s)) + start;
			}
			value -= 2;
			s *= (1.525f);
			return end / 2 * ((value) * value * (((s) + 1) * value + s) + 2) + start;
		}
	
		protected float punch(float amplitude, float value){
			float s = 9;
			if (value == 0){
				return 0;
			}
			if (value == 1){
				return 0;
			}
			float period = 1 * 0.3f;
			s = period / (2 * Mathf.PI) * Mathf.Asin(0);
			return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
	    }
		
		protected float elastic(float start, float end, float value){
			//Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
			end -= start;
			
			float d = 1f;
			float p = d * .3f;
			float s = 0;
			float a = 0;
			
			if (value == 0) return start;
			
			if ((value /= d) == 1) return start + end;
			
			if (a == 0f || a < Mathf.Abs(end)){
				a = end;
				s = p / 4;
				}else{
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}
			
			return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
		}		
		
		#endregion	
		
	}
}