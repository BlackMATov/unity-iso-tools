// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Label.")]
	public class GUILayoutLabel : GUILayoutAction
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
			if (string.IsNullOrEmpty(style.Value))
			{
				GUILayout.Label(new GUIContent(text.Value, image.Value, tooltip.Value), LayoutOptions);
			}
			else
			{
				GUILayout.Label(new GUIContent(text.Value, image.Value, tooltip.Value), style.Value, LayoutOptions);
			}
		}
	}
}