// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Application)]
	[Tooltip("Gets the Height of the Screen in pixels.")]
	public class GetScreenHeight : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeScreenHeight;

		public override void Reset()
		{
			storeScreenHeight = null;
		}
		
		public override void OnEnter()
		{
			storeScreenHeight.Value = Screen.height;
			Finish();
		}
		
	}
}