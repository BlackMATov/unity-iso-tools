// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets a Game Object's Name.")]
	public class SetName : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmString name;

		public override void Reset()
		{
			gameObject = null;
			name = null;
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
			
			go.name = name.Value;
		}

	}
}