// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the network OnPlayerConnected or OnPlayerDisConnected message player info.")]
	public class NetworkGetMessagePlayerProperties : FsmStateAction
	{		
		
		[Tooltip("Get the IP address of this connected player.")]
		[UIHint(UIHint.Variable)]
		public FsmString IpAddress;
		
		[Tooltip("Get the port of this connected player.")]
		[UIHint(UIHint.Variable)]
		public FsmInt port;
		
		[Tooltip("Get the GUID for this connected player, used when connecting with NAT punchthrough.")]
		[UIHint(UIHint.Variable)]
		public FsmString guid;
		
		[Tooltip("Get the external IP address of the network interface. This will only be populated after some external connection has been made.")]
		[UIHint(UIHint.Variable)]
		public FsmString externalIPAddress;
		
		[Tooltip("Get the external port of the network interface. This will only be populated after some external connection has been made.")]
		[UIHint(UIHint.Variable)]
		public FsmInt externalPort;

		
		public override void Reset()
		{
			IpAddress = null;
			port = null;
			guid = null;
			externalIPAddress = null;
			externalPort = null;
		}
		

		public override void OnEnter()
		{
			doGetOnPLayerConnectedProperties();
			Finish();
		}
		
		void doGetOnPLayerConnectedProperties()
		{
			NetworkPlayer _player = Fsm.EventData.Player;
			Debug.Log("hello "+_player.ipAddress);
			
			
			IpAddress.Value = _player.ipAddress;
			port.Value = _player.port;
			guid.Value = _player.guid;
			externalIPAddress.Value = _player.externalIP;
			externalPort.Value = _player.externalPort;
			
			Finish();
		}
		
	}
}

#endif