// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Check if this machine has a public IP address.")]
	public class NetworkHavePublicIpAddress : FsmStateAction
	{	
		[UIHint(UIHint.Variable)]
		[Tooltip("True if this machine has a public IP address")]
		public FsmBool havePublicIpAddress;

		[Tooltip("Event to send if this machine has a public IP address")]
		public FsmEvent publicIpAddressFoundEvent;

		[Tooltip("Event to send if this machine has no public IP address")]
		public FsmEvent publicIpAddressNotFoundEvent;

		public override void Reset()
		{
			havePublicIpAddress = null;
			publicIpAddressFoundEvent = null;
			publicIpAddressNotFoundEvent =null;			
		}

		public override void OnEnter()
		{
			
			bool _publicIpAddress = Network.HavePublicAddress();
			
			havePublicIpAddress.Value = _publicIpAddress;

			if (_publicIpAddress && publicIpAddressFoundEvent != null)
			{
				Fsm.Event(publicIpAddressFoundEvent);
			}
			else if (!_publicIpAddress && publicIpAddressNotFoundEvent != null)
			{
				Fsm.Event(publicIpAddressNotFoundEvent);
			}

			Finish();
		}
	}
}

#endif