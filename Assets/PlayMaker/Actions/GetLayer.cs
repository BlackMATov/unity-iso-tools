// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets a Game Object's Layer and stores it in an Int Variable.")]
	public class GetLayer : FsmStateAction
	{
		[RequiredField]
		public FsmGameObject gameObject;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt storeResult;
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetLayer();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoGetLayer();
		}
		
		void DoGetLayer()
		{
			if (gameObject.Value == null) return;
			
			storeResult.Value = gameObject.Value.layer;
		}
	}
}