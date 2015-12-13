// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.
/*
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Get the ViewId of a NetworkView.\n\nNOTE: The GameObject must have a NetworkView attached.")]
	public class NetworkViewGetViewID : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkView))]
		[Tooltip("The GameObject.\nNOTE: The Game Object must have a NetworkView attached.")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The view Id")]
		[UIHint(UIHint.Variable)]
		public FsmInt viewID;
		
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
			viewID = null;
		}

		public override void OnEnter()
		{
			_getNetworkView();
			
			getViewId();
			
			Finish();
		}
		
		void getViewId()
		{
			if (_networkView ==null)
			{
				return;	
			}
			
			viewID.Value = _networkView.viewID;
		}

	}
}
*/