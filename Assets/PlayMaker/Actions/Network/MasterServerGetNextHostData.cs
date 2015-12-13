// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the next host data from the master server. \nEach time this action is called it gets the next connected host." +
	 	"This lets you quickly loop through all the connected hosts to get information on each one.")]
	public class MasterServerGetNextHostData : FsmStateAction
	{
		
		[ActionSection("Set up")]
		
		[Tooltip("Event to send for looping.")]
		public FsmEvent loopEvent;
		
		[Tooltip("Event to send when there are no more hosts.")]
		public FsmEvent finishedEvent;
		
		[ActionSection("Result")]
		
		[Tooltip("The index into the MasterServer Host List")]
		[UIHint(UIHint.Variable)]
		public FsmInt index;
		
		[Tooltip("Does this server require NAT punchthrough?")]
		[UIHint(UIHint.Variable)]
		public FsmBool useNat;
		
		[Tooltip("The type of the game (e.g., 'MyUniqueGameType')")]
		[UIHint(UIHint.Variable)]
		public FsmString gameType;
		
		[Tooltip("The name of the game (e.g., 'John Does's Game')")]
		[UIHint(UIHint.Variable)]
		public FsmString gameName;
		
	 	[Tooltip("Currently connected players")]
		[UIHint(UIHint.Variable)]
		public FsmInt connectedPlayers;
		
		[Tooltip("Maximum players limit")]
		[UIHint(UIHint.Variable)]
		public FsmInt playerLimit;
		
		[Tooltip("Server IP address.")]
		[UIHint(UIHint.Variable)]
		public FsmString ipAddress;
		
		[Tooltip("Server port")]
		[UIHint(UIHint.Variable)]
		public FsmInt port;
	 
		[Tooltip("Does the server require a password?")]
		[UIHint(UIHint.Variable)]
		public FsmBool passwordProtected;
		
		[Tooltip("A miscellaneous comment (can hold data)")]
		[UIHint(UIHint.Variable)]
		public FsmString comment;
		 
		[Tooltip("The GUID of the host, needed when connecting with NAT punchthrough.")]
		[UIHint(UIHint.Variable)]
		public FsmString guid;

		// increment a child index as we loop through children
		private int nextItemIndex;

		// are we there yet?
		private bool noMoreItems;
		
		public override void Reset()
		{
			finishedEvent = null;
			loopEvent = null;
			
			index = null;
			useNat = null;
			gameType = null;
			gameName = null;
			connectedPlayers = null;
			playerLimit = null;
			ipAddress = null;
			port = null;
			passwordProtected = null;
			comment = null;
			guid = null;
		}
		
		public override void OnEnter()
		{
		
			DoGetNextHostData();
			
			Finish();
		}
				
		void DoGetNextHostData()
		{		
			// no more items?

			if (nextItemIndex >= MasterServer.PollHostList().Length)
			{
				nextItemIndex = 0;
				Fsm.Event(finishedEvent);
				return;
			}

			// get next item	
			HostData _hostData = MasterServer.PollHostList()[nextItemIndex];

			index.Value = nextItemIndex;
			useNat.Value = _hostData.useNat;
			gameType.Value = _hostData.gameType;
			gameName.Value = _hostData.gameName;
			connectedPlayers.Value = _hostData.connectedPlayers;
			playerLimit.Value = _hostData.playerLimit;
			ipAddress.Value = _hostData.ip[0];
			port.Value = _hostData.port;
			passwordProtected.Value = _hostData.passwordProtected;
			comment.Value = _hostData.comment;
			guid.Value = _hostData.guid;
			
			// no more items?

			if (nextItemIndex >= MasterServer.PollHostList().Length)
			{
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