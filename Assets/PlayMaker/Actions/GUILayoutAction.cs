// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("GUILayout base action - don't use!")]
	public abstract class GUILayoutAction : FsmStateAction
	{
		public LayoutOption[] layoutOptions;

		GUILayoutOption[] options;
		
		public GUILayoutOption[] LayoutOptions
		{
			get
			{
				if (options == null)
				{
					options = new GUILayoutOption[layoutOptions.Length];
					for (int i = 0; i < layoutOptions.Length; i++) 
						options[i] = layoutOptions[i].GetGUILayoutOption();
				}
				
				return options;
			}
		}
		
		public override void Reset()
		{
			layoutOptions = new LayoutOption[0];
		}
	}
}