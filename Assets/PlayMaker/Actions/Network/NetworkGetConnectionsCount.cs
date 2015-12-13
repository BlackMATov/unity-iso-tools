// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the number of connected players.\n\nOn a client this returns 1 (the server).")]
	public class NetworkGetConnectionsCount : FsmStateAction
	{
		[Tooltip("Number of connected players.")]
		[UIHint(UIHint.Variable)]
		public FsmInt connectionsCount;
		
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			connectionsCount = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			connectionsCount.Value = Network.connections.Length;
			
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			connectionsCount.Value = Network.connections.Length;
		}

	}
}

#endif