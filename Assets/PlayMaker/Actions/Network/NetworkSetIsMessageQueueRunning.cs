// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Enable or disable the processing of network messages.\n\nIf this is disabled no RPC call execution or network view synchronization takes place.")]
	public class NetworkSetIsMessageQueueRunning : FsmStateAction
	{
		[Tooltip("Is Message Queue Running. If this is disabled no RPC call execution or network view synchronization takes place")]
		public FsmBool isMessageQueueRunning;
		
		public override void Reset()
		{
			isMessageQueueRunning = null;
		}

		public override void OnEnter()
		{
			Network.isMessageQueueRunning = isMessageQueueRunning.Value;
			
			Finish();
		}

	}
}

#endif