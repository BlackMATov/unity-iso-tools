// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Instantly changes a GameObject's Euler angles in degrees then returns it to it's starting rotation over time.")]
	public class iTweenRotateFrom : iTweenFsmAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;
		
		[Tooltip("A rotation from a GameObject.")]
		public FsmGameObject transformRotation;
		[Tooltip("A rotation vector the GameObject will animate from.")]
		public FsmVector3 vectorRotation;
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;
		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;
		[Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
		public FsmFloat speed;
		[Tooltip("The shape of the easing curve applied to the animation.")]
		public iTween.EaseType easeType = iTween.EaseType.linear;
		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType = iTween.LoopType.none;

		[Tooltip("Whether to animate in local or world space.")]
		public Space space = Space.World;
		
		public override void Reset()
		{
			base.Reset();
			id = new FsmString{UseVariable = true};
			transformRotation = new FsmGameObject { UseVariable = true};
			vectorRotation = new FsmVector3 { UseVariable = true };
			time = 1f;
			delay = 0f;
			loopType = iTween.LoopType.none;
			speed = new FsmFloat { UseVariable = true };
			space = Space.World;
		}

		public override void OnEnter()
		{
			base.OnEnteriTween(gameObject);
			if(loopType != iTween.LoopType.none) base.IsLoop(true);
			DoiTween();	
		}
		
		public override void OnExit(){
			base.OnExitiTween(gameObject);
		}
		
		void DoiTween()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			
			Vector3 rot = vectorRotation.IsNone ? Vector3.zero : vectorRotation.Value;
			if(!transformRotation.IsNone) 
				if(transformRotation.Value)
					rot = space == Space.World ? transformRotation.Value.transform.eulerAngles + rot : transformRotation.Value.transform.localEulerAngles + rot;
			
			itweenType = "rotate";
			iTween.RotateFrom(go, iTween.Hash(
			                              "rotation", rot,
			                              "name", id.IsNone ? "" : id.Value,    
			                              speed.IsNone ? "time" : "speed", speed.IsNone ? time.IsNone ? 1f : time.Value : speed.Value,
			                              "delay", delay.IsNone ? 0f : delay.Value,
			                              "easetype", easeType,
			                              "looptype", loopType,
			                              "oncomplete", "iTweenOnComplete",
			                              "oncompleteparams", itweenID,
			                              "onstart", "iTweenOnStart",
			                              "onstartparams", itweenID,
			                              "ignoretimescale", realTime.IsNone ? false : realTime.Value,
			                              "islocal", space == Space.Self
			                              ));
			}
	}
}