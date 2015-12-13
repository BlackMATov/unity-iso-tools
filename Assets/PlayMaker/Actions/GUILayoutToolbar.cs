// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Toolbar. NOTE: Arrays must be the same length as NumButtons or empty.")]
	public class GUILayoutToolbar : GUILayoutAction
	{
		public FsmInt numButtons;
		[UIHint(UIHint.Variable)]
		public FsmInt selectedButton;
		public FsmEvent[] buttonEventsArray;
		public FsmTexture[] imagesArray;
		public FsmString[] textsArray;
		public FsmString[] tooltipsArray;
		public FsmString style;

		GUIContent[] contents;

		public GUIContent[] Contents
		{
			get 
			{
				if (contents == null)
				{
					contents = new GUIContent[numButtons.Value];
					for (int i = 0; i < numButtons.Value; i++)
						contents[i] = new GUIContent();
					
					for (int i = 0; i < imagesArray.Length; i++) 
						contents[i].image = imagesArray[i].Value;
					
					for (int i = 0; i < textsArray.Length; i++) 
						contents[i].text = textsArray[i].Value;
		
					for (int i = 0; i < tooltipsArray.Length; i++) 
						contents[i].tooltip = tooltipsArray[i].Value;
				}
				
				return contents;
			}
		}
		
		public override void Reset()
		{
			base.Reset();
			numButtons = 0;
			selectedButton = null;
			buttonEventsArray = new FsmEvent[0];
			imagesArray = new FsmTexture[0];
			tooltipsArray = new FsmString[0];
			style = "Button";
		}
		
		public override void OnEnter()
		{
			string error = ErrorCheck();
			
			if (!string.IsNullOrEmpty(error))
			{
				LogError(error);
				Finish();
			}
			
		}
		
		public override void OnGUI()
		{
			var guiChanged = GUI.changed;
			GUI.changed = false;
			
			selectedButton.Value = GUILayout.Toolbar(selectedButton.Value, Contents, style.Value, LayoutOptions);
			
			if (GUI.changed)
			{
				if (selectedButton.Value < buttonEventsArray.Length)
				{
					Fsm.Event(buttonEventsArray[selectedButton.Value]);
					GUIUtility.ExitGUI();
				}
			}
			else
			{
				GUI.changed = guiChanged;
			}
		}
		
		public override string ErrorCheck ()
		{
			string error = "";
			
			if (imagesArray.Length > 0 && imagesArray.Length != numButtons.Value)
				error += "Images array doesn't match NumButtons.\n";
			if (textsArray.Length > 0 && textsArray.Length != numButtons.Value)
				error += "Texts array doesn't match NumButtons.\n";
			if (tooltipsArray.Length > 0 && tooltipsArray.Length != numButtons.Value)
				error += "Tooltips array doesn't match NumButtons.\n";
				
			return error;
		}
	}
}