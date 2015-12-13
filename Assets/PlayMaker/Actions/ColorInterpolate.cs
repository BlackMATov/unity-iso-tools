// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Interpolate through an array of Colors over a specified amount of Time.")]
	public class ColorInterpolate : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Array of colors to interpolate through.")]
		public FsmColor[] colors;
		
		[RequiredField]
		[Tooltip("Interpolation time.")]
		public FsmFloat time;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the interpolated color in a Color variable.")]
		public FsmColor storeColor;
		
		[Tooltip("Event to send when the interpolation finishes.")]
		public FsmEvent finishEvent;
		
		[Tooltip("Ignore TimeScale")]
		public bool realTime;

		private float startTime;
		private float currentTime;
		
		public override void Reset()
		{
			colors = new FsmColor[3];
			time = 1.0f;
			storeColor = null;
			finishEvent = null;
			realTime = false;
		}

		public override void OnEnter()
		{
			startTime = FsmTime.RealtimeSinceStartup;
			currentTime = 0f;

			if (colors.Length < 2)
			{
				if (colors.Length == 1)
				{
					storeColor.Value = colors[0].Value;
				}
				Finish();
			}
			else
			{
				storeColor.Value = colors[0].Value;
			}
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
			
			// finished?
			
			if (currentTime > time.Value)
			{
				Finish();

				storeColor.Value = colors[colors.Length - 1].Value;
				
				if (finishEvent != null)
				{
					Fsm.Event(finishEvent);
				}

				return;
			}
			
			// lerp
			
			Color lerpColor;
			var lerpAmount = (colors.Length-1) * currentTime/time.Value;

			if (lerpAmount.Equals(0))
			{
				lerpColor = colors[0].Value;
			}
			
			else if (lerpAmount.Equals(colors.Length-1))
			{
				lerpColor = colors[colors.Length-1].Value;
			}
			
			else
			{
				var color1 = colors[Mathf.FloorToInt(lerpAmount)].Value;
				var color2 = colors[Mathf.CeilToInt(lerpAmount)].Value;
				lerpAmount -= Mathf.Floor(lerpAmount);
				
				lerpColor = Color.Lerp(color1, color2, lerpAmount);
			}
			
			storeColor.Value = lerpColor;
		}
		
		public override string ErrorCheck ()
		{
			return colors.Length < 2 ? "Define at least 2 colors to make a gradient." : null;
		}
	}
}