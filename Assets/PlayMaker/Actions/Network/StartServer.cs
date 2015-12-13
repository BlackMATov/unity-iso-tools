// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Start a server.")]
	public class StartServer : FsmStateAction
	{	
		[RequiredField]
		[Tooltip("The number of allowed incoming connections/number of players allowed in the game.")]
		public FsmInt connections;
		
		[RequiredField]
		[Tooltip("The port number we want to listen to.")]
		public FsmInt listenPort;

		[Tooltip("Sets the password for the server. This must be matched in the NetworkConnect action.")]
		public FsmString incomingPassword;

		[Tooltip("Sets the NAT punchthrough functionality.")]
		public FsmBool useNAT;
		
		[Tooltip("Unity handles the network layer by providing secure connections if you wish to use them. \n" +
		 	"Most games will want to use secure connections. " +
		 	"However, they add up to 15 bytes per packet and take time to compute so you may wish to limit usage to deployed games only.")]
		public FsmBool useSecurityLayer;

		[Tooltip("Run the server in the background, even if it doesn't have focus.")]
		public FsmBool runInBackground;

		[ActionSection("Errors")]

		[Tooltip("Event to send in case of an error creating the server.")]
		public FsmEvent errorEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the error string in a variable.")]
		public FsmString errorString;

		public override void Reset()
		{
			connections = 32;
			listenPort = 25001;
			incomingPassword = "";
			errorEvent = null;
			errorString = null;
			useNAT = false;
			useSecurityLayer = false;
			runInBackground = true;
		}

		public override void OnEnter()
		{
			//var useNAT = !Network.HavePublicAddress();

			Network.incomingPassword = incomingPassword.Value;
			
			if (useSecurityLayer.Value){
				Network.InitializeSecurity();
			}

			if (runInBackground.Value)
			{
				Application.runInBackground = true;
			}

			var error = Network.InitializeServer(connections.Value, listenPort.Value, useNAT.Value);

			if (error != NetworkConnectionError.NoError)
			{
				errorString.Value = error.ToString();
				LogError(errorString.Value);
				Fsm.Event(errorEvent);
			}

			Finish();
		}
	}
}

#endif