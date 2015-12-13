// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

// Unity 5.1 introduced a new networking library. 
// Unless we define PLAYMAKER_LEGACY_NETWORK old network actions are disabled
#if !(UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || PLAYMAKER_LEGACY_NETWORK)
#define UNITY_NEW_NETWORK
#endif

// Some platforms do not support networking (at least the old network library)
#if (UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)
#define PLATFORM_NOT_SUPPORTED
#endif

#if !(PLATFORM_NOT_SUPPORTED || UNITY_NEW_NETWORK || PLAYMAKER_NO_NETWORK)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Send an Fsm Event on a remote machine. Uses Unity RPC functions.")]
	public class SendRemoteEvent : ComponentAction<NetworkView>
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkView))]
		[Tooltip("The game object that sends the event.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The event you want to send.")]
		public FsmEvent remoteEvent;
		
		[Tooltip("Optional string data. Use 'Get Event Info' action to retrieve it.")]
		public FsmString stringData;

		[Tooltip("Option for who will receive an RPC.")]
		public RPCMode mode;
		
		public override void Reset()
		{
			gameObject = null;
			remoteEvent = null;
			mode = RPCMode.All;
			stringData = null;
			mode = RPCMode.All;
		}

		public override void OnEnter()
		{
			DoRemoteEvent();
			
			Finish();
		}

		void DoRemoteEvent()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (!UpdateCache(go))
			{
				return;
			}
	
			if (!stringData.IsNone && stringData.Value != "")
			{
				networkView.RPC("SendRemoteFsmEventWithData", mode, remoteEvent.Name,stringData.Value);
			}
			else
			{
				networkView.RPC("SendRemoteFsmEvent", mode, remoteEvent.Name);
			}		
		}
	}
}

#endif
