// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Application)]
	[Tooltip("Quits the player application.")]
	public class ApplicationQuit : FsmStateAction
	{
		public override void Reset()
		{
		}

		public override void OnEnter()
		{
			Application.Quit();

			Finish();
		}
	}
}