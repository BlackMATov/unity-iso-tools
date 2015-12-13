// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets the Parent of a Game Object.")]
	public class GetParent : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeResult;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go != null)
				storeResult.Value = go.transform.parent == null ? null : go.transform.parent.gameObject;
			else
				storeResult.Value = null;
			
			Finish();
		}
	}
}