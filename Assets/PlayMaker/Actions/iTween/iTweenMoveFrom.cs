// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Instantly changes a GameObject's position to a supplied destination then returns it to it's starting position over time.")]
	public class iTweenMoveFrom : iTweenFsmAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;
		
		[Tooltip("Move From a transform rotation.")]
		public FsmGameObject transformPosition;
		[Tooltip("The position the GameObject will animate from. If Transform Position is defined this is used as a local offset.")]
		public FsmVector3 vectorPosition;
		
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

		[ActionSection("LookAt")]
		[Tooltip("Whether or not the GameObject will orient to its direction of travel. False by default.")]
		public FsmBool orientToPath;
//		[Tooltip("For how much of a percentage (from 0 to 1) to look ahead on a path to influence how strict 'orienttopath' is and how much the object will anticipate each curve. 0 by default")]
//		public FsmFloat lookAhead;
		[Tooltip("A target object the GameObject will look at.")]
		public FsmGameObject lookAtObject;
		[Tooltip("A target position the GameObject will look at.")]
		public FsmVector3 lookAtVector;
		[Tooltip("The time in seconds the object will take to look at either the Look At Target or Orient To Path. 0 by default")]
		public FsmFloat lookTime;
		[Tooltip("Restricts rotation to the supplied axis only.")]
		public iTweenFsmAction.AxisRestriction axis = iTweenFsmAction.AxisRestriction.none;
		
				
		public override void Reset()
		{
			base.Reset();
			id = new FsmString{UseVariable = true};
			transformPosition = new FsmGameObject { UseVariable = true};
			vectorPosition = new FsmVector3 { UseVariable = true };
			time = 1f;
			delay = 0f;
			loopType = iTween.LoopType.none;
			speed = new FsmFloat { UseVariable = true };
			space = Space.World;
			orientToPath = new FsmBool { Value = true };
			lookAtObject = new FsmGameObject { UseVariable = true};
			lookAtVector = new FsmVector3 { UseVariable = true };
			lookTime = 0f;
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

			Vector3 pos = vectorPosition.IsNone ? Vector3.zero : vectorPosition.Value;
			if(!transformPosition.IsNone) 
				if(transformPosition.Value)
					pos = (space == Space.World || go.transform.parent == null) ? transformPosition.Value.transform.position + pos : go.transform.parent.InverseTransformPoint(transformPosition.Value.transform.position) + pos;
					
			System.Collections.Hashtable hash = new System.Collections.Hashtable();
			hash.Add("position", pos);
			hash.Add(speed.IsNone ? "time" : "speed", speed.IsNone ? time.IsNone ? 1f : time.Value : speed.Value);
			hash.Add("delay", delay.IsNone ? 0f : delay.Value);
			hash.Add("easetype", easeType);
			hash.Add("looptype", loopType);
			hash.Add("oncomplete", "iTweenOnComplete");
			hash.Add("oncompleteparams", itweenID);
			hash.Add("onstart", "iTweenOnStart");
			hash.Add("onstartparams", itweenID);
			hash.Add("ignoretimescale", realTime.IsNone ? false : realTime.Value);
			hash.Add("islocal", space == Space.Self);
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
			iTween.MoveFrom(go, hash);
		}
	}
}