// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Unregister this server from the master server.\n\n" +
		"Does nothing if the server is not registered or has already unregistered.")]
	public class MasterServerUnregisterHost : FsmStateAction
	{		
		public override void OnEnter()
		{
			MasterServer.UnregisterHost();
			
			Finish();
		}
	}
}

#endif