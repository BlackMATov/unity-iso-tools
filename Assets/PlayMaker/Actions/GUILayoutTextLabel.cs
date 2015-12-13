// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Label for simple text.")]
	public class GUILayoutTextLabel : GUILayoutAction
	{
		[Tooltip("Text to display.")]
		public FsmString text;

		[Tooltip("Optional GUIStyle in the active GUISkin.")]
		public FsmString style;

		public override void Reset()
		{
			base.Reset();
			text = "";
			style = "";
		}

		public override void OnGUI()
		{
			if (string.IsNullOrEmpty(style.Value))
			{
				GUILayout.Label(new GUIContent(text.Value), LayoutOptions);
			}
			else
			{
				GUILayout.Label(new GUIContent(text.Value), style.Value, LayoutOptions);
			}
		}
	}
}