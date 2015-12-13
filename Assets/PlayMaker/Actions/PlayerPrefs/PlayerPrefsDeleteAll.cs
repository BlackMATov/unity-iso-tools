// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("PlayerPrefs")]
	[Tooltip("Removes all keys and values from the preferences. Use with caution.")]
	public class PlayerPrefsDeleteAll : FsmStateAction
	{
		public override void Reset()
		{
		}
		
		public override void OnEnter()
		{
			PlayerPrefs.DeleteAll();
			Finish();
		}
	}
}