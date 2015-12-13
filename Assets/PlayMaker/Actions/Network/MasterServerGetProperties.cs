// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the IP address, port, update rate and dedicated server flag of the master server and store in variables.")]
	public class MasterServerGetProperties : FsmStateAction
	{
	
		[Tooltip("The IP address of the master server.")]
		[UIHint(UIHint.Variable)]
		public FsmString ipAddress;
		
		[Tooltip("The connection port of the master server.")]
		[UIHint(UIHint.Variable)]
		public FsmInt port;
		
		[Tooltip("The minimum update rate for master server host information update. Default is 60 seconds")]
		[UIHint(UIHint.Variable)]
		public FsmInt updateRate;
		
		[Tooltip("Flag to report if this machine is a dedicated server.")]
		[UIHint(UIHint.Variable)]
		public FsmBool dedicatedServer;
		
		[Tooltip("Event sent if this machine is a dedicated server")]
		public FsmEvent isDedicatedServerEvent;
		
		[Tooltip("Event sent if this machine is not a dedicated server")]
		public FsmEvent isNotDedicatedServerEvent;
		
		public override void Reset()
		{
			ipAddress = null;
			port = null;
			updateRate = null;
			dedicatedServer = null;
			isDedicatedServerEvent = null;
			isNotDedicatedServerEvent = null;		
		}

		public override void OnEnter()
		{
			GetMasterServerProperties();
			
			Finish();			
		}

		void GetMasterServerProperties()
		{		
			ipAddress.Value = MasterServer.ipAddress;
			port.Value = MasterServer.port;
			updateRate.Value = MasterServer.updateRate;
			
			bool _dedicated = MasterServer.dedicatedServer;
			
			dedicatedServer.Value = _dedicated;
			
			if (_dedicated && isDedicatedServerEvent != null)
			{
				Fsm.Event(isDedicatedServerEvent);
			}
			
			if (!_dedicated && isNotDedicatedServerEvent !=null)
			{
				Fsm.Event(isNotDedicatedServerEvent);
			}
		}
	}
}

#endif