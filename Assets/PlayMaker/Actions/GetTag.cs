// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets a Game Object's Tag and stores it in a String Variable.")]
	public class GetTag : FsmStateAction
	{
		[RequiredField]
		public FsmGameObject gameObject;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString storeResult;
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetTag();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoGetTag();
		}
		
		void DoGetTag()
		{
			if (gameObject.Value == null) return;
			
			storeResult.Value = gameObject.Value.tag;
		}
		
	}
}