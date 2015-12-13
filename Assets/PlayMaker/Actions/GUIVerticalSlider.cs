// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("GUI Vertical Slider connected to a Float Variable.")]
	public class GUIVerticalSlider : GUIAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;
		[RequiredField]
		public FsmFloat topValue;
		[RequiredField]
		public FsmFloat bottomValue;
		public FsmString sliderStyle;
		public FsmString thumbStyle;
		
		public override void Reset()
		{
			base.Reset();
			floatVariable = null;
			topValue = 100f;
			bottomValue = 0f;
			sliderStyle = "verticalslider";
			thumbStyle = "verticalsliderthumb";
			width = 0.1f;
		}
		
		public override void OnGUI()
		{
			base.OnGUI();
			
			if(floatVariable != null)
			{
				floatVariable.Value = GUI.VerticalSlider(rect, floatVariable.Value, topValue.Value, bottomValue.Value, 
					sliderStyle.Value != "" ? sliderStyle.Value : "verticalslider", 
					thumbStyle.Value != "" ? thumbStyle.Value : "verticalsliderthumb");
			}
		}
	}
}