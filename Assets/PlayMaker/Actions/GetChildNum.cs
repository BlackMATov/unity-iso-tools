// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets the Child of a GameObject by Index.\nE.g., O to get the first child. HINT: Use this with an integer variable to iterate through children.")]
	public class GetChildNum : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject to search.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
        [Tooltip("The index of the child to find.")]
		public FsmInt childIndex;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the child in a GameObject variable.")]
		public FsmGameObject store;

		public override void Reset()
		{
			gameObject = null;
			childIndex = 0;
			store = null;
		}

		public override void OnEnter()
		{
			store.Value = DoGetChildNum(Fsm.GetOwnerDefaultTarget(gameObject));

			Finish();
		}

		GameObject DoGetChildNum(GameObject go)
		{
			return go == null ? null : go.transform.GetChild(childIndex.Value % go.transform.childCount).gameObject;
		}
	}
}