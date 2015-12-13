// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Text Field to edit an Int Variable. Optionally send an event if the text has been edited.")]
	public class GUILayoutIntField : GUILayoutAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Int Variable to show in the edit field.")]
		public FsmInt intVariable;
		
		[Tooltip("Optional GUIStyle in the active GUISKin.")]
		public FsmString style;

		[Tooltip("Optional event to send when the value changes.")]
		public FsmEvent changedEvent;

		public override void Reset()
		{
			base.Reset();
			intVariable = null;
			style = "";
			changedEvent = null;
		}

		public override void OnGUI()
		{
			var guiChanged = GUI.changed;
			GUI.changed = false;

			if (!string.IsNullOrEmpty(style.Value))
			{
				intVariable.Value = int.Parse(GUILayout.TextField(intVariable.Value.ToString(), style.Value, LayoutOptions));
			}
			else
			{
				intVariable.Value = int.Parse(GUILayout.TextField(intVariable.Value.ToString(), LayoutOptions));
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