// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Sets the current Time of an Animation, Normalize time means 0 (start) to 1 (end); useful if you don't care about the exact time. Check Every Frame to update the time continuosly.")]
	public class SetAnimationTime : ComponentAction<Animation>
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		[UIHint(UIHint.Animation)]
		public FsmString animName;
		public FsmFloat time;
		public bool normalized;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
			time = null;
			normalized = false;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetAnimationTime(gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value);
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoSetAnimationTime(gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value);
		}

		void DoSetAnimationTime(GameObject go)
		{
		    if (!UpdateCache(go))
		    {
		        return;
		    }

			animation.Play(animName.Value);

			var anim = animation[animName.Value];
			if (anim == null)
			{
				LogWarning("Missing animation: " + animName.Value);
				return;
			}

			if (normalized)
			{
				anim.normalizedTime = time.Value;
			}
			else
			{
				anim.time = time.Value;
			}
			
			// TODO: need to do this?
		    if (everyFrame)
		    {
		        anim.speed = 0;
		    }
		}

	}
}