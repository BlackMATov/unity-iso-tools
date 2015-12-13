// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Get the RGBA channels of a Color Variable and store them in Float Variables.")]
	public class GetColorRGBA : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("The Color variable.")]
		public FsmColor color;

		[UIHint(UIHint.Variable)]
        [Tooltip("Store the red channel in a float variable.")]
		public FsmFloat storeRed;	
	
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the green channel in a float variable.")]
		public FsmFloat storeGreen;	
	
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the blue channel in a float variable.")]
		public FsmFloat storeBlue;
		
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the alpha channel in a float variable.")]
		public FsmFloat storeAlpha;

        [Tooltip("Repeat every frame. Useful if the color variable is changing.")]
		public bool everyFrame;
		
		public override void Reset()
		{
			color = null;
			storeRed = null;
			storeGreen = null;
			storeBlue = null;
			storeAlpha = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetColorRGBA();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate ()
		{
			DoGetColorRGBA();
		}
		
		void DoGetColorRGBA()
		{
			if (color.IsNone)
			{
				return;
			}
			
			storeRed.Value = color.Value.r;
			storeGreen.Value = color.Value.g;
			storeBlue.Value = color.Value.b;
			storeAlpha.Value = color.Value.a;
		}
	}
}