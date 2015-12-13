// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the next connected player properties. \nEach time this action is called it gets the next child of a GameObject." +
	 	"This lets you quickly loop through all the connected player to perform actions on them.")]
	public class NetworkGetNextConnectedPlayerProperties : FsmStateAction
	{	
		
		[ActionSection("Set up")]

		[Tooltip("Event to send for looping.")]
		public FsmEvent loopEvent;
		
		[Tooltip("Event to send when there are no more children.")]
		public FsmEvent finishedEvent;
		
		[ActionSection("Result")]
	
		[Tooltip("The player connection index.")]
		[UIHint(UIHint.Variable)]
		public FsmInt index;
		
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
		
		
		// increment a child index as we loop through children
		private int nextItemIndex;
		
		public override void Reset()
		{	
			finishedEvent = null;
			loopEvent = null;
			
			index = null;
			IpAddress = null;
			port = null;
			guid = null;
			externalIPAddress = null;
			externalPort = null;
		}
		

		public override void OnEnter()
		{
			DoGetNextPlayerProperties();
			
			Finish();
		}
		
		void DoGetNextPlayerProperties()
		{
			
			// no more items?

			if (nextItemIndex >= Network.connections.Length)
			{
			//	Debug.Log("no more players to loop: "+ nextItemIndex + " total "+Network.connections.Length);
				Fsm.Event(finishedEvent);
				nextItemIndex = 0;
				return;
			}

			// get next item
			NetworkPlayer _player = Network.connections[nextItemIndex];
			
			index.Value = nextItemIndex;
			
			IpAddress.Value = _player.ipAddress;
			port.Value = _player.port;
			guid.Value = _player.guid;
			externalIPAddress.Value = _player.externalIP;
			externalPort.Value = _player.externalPort;
			
			
			
			// check again if we reached the end of the list.
			if (nextItemIndex >= Network.connections.Length)
			{
				//Debug.Log("no more players to loop: "+ nextItemIndex + " total "+Network.connections.Length);
				Fsm.Event(finishedEvent);
				nextItemIndex = 0;
				return;
			}
			
			// iterate to next 
			nextItemIndex++;
				
			if (loopEvent!=null){
				Fsm.Event(loopEvent);
			}
			
		}
	
	}
}

#endif