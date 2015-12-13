// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets the top most parent of the Game Object.\nIf the game object has no parent, returns itself.")]
	public class GetRoot : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeRoot;

		public override void Reset()
		{
			gameObject = null;
			storeRoot = null;
		}

		public override void OnEnter()
		{
			DoGetRoot();
			
			Finish();
		}

		void DoGetRoot()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			
			storeRoot.Value = go.transform.root.gameObject;
		}
	}
}