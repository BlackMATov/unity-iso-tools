// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Plays an Animation on a Game Object. You can add named animation clips to the object in the Unity editor, or with the Add Animation Clip action.")]
	public class PlayAnimation : ComponentAction<Animation>
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		[Tooltip("Game Object to play the animation on.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Animation)]
		[Tooltip("The name of the animation to play.")]
		public FsmString animName;
		
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
			animName = null;
			playMode = PlayMode.StopAll;
			blendTime = 0.3f;
			finishEvent = null;
			loopEvent = null;
			stopOnExit = false;
		}

		public override void OnEnter()
		{
			DoPlayAnimation();
		}

		void DoPlayAnimation()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (!UpdateCache(go))
			{
				Finish();
				return;
			}
			
			if (string.IsNullOrEmpty(animName.Value))
			{
				LogWarning("Missing animName!");
				Finish();
				return;
			}

			anim = animation[animName.Value];

			if (anim == null)
			{
				LogWarning("Missing animation: " + animName.Value);
				Finish();
				return;
			}

			var time = blendTime.Value;
			if (time < 0.001f)
			{
				animation.Play(animName.Value, playMode);
			}
			else
			{
				animation.CrossFade(animName.Value, time, playMode);
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
				animation.Stop(animName.Value);
			}
		}
	}
}