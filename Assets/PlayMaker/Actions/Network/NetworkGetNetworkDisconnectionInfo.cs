// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the network OnDisconnectedFromServer.")]
	public class NetworkGetNetworkDisconnectionInfos : FsmStateAction
	{		
		
		[Tooltip("Disconnection label")]
		[UIHint(UIHint.Variable)]
		public FsmString disconnectionLabel;
		
		[Tooltip("The connection to the system has been lost, no reliable packets could be delivered.")]
		public FsmEvent lostConnectionEvent;
		[Tooltip("The connection to the system has been closed.")]
		public FsmEvent disConnectedEvent;

		
		public override void Reset ()
		{
			disconnectionLabel = null;
			lostConnectionEvent = null;
			disConnectedEvent = null;
			
		}
		
		
		public override void OnEnter ()
		{
			doGetNetworkDisconnectionInfo();
			
			Finish();
		}
		
		void doGetNetworkDisconnectionInfo()
		{
			NetworkDisconnection _networkDisconnectionInfo = Fsm.EventData.DisconnectionInfo;

			disconnectionLabel.Value = _networkDisconnectionInfo.ToString();
			
			switch (_networkDisconnectionInfo) {
			case NetworkDisconnection.Disconnected:
				if (disConnectedEvent != null) {
					Fsm.Event (disConnectedEvent);
				}
				break;
			case NetworkDisconnection.LostConnection:
				if (lostConnectionEvent != null) {
					Fsm.Event (lostConnectionEvent);
				}
				break;
			}
		}
		
	}
}

#endif