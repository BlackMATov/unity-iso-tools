// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	// base type for GUI actions that need a Rect
	[Tooltip("GUI base action - don't use!")]
	public abstract class GUIAction : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmRect screenRect;

		public FsmFloat left;
		public FsmFloat top;		
		public FsmFloat width;		
		public FsmFloat height;
		
		[RequiredField]
		public FsmBool normalized;
		
		internal Rect rect;
		
		public override void Reset()
		{
			screenRect = null;
			left = 0;
			top = 0;
			width = 1;
			height = 1;
			normalized = true;
		}
		
		public override void OnGUI()
		{
			rect = !screenRect.IsNone ? screenRect.Value : new Rect();
			
			if (!left.IsNone) rect.x = left.Value;
			if (!top.IsNone) rect.y = top.Value;
			if (!width.IsNone) rect.width = width.Value;
			if (!height.IsNone) rect.height = height.Value;
			
			if (normalized.Value)
			{
				rect.x *= Screen.width;
				rect.width *= Screen.width;
				rect.y *= Screen.height;
				rect.height *= Screen.height;
			}
		}
	}
}