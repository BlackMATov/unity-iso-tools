// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Store the current send rate for all NetworkViews")]
	public class NetworkGetSendRate : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Store the current send rate for NetworkViews")]
		[UIHint(UIHint.Variable)]
		public FsmFloat sendRate;

		public override void Reset()
		{
			sendRate = null;
		}

		public override void OnEnter()
		{
			DoGetSendRate();
			
			Finish();
		}

		void DoGetSendRate()
		{
			sendRate.Value = Network.sendRate;	
		}
	}
}

#endif