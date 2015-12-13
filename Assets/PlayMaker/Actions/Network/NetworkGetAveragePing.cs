// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the last average ping time to the given player in milliseconds. \n" +
		"If the player can't be found -1 will be returned. Pings are automatically sent out every couple of seconds.")]
	public class NetworkGetPlayerPing : FsmStateAction
	{
		[ActionSection("Setup")]
		
		[RequiredField]
		[Tooltip("The Index of the player in the network connections list.")]
		[UIHint(UIHint.Variable)]
		public FsmInt playerIndex;
		
		[Tooltip("The player reference is cached, that is if the connections list changes, the player reference remains.")]
		public bool cachePlayerReference = true;
		
		public bool everyFrame;
		
		[ActionSection("Result")]

		[RequiredField]
		[Tooltip("Get the last average ping time to the given player in milliseconds.")]
		[UIHint(UIHint.Variable)]
		public FsmInt averagePing;
		
		[Tooltip("Event to send if the player can't be found. Average Ping is set to -1.")]
		public FsmEvent PlayerNotFoundEvent;
		
		[Tooltip("Event to send if the player is found (pings back).")]
		public FsmEvent PlayerFoundEvent;		
		
		
		private NetworkPlayer _player;
		
		public override void Reset()
		{
			playerIndex = null;
			averagePing = null;
			PlayerNotFoundEvent = null;
			PlayerFoundEvent = null;
			cachePlayerReference = true;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			if (cachePlayerReference)
			{
				_player = Network.connections[playerIndex.Value];
			}
			
			GetAveragePing();
			
			if(!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			GetAveragePing();
		}
		
		void GetAveragePing()
		{
		
			if (!cachePlayerReference)
			{
				_player = Network.connections[playerIndex.Value];
			}
			
			int _averagePing = Network.GetAveragePing(_player);
			if (!averagePing.IsNone)
			{
				averagePing.Value = _averagePing;
			}
			
			if (_averagePing ==-1 && PlayerNotFoundEvent != null)
			{
				Fsm.Event(PlayerNotFoundEvent);
			}
			
			if (_averagePing!=-1 && PlayerFoundEvent !=null)
			{
				Fsm.Event(PlayerFoundEvent);
			}
					
		}
	}
}

#endif