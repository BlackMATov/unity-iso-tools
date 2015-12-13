// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Request a host list from the master server.\n\n" +
		"Use MasterServer Get Host Data to get info on each host in the host list.")]
	public class MasterServerRequestHostList : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The unique game type name.")]
		public FsmString gameTypeName;
				
		[Tooltip("Event sent when the host list has arrived. NOTE: The action will not Finish until the host list arrives.")]
		public FsmEvent HostListArrivedEvent;
		
		public override void Reset()
		{
			gameTypeName = null;
			HostListArrivedEvent = null;
		}

		public override void OnEnter()
		{
			DoMasterServerRequestHost();
		}
		
		public override void OnUpdate()
		{
			WatchServerRequestHost();
		}

		void DoMasterServerRequestHost()
		{
			MasterServer.ClearHostList();
			
			MasterServer.RequestHostList(gameTypeName.Value);
		}
		
		void WatchServerRequestHost()
		{
			if (MasterServer.PollHostList().Length != 0)
			{
				Fsm.Event(HostListArrivedEvent);
				Finish();
			}			
		}
	}
}

#endif