using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Password Field. Optionally send an event if the text has been edited.")]
	public class GUILayoutEmailField : GUILayoutAction {

		[UIHint(UIHint.Variable)]
		public FsmString text;
		public FsmInt maxLength;
		public FsmString style;
		public FsmEvent changedEvent;
		public FsmBool valid;

		public override void Reset()
		{
			text = null;
			maxLength = 25;
			style = "TextField";
			valid = true;
			changedEvent = null;
		}
		
		public override void OnGUI()
		{
			var guiChanged = GUI.changed;
			GUI.changed = false;
			
			text.Value = GUILayout.TextField(text.Value, style.Value, LayoutOptions);
			
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
