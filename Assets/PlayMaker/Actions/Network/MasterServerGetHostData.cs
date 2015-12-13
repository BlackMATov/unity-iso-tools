// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get host data from the master server.")]
	public class MasterServerGetHostData : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The index into the MasterServer Host List")]
		public FsmInt hostIndex;
		
		//[ActionSection("Result")]		
		
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

		public override void Reset()
		{
			hostIndex = null;
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
			GetHostData();

			Finish();
		}
				
		void GetHostData()
		{			
			int _count = MasterServer.PollHostList().Length;
			
			int _index = hostIndex.Value;
			
			if (_index<0 || _index>=_count)
			{
				LogError("MasterServer Host index out of range!");
				return;
			}
			
			HostData _hostData = MasterServer.PollHostList()[_index];
			
			if (_hostData == null)
			{
				LogError("MasterServer HostData could not found at index "+_index);
				return;
			}
			
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
		}
	}
}

#endif