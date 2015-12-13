// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Label for an Int Variable.")]
	public class GUILayoutIntLabel : GUILayoutAction
	{
		[Tooltip("Text to put before the int variable.")]
		public FsmString prefix;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Int variable to display.")]
		public FsmInt intVariable;

		[Tooltip("Optional GUIStyle in the active GUISKin.")]
		public FsmString style;

		public override void Reset()
		{
			base.Reset();
			prefix = "";
			style = "";
			intVariable = null;
		}

		public override void OnGUI()
		{
			if (string.IsNullOrEmpty(style.Value))
			{
				GUILayout.Label(new GUIContent(prefix.Value + intVariable.Value), LayoutOptions);
			}
			else
			{
				GUILayout.Label(new GUIContent(prefix.Value + intVariable.Value), style.Value, LayoutOptions);
			}
		}
	}
}