// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets a Game Object's Layer.")]
	public class SetLayer : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.Layer)]
		public int layer;

		public override void Reset()
		{
			gameObject = null;
			layer = 0;
		}

		public override void OnEnter()
		{
			DoSetLayer();
			
			Finish();
		}

		void DoSetLayer()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

			go.layer = layer;
		}

	}
}