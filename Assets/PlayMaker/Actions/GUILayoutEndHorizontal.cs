// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Close a group started with BeginHorizontal.")]
	public class GUILayoutEndHorizontal : FsmStateAction
	{
		public override void Reset()
		{
		}

		public override void OnGUI()
		{
			GUILayout.EndHorizontal();
		}
	}
}