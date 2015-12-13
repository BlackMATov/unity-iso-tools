// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets a Random Child of a Game Object.")]
	public class GetRandomChild : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeResult;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoGetRandomChild();
			Finish();
		}

		void DoGetRandomChild()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			
			int childCount = go.transform.childCount;
			if (childCount == 0) return;
			
			storeResult.Value = go.transform.GetChild(Random.Range(0, childCount)).gameObject;
		}
	}
}