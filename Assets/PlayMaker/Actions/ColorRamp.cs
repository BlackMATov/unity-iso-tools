// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Samples a Color on a continuous Colors gradient.")]
	public class ColorRamp : FsmStateAction
	{
		[RequiredField]
        [Tooltip("Array of colors to defining the gradient.")]
		public FsmColor[] colors;

		[RequiredField]
        [Tooltip("Point on the gradient to sample. Should be between 0 and the number of colors in the gradient.")]
		public FsmFloat sampleAt;

		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the sampled color in a Color variable.")]
		public FsmColor storeColor;

        [Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
		
		public override void Reset()
		{
			colors = new FsmColor[3];
			sampleAt = 0;
			storeColor = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoColorRamp();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoColorRamp();
		}
		
		void DoColorRamp()
		{
			if (colors == null) return;
			if (colors.Length == 0) return;
			if (sampleAt == null) return;
			if (storeColor == null) return;
			
			Color lerpColor;
			var lerpAmount = Mathf.Clamp(sampleAt.Value, 0, colors.Length-1);

			if (lerpAmount == 0)
			{
			    lerpColor = colors[0].Value;
			}
			else if (lerpAmount == colors.Length)
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
			if (colors.Length < 2)
			{
			    return "Define at least 2 colors to make a gradient.";
			}
			return null;
		}
	}
}