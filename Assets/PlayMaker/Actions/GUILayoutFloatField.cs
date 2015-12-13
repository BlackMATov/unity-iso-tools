// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Text Field to edit a Float Variable. Optionally send an event if the text has been edited.")]
	public class GUILayoutFloatField : GUILayoutAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Float Variable to show in the edit field.")]
		public FsmFloat floatVariable;

		[Tooltip("Optional GUIStyle in the active GUISKin.")]
		public FsmString style;

		[Tooltip("Optional event to send when the value changes.")]
		public FsmEvent changedEvent;

		public override void Reset()
		{
			base.Reset();
			floatVariable = null;
			style = "";
			changedEvent = null;
		}

		public override void OnGUI()
		{
			var guiChanged = GUI.changed;
			GUI.changed = false;

			if (!string.IsNullOrEmpty(style.Value))
			{
				floatVariable.Value = float.Parse(GUILayout.TextField(floatVariable.Value.ToString(), style.Value, LayoutOptions));
			}
			else
			{
				floatVariable.Value = float.Parse(GUILayout.TextField(floatVariable.Value.ToString(), LayoutOptions));
			}

			if (GUI.changed)
			{
				Fsm.Event(changedEvent);
				GUIUtility.ExitGUI();
			}
			else
			{
				GUI.changed = guiChanged;
			}
		}
	}
}