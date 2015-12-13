// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Text Field. Optionally send an event if the text has been edited.")]
	public class GUILayoutTextField : GUILayoutAction
	{
		[UIHint(UIHint.Variable)]
		public FsmString text;
		public FsmInt maxLength;
		public FsmString style;
		public FsmEvent changedEvent;

		public override void Reset()
		{
			base.Reset();
			text = null;
			maxLength = 25;
			style = "TextField";
			changedEvent = null;
		}
		
		public override void OnGUI()
		{
			var guiChanged = GUI.changed;
			GUI.changed = false;
			
			text.Value = GUILayout.TextField(text.Value, maxLength.Value, style.Value, LayoutOptions);
			
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