// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the number of hosts on the master server.\n\nUse MasterServer Get Host Data to get host data at a specific index.")]
	public class MasterServerGetHostCount : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The number of hosts on the MasterServer.")]
		[UIHint(UIHint.Variable)]
		public FsmInt count;

		public override void OnEnter()
		{
			count.Value = MasterServer.PollHostList().Length;

			Finish();
		}
	}
}

#endif