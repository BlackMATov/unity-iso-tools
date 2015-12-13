// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	// base type for GUI actions with GUIContent parameters
	
	[Tooltip("GUI base action - don't use!")]
	public abstract class GUIContentAction : GUIAction
	{
		public FsmTexture image;
		public FsmString text;
		public FsmString tooltip;
		public FsmString style;
		
		internal GUIContent content;
		
		public override void Reset()
		{
			base.Reset();
			image = null;
			text = "";
			tooltip = "";
			style = "";
		}
		
		public override void OnGUI()
		{
			base.OnGUI();
			
			content = new GUIContent(text.Value, image.Value, tooltip.Value);
		}
	}
}
