// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Rewinds the named animation.")]
	public class RewindAnimation : ComponentAction<Animation>
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.Animation)]
		public FsmString animName;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
		}

		public override void OnEnter()
		{
			DoRewindAnimation();

			Finish();
		}

		void DoRewindAnimation()
		{
			if (string.IsNullOrEmpty(animName.Value))
			{
				return;
			}

			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(go))
			{
                animation.Rewind(animName.Value);
			}
		}
	}
}