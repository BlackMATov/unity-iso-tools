// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets the pressed state of a Key.")]
	public class GetKey : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The key to test.")]
		public KeyCode key;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store if the key is down (True) or up (False).")]
		public FsmBool storeResult;
		
		[Tooltip("Repeat every frame. Useful if you're waiting for a key press/release.")]
		public bool everyFrame;
		
		public override void Reset()
		{
			key = KeyCode.None;
			storeResult = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoGetKey();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		

		public override void OnUpdate()
		{
			DoGetKey();
		}
		
		void DoGetKey()
		{
			storeResult.Value = Input.GetKey(key);
		}
		
	}
}