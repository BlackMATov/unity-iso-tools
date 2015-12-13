// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets the value of a Game Object Variable.")]
	public class SetGameObject : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject variable;
		public FsmGameObject gameObject;
		public bool everyFrame;

		public override void Reset()
		{
			variable = null;
			gameObject = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			variable.Value = gameObject.Value;
			
			if (!everyFrame)
			{
				Finish();		
			}
		}

		public override void OnUpdate()
		{
			variable.Value = gameObject.Value;
		}
	}
}