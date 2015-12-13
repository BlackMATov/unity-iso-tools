// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("PlayerPrefs")]
	[Tooltip("Removes key and its corresponding value from the preferences.")]
	public class PlayerPrefsDeleteKey : FsmStateAction
	{
		public FsmString key;
				
		public override void Reset()
		{
			key = "";
		}

		public override void OnEnter()
		{
			if(!key.IsNone && !key.Value.Equals("")) PlayerPrefs.DeleteKey(key.Value);
			Finish();
		}
	}
}