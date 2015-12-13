// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Blends an Animation towards a Target Weight over a specified Time.\nOptionally sends an Event when finished.")]
	public class BlendAnimation : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		[Tooltip("The GameObject to animate.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[UIHint(UIHint.Animation)]
		[Tooltip("The name of the animation to blend.")]
		public FsmString animName;
		
		[RequiredField]
		[HasFloatSlider(0f, 1f)]
		[Tooltip("Target weight to blend to.")]
		public FsmFloat targetWeight;
		
		[RequiredField]
		[HasFloatSlider(0f, 5f)]
		[Tooltip("How long should the blend take.")]
		public FsmFloat time;
		
		[Tooltip("Event to send when the blend has finished.")]
		public FsmEvent finishEvent;

		// TODO: Delayed event doesn't handle speed changes etc.
		// Use Animation isPlaying instead?
		DelayedEvent delayedFinishEvent;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
			targetWeight = 1f;
			time = 0.3f;
			finishEvent = null;
		}

		public override void OnEnter()
		{
			DoBlendAnimation(gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value);
		}

		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(delayedFinishEvent))
			{
				Finish();
			}
		}

		void DoBlendAnimation(GameObject go)
		{
			if (go == null)
			{
				return;
			}

            var animation = go.GetComponent<Animation>();
			if (animation == null)
			{
				LogWarning("Missing Animation component on GameObject: " + go.name);
				Finish();
				return;
			}

			var anim = animation[animName.Value];

			if (anim == null)
			{
				LogWarning("Missing animation: " + animName.Value);
				Finish();
				return;
			}

			var timeValue = time.Value;
			animation.Blend(animName.Value, targetWeight.Value, timeValue);
			
			// TODO: doesn't work well with scaled time
			if (finishEvent != null)
			{
				delayedFinishEvent = Fsm.DelayedEvent(finishEvent, anim.length);
			}
			else
			{
				Finish();
			}
		}
	}
}