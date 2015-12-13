// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Set the send rate for all networkViews. Default is 15")]
	public class NetworkSetSendRate : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The send rate for all networkViews")]
		public FsmFloat sendRate;

		public override void Reset()
		{
			sendRate = 15f;
		}

		public override void OnEnter()
		{
			DoSetSendRate();
			
			Finish();
		}

		void DoSetSendRate()
		{
			Network.sendRate = sendRate.Value;
		}
	}
}

#endif