// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Plays a Random Animation on a Game Object. You can set the relative weight of each animation to control how often they are selected.")]
	public class PlayRandomAnimation : ComponentAction<Animation>
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		[Tooltip("Game Object to play the animation on.")]
		public FsmOwnerDefault gameObject;
		
		[CompoundArray("Animations", "Animation", "Weight")]
		
		[UIHint(UIHint.Animation)]
		public FsmString[] animations;
		
		[HasFloatSlider(0, 1)]
		public FsmFloat[] weights;
		
		[Tooltip("How to treat previously playing animations.")]
		public PlayMode playMode;

		[HasFloatSlider(0f, 5f)]
		[Tooltip("Time taken to blend to this animation.")]
		public FsmFloat blendTime;

		[Tooltip("Event to send when the animation is finished playing. NOTE: Not sent with Loop or PingPong wrap modes!")]
		public FsmEvent finishEvent;

		[Tooltip("Event to send when the animation loops. If you want to send this event to another FSM use Set Event Target. NOTE: This event is only sent with Loop and PingPong wrap modes.")]
		public FsmEvent loopEvent;

		[Tooltip("Stop playing the animation when this state is exited.")]
		public bool stopOnExit;

		private AnimationState anim;
		private float prevAnimtTime;

		public override void Reset()
		{
			gameObject = null;
			animations = new FsmString[0];
			weights = new FsmFloat[0];
			playMode = PlayMode.StopAll;
			blendTime = 0.3f;
			finishEvent = null;
			loopEvent = null;
			stopOnExit = false;
		}

		public override void OnEnter()
		{
			DoPlayRandomAnimation();
		}

		void DoPlayRandomAnimation()
		{
			if (animations.Length > 0)
			{
				var randomIndex = ActionHelpers.GetRandomWeightedIndex(weights);

				if (randomIndex != -1)
				{
					DoPlayAnimation(animations[randomIndex].Value);
				}
			}
		}

		void DoPlayAnimation(string animName)
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (!UpdateCache(go))
			{
				Finish();
				return;
			}

			if (string.IsNullOrEmpty(animName))
			{
				LogWarning("Missing animName!");
				Finish();
				return;
			}

			anim = animation[animName];

			if (anim == null)
			{
				LogWarning("Missing animation: " + animName);
				Finish();
				return;
			}

			var time = blendTime.Value;
			if (time < 0.001f)
			{
				animation.Play(animName, playMode);
			}
			else
			{
				animation.CrossFade(animName, time, playMode);
			}

			prevAnimtTime = anim.time;
		}

		public override void OnUpdate()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null || anim == null)
			{
				return;
			}

			if (!anim.enabled || (anim.wrapMode == WrapMode.ClampForever && anim.time > anim.length))
			{
				Fsm.Event(finishEvent);
				Finish();
			}

			if (anim.wrapMode != WrapMode.ClampForever && anim.time > anim.length && prevAnimtTime < anim.length)
			{
				Fsm.Event(loopEvent);
			}
		}

		public override void OnExit()
		{
			if (stopOnExit)
			{
				StopAnimation();
			}
		}

		void StopAnimation()
		{
			if (animation != null)
			{
				animation.Stop(anim.name);
			}
		}

	}
}