// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get connected player properties.")]
	public class NetworkGetConnectedPlayerProperties : FsmStateAction
	{	
		[RequiredField]
		[Tooltip("The player connection index.")]
		public FsmInt index;
		
		[ActionSection("Result")]

		[Tooltip("Get the IP address of this player.")]
		[UIHint(UIHint.Variable)]
		public FsmString IpAddress;
		
		[Tooltip("Get the port of this player.")]
		[UIHint(UIHint.Variable)]
		public FsmInt port;
		
		[Tooltip("Get the GUID for this player, used when connecting with NAT punchthrough.")]
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
			index = null;
			IpAddress = null;
			port = null;
			guid = null;
			externalIPAddress = null;
			externalPort = null;
		}

		public override void OnEnter()
		{
			getPlayerProperties();

			Finish();
		}
		
		void getPlayerProperties()
		{
			int _index = index.Value;
			
			 if (_index<0 || _index>= Network.connections.Length)
			{
				LogError("Player index out of range");
				return;
			}
			
			NetworkPlayer _player = Network.connections[_index];
/*			if (_player==null)
			{
				LogError("player not found");
				return;
			}*/
			
			IpAddress.Value = _player.ipAddress;
			port.Value = _player.port;
			guid.Value = _player.guid;
			externalIPAddress.Value = _player.externalIP;
			externalPort.Value = _player.externalPort;
		}
	}
}

#endif