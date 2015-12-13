// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets a Game Object's Tag.")]
	public class SetTag : FsmStateAction
	{
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.Tag)]
		public FsmString tag;

		public override void Reset()
		{
			gameObject = null;
			tag = "Untagged";
		}

		public override void OnEnter()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			
			if (go != null)
				go.tag = tag.Value;
			
			Finish();
		}
	}
}