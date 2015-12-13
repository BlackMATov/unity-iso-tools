// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Begin a centered GUILayout block. The block is centered inside a parent GUILayout Area. So to place the block in the center of the screen, first use a GULayout Area the size of the whole screen (the default setting). NOTE: Block must end with a corresponding GUILayoutEndCentered.")]
	public class GUILayoutBeginCentered : FsmStateAction
	{
		public override void Reset()
		{
		}

		public override void OnGUI()
		{
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
		}
	}
}