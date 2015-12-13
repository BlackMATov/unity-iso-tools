// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Sets the GUISkin used by GUI elements.")]
	public class SetGUISkin : FsmStateAction
	{
		[RequiredField]
		public GUISkin skin;
		public FsmBool applyGlobally;

		public override void Reset()
		{
			skin = null;
			applyGlobally = true;
		}

		public override void OnGUI()
		{
			if (skin != null)
				GUI.skin = skin;
			
			if (applyGlobally.Value)
			{
				PlayMakerGUI.GUISkin = skin;
				Finish();
			}
		}
	}
}