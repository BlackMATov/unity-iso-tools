// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Changes a GameObject's scale over time.")]
	public class iTweenScaleTo: iTweenFsmAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;
		
		[Tooltip("Scale To a transform scale.")]
		public FsmGameObject transformScale;
		[Tooltip("A scale vector the GameObject will animate To.")]
		public FsmVector3 vectorScale;
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
				
		public override void Reset()
		{
			base.Reset();
			id = new FsmString{UseVariable = true};
			transformScale = new FsmGameObject { UseVariable = true};
			vectorScale = new FsmVector3 { UseVariable = true };
			time = 1f;
			delay = 0f;
			loopType = iTween.LoopType.none;
			speed = new FsmFloat { UseVariable = true };
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
			
			// init position
			
			Vector3 scl = vectorScale.IsNone ? Vector3.zero : vectorScale.Value;
			if(!transformScale.IsNone) 
				if(transformScale.Value)
					scl = transformScale.Value.transform.localScale + scl ;

			itweenType = "scale";
			iTween.ScaleTo(go, iTween.Hash(
			                              "scale", scl,
			                              "name", id.IsNone ? "" : id.Value, 
			                              speed.IsNone ? "time" : "speed", speed.IsNone ? time.IsNone ? 1f : time.Value : speed.Value,
			                              "delay", delay.IsNone ? 0f : delay.Value,
			                              "easetype", easeType,
			                              "looptype", loopType,
			                              "oncomplete", "iTweenOnComplete",
			                              "oncompleteparams", itweenID,
			                              "onstart", "iTweenOnStart",
			                              "onstartparams", itweenID,
			                              "ignoretimescale", realTime.IsNone ? false : realTime.Value  
			                              )); 
		}
	}
}