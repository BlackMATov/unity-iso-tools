// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Begins a vertical control group. The group must be closed with GUILayoutEndVertical action.")]
	public class GUILayoutBeginVertical : GUILayoutAction
	{
		public FsmTexture image;
		public FsmString text;
		public FsmString tooltip;
		public FsmString style;

		public override void Reset()
		{
			base.Reset();
			text = "";
			image = null;
			tooltip = "";
			style = "";
		}
		
		public override void OnGUI()
		{
			GUILayout.BeginVertical(new GUIContent(text.Value, image.Value, tooltip.Value), style.Value, LayoutOptions);
		}
	}
}