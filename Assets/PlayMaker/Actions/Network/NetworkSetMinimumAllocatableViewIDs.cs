// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Set the minimum number of ViewID numbers in the ViewID pool given to clients by the server. The default value is 100.\n\n" +
		"The ViewID pools are given to each player as he connects and are refreshed with new numbers if the player runs out. " +
		"The server and clients should be in sync regarding this value.\n\n" +
		"Setting this higher only on the server has the effect that he sends more view ID numbers to clients, than they really want.\n\n" +
		"Setting this higher only on clients means they request more view IDs more often, for example twice in a row, as the pools received from the server don't contain enough numbers. ")]
	public class NetworkSetMinimumAllocatableViewIDs : FsmStateAction
	{		
		[Tooltip("The minimum number of ViewID numbers in the ViewID pool given to clients by the server. The default value is 100.")]
		public FsmInt minimumViewIDs;
		
		public override void Reset()
		{
			minimumViewIDs = 100;
		}

		public override void OnEnter()
		{
			Network.minimumAllocatableViewIDs  = minimumViewIDs.Value;
			
			Finish();
		}

	}
}

#endif