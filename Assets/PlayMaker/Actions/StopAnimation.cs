// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Stops all playing Animations on a Game Object. Optionally, specify a single Animation to Stop.")]
	public class StopAnimation : ComponentAction<Animation>
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		public FsmOwnerDefault gameObject;
		[Tooltip("Leave empty to stop all playing animations.")]
		[UIHint(UIHint.Animation)]
		public FsmString animName;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
		}

		public override void OnEnter()
		{
			DoStopAnimation();
			
			Finish();
		}

		void DoStopAnimation()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (!UpdateCache(go))
		    {
		        return;
		    }

            if (FsmString.IsNullOrEmpty(animName))
            {
                animation.Stop();
            }
            else
            {
                animation.Stop(animName.Value);
            }
		}

        /*
			public override string ErrorCheck()
			{
				return ErrorCheckHelpers.CheckAnimationSetup(gameObject.value);
			}*/
	}
}