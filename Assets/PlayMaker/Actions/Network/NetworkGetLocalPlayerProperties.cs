// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the local network player properties")]
	public class NetworkGetLocalPlayerProperties : FsmStateAction
	{		
		[Tooltip("The IP address of this player.")]
		[UIHint(UIHint.Variable)]
		public FsmString IpAddress;
		
		[Tooltip("The port of this player.")]
		[UIHint(UIHint.Variable)]
		public FsmInt port;
		
		[Tooltip("The GUID for this player, used when connecting with NAT punchthrough.")]
		[UIHint(UIHint.Variable)]
		public FsmString guid;
		
		[Tooltip("The external IP address of the network interface. This will only be populated after some external connection has been made.")]
		[UIHint(UIHint.Variable)]
		public FsmString externalIPAddress;
		
		[Tooltip("Returns the external port of the network interface. This will only be populated after some external connection has been made.")]
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
			IpAddress.Value = Network.player.ipAddress;
			port.Value = Network.player.port;
			guid.Value = Network.player.guid;
			externalIPAddress.Value = Network.player.externalIP;
			externalPort.Value = Network.player.externalPort;
			
			Finish();
		}

	}
}

#endif