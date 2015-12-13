// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Gets the Tooltip of the control the mouse is currently over and store it in a String Variable.")]
	public class GUITooltip : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmString storeTooltip;
			
		public override void Reset()
		{
			storeTooltip = null;
		}

		public override void OnGUI()
		{
			storeTooltip.Value = GUI.tooltip;
		}
	}
}