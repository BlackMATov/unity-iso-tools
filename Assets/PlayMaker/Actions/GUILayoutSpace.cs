// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Inserts a space in the current layout group.")]
	public class GUILayoutSpace : FsmStateAction
	{
		public FsmFloat space;
			
		public override void Reset()
		{
			space = 10;
		}

		public override void OnGUI()
		{
			GUILayout.Space(space.Value);
		}
	}
}