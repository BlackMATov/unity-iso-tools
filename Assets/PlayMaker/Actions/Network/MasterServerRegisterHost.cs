// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Register this server on the master server.\n\n" +
	 	"If the master server address information has not been changed the default Unity master server will be used.")]
	public class MasterServerRegisterHost : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The unique game type name.")]
		public FsmString gameTypeName;
		
		[RequiredField]
		[Tooltip("The game name.")]
		public FsmString gameName;
		
		[Tooltip("Optional comment")]
		public FsmString comment;
		
		public override void Reset()
		{
			gameTypeName = null;
			gameName = null;
			comment = null;
		}

		public override void OnEnter()
		{
			DoMasterServerRegisterHost();
			
			Finish();			
		}

		void DoMasterServerRegisterHost()
		{
			MasterServer.RegisterHost(gameTypeName.Value,gameName.Value,comment.Value);
		}
	}
}

#endif