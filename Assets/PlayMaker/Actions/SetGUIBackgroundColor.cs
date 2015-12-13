// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Sets the Tinting Color for all background elements rendered by the GUI. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
	public class SetGUIBackgroundColor : FsmStateAction
	{
		[RequiredField]
		public FsmColor backgroundColor;
		public FsmBool applyGlobally;
	
		public override void Reset()
		{
			backgroundColor = Color.white;
		}

		public override void OnGUI()
		{
			GUI.backgroundColor = backgroundColor.Value;
			
			if (applyGlobally.Value)
			{
				PlayMakerGUI.GUIBackgroundColor = GUI.backgroundColor;
			}
		}
	}
}