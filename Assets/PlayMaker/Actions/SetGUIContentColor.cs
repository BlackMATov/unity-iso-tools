// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Sets the Tinting Color for all text rendered by the GUI. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
	public class SetGUIContentColor : FsmStateAction
	{
		[RequiredField]
		public FsmColor contentColor;
		public FsmBool applyGlobally;
	
		public override void Reset()
		{
			contentColor = Color.white;
		}

		public override void OnGUI()
		{
			GUI.contentColor = contentColor.Value;
			
			if (applyGlobally.Value)
			{
				PlayMakerGUI.GUIContentColor = GUI.contentColor;
			}
		}
	}
}