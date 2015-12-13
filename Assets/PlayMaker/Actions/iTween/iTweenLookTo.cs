// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Rotates a GameObject to look at a supplied Transform or Vector3 over time.")]
	public class iTweenLookTo : iTweenFsmAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;
		
		[Tooltip("Look at a transform position.")]
		public FsmGameObject transformTarget;
		[Tooltip("A target position the GameObject will look at. If Transform Target is defined this is used as a local offset.")]
		public FsmVector3 vectorTarget;

		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;
		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;
		[Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
		public FsmFloat speed;
		[Tooltip("For the shape of the easing curve applied to the animation.")]
		public iTween.EaseType easeType = iTween.EaseType.linear;
		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType = iTween.LoopType.none;
		
		[Tooltip("Restricts rotation to the supplied axis only. Just put there strinc like 'x' or 'xz'")]
		public iTweenFsmAction.AxisRestriction axis = iTweenFsmAction.AxisRestriction.none;
			
		public override void Reset()
		{
			base.Reset();
			id = new FsmString{UseVariable = true};
			transformTarget = new FsmGameObject { UseVariable = true};
			vectorTarget = new FsmVector3 { UseVariable = true};
			time = 1f;
			delay = 0f;
			loopType = iTween.LoopType.none;
			speed = new FsmFloat { UseVariable = true };
			axis = iTweenFsmAction.AxisRestriction.none;
		}

		public override void OnEnter()
		{
			base.OnEnteriTween(gameObject);
			if(loopType != iTween.LoopType.none) base.IsLoop(true);
			DoiTween();	
		}
		
		public override void OnExit(){
			base.OnExitiTween(gameObject);;
		}
		
		void DoiTween()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			
			Vector3 pos = vectorTarget.IsNone ? Vector3.zero : vectorTarget.Value;
			if(!transformTarget.IsNone) 
				if(transformTarget.Value)
					pos = transformTarget.Value.transform.position + pos;
			
			itweenType = "rotate";
			iTween.LookTo(go, iTween.Hash(
			                              "looktarget", pos,
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
			                              "axis", axis == iTweenFsmAction.AxisRestriction.none ? "" : System.Enum.GetName(typeof(iTweenFsmAction.AxisRestriction), axis)
			                              ));
			}
	}
}