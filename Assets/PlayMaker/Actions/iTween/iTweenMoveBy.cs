// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Adds the supplied vector to a GameObject's position.")]
	public class iTweenMoveBy : iTweenFsmAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;
		
		[RequiredField]
		[Tooltip("The vector to add to the GameObject's position.")]
		public FsmVector3 vector;
		
		[Tooltip("For the time in seconds the animation will take to complete.")]
		public FsmFloat time;
		[Tooltip("For the time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;
		[Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
		public FsmFloat speed;
		[Tooltip("For the shape of the easing curve applied to the animation.")]
		public iTween.EaseType easeType = iTween.EaseType.linear;
		[Tooltip("For the type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType = iTween.LoopType.none;
		
		public Space space = Space.World;
		[ActionSection("LookAt")]
		[Tooltip("For whether or not the GameObject will orient to its direction of travel. False by default.")]
		public FsmBool orientToPath;
//		[Tooltip("For how much of a percentage (from 0 to 1) to look ahead on a path to influence how strict 'orienttopath' is and how much the object will anticipate each curve. 0 by default")]
//		public FsmFloat lookAhead;
		[Tooltip("For a target the GameObject will look at.")]
		public FsmGameObject lookAtObject;
		[Tooltip("For a target the GameObject will look at.")]
		public FsmVector3 lookAtVector;
		[Tooltip("For the time in seconds the object will take to look at either the 'looktarget' or 'orienttopath'. 0 by default")]
		public FsmFloat lookTime;
		[Tooltip("Restricts rotation to the supplied axis only. Just put there strinc like 'x' or 'xz'")]
		public iTweenFsmAction.AxisRestriction axis = iTweenFsmAction.AxisRestriction.none;
		
				
		public override void Reset()
		{
			base.Reset();
			id = new FsmString{UseVariable = true};
			time = 1f;
			delay = 0f;
			loopType = iTween.LoopType.none;
			vector = new FsmVector3 { UseVariable = true };
			speed = new FsmFloat { UseVariable = true };
			space = Space.World;
			orientToPath = false;
			lookAtObject = new FsmGameObject { UseVariable = true};
			lookAtVector = new FsmVector3 { UseVariable = true };
			lookTime = 0f;// new FsmFloat { UseVariable = true };
			axis = iTweenFsmAction.AxisRestriction.none;
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
					
			System.Collections.Hashtable hash = new System.Collections.Hashtable();
			hash.Add("amount", vector.IsNone ? Vector3.zero : vector.Value);
			hash.Add(speed.IsNone ? "time" : "speed", speed.IsNone ? time.IsNone ? 1f : time.Value : speed.Value);
			hash.Add("delay", delay.IsNone ? 0f : delay.Value);
			hash.Add("easetype", easeType);
			hash.Add("looptype", loopType);
			hash.Add("oncomplete", "iTweenOnComplete");
			hash.Add("oncompleteparams", itweenID);
			hash.Add("onstart", "iTweenOnStart");
			hash.Add("onstartparams", itweenID);
			hash.Add("ignoretimescale", realTime.IsNone ? false : realTime.Value);
			hash.Add("space", space);
			hash.Add("name", id.IsNone ? "" : id.Value);
			hash.Add("axis", axis == iTweenFsmAction.AxisRestriction.none ? "" : System.Enum.GetName(typeof(iTweenFsmAction.AxisRestriction), axis));
			if(!orientToPath.IsNone) {
				hash.Add("orienttopath", orientToPath.Value);
				
			}
			if(!lookAtObject.IsNone) {
				hash.Add("looktarget", lookAtVector.IsNone ? lookAtObject.Value.transform.position : lookAtObject.Value.transform.position + lookAtVector.Value);
			} else {
				if(!lookAtVector.IsNone) {
					hash.Add("looktarget", lookAtVector.Value);
				} 
			}
			if(!lookAtObject.IsNone || !lookAtVector.IsNone){
				hash.Add("looktime", lookTime.IsNone ? 0f : lookTime.Value);
			}
								
			itweenType = "move";
			iTween.MoveBy(go, hash);
		}
	}
}