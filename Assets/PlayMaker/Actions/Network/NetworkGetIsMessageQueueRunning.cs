// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get if network messages are enabled or disabled.\n\nIf disabled no RPC call execution or network view synchronization takes place")]
	public class NetworkGetIsMessageQueueRunning : FsmStateAction
	{
		[Tooltip("Is Message Queue Running. If this is disabled no RPC call execution or network view synchronization takes place")]
		[UIHint(UIHint.Variable)]
		public FsmBool result;

		public override void Reset()
		{
			result = null;
		}

		public override void OnEnter()
		{
			result.Value = Network.isMessageQueueRunning;
			
			Finish();
		}
	}
}

#endif