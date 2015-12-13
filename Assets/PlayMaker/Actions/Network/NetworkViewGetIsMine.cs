// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Test if the Network View is controlled by a GameObject.")]
	public class NetworkViewIsMine : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkView))]
		[Tooltip("The Game Object with the NetworkView attached.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("True if the network view is controlled by this object.")]
		public FsmBool isMine;
		
		[Tooltip("Send this event if the network view controlled by this object.")]
		public FsmEvent isMineEvent;
		
		[Tooltip("Send this event if the network view is NOT controlled by this object.")]
		public FsmEvent isNotMineEvent;
		
		private NetworkView _networkView;
		
		private void _getNetworkView()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_networkView =  go.GetComponent<NetworkView>();
		}
		
		public override void Reset()
		{
			gameObject = null;
			isMine = null;
			isMineEvent = null;
			isNotMineEvent = null;
		}

		public override void OnEnter()
		{
			_getNetworkView();
			
			checkIsMine();
			
			Finish();
		}
		
		void checkIsMine()
		{
			if (_networkView ==null)
			{
				return;	
			}
			
			bool _isMine = _networkView.isMine;
			isMine.Value = _isMine;
			
			if (_isMine )
			{
				if (isMineEvent!=null)
				{
					Fsm.Event(isMineEvent);
				}
			}
			else if (isNotMineEvent!=null)
			{
				Fsm.Event(isNotMineEvent);
			}
		}

	}
}

#endif