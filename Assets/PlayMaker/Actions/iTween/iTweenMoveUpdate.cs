// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Similar to MoveTo but incredibly less expensive for usage inside the Update function or similar looping situations involving a 'live' set of changing values. Does not utilize an EaseType.")]
	public class iTweenMoveUpdate : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Move From a transform rotation.")]
		public FsmGameObject transformPosition;
		[Tooltip("The position the GameObject will animate from.  If transformPosition is set, this is used as an offset.")]
		public FsmVector3 vectorPosition;
		
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

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
		
		System.Collections.Hashtable hash;			
		GameObject go;
				
		public override void Reset()
		{
			transformPosition = new FsmGameObject { UseVariable = true};
			vectorPosition = new FsmVector3 { UseVariable = true };
			time = 1f;
			space = Space.World;
			orientToPath = new FsmBool { Value = true };
			lookAtObject = new FsmGameObject { UseVariable = true};
			lookAtVector = new FsmVector3 { UseVariable = true };
			lookTime = 0f;
			axis = iTweenFsmAction.AxisRestriction.none;
		}

		public override void OnEnter()
		{
			hash = new System.Collections.Hashtable();
			go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) {
				Finish();
				return;
			}

			if(transformPosition.IsNone){
				hash.Add("position", vectorPosition.IsNone ? Vector3.zero : vectorPosition.Value);
			} else {
				if(vectorPosition.IsNone){
					hash.Add("position", transformPosition.Value.transform);
				} else {
					if (space == Space.World || go.transform.parent == null) hash.Add("position", transformPosition.Value.transform.position + vectorPosition.Value);
					else hash.Add("position", go.transform.parent.InverseTransformPoint(transformPosition.Value.transform.position) + vectorPosition.Value);
				}
				
			}
			hash.Add("time", time.IsNone ? 1f : time.Value);
			hash.Add("islocal", space == Space.Self);
			hash.Add("axis", axis == iTweenFsmAction.AxisRestriction.none ? "" : System.Enum.GetName(typeof(iTweenFsmAction.AxisRestriction), axis));
			if(!orientToPath.IsNone) {
				hash.Add("orienttopath", orientToPath.Value);
				
			}
			if(lookAtObject.IsNone) {
				if(!lookAtVector.IsNone){
					hash.Add("looktarget", lookAtVector.Value);
				}
			} else {	
				hash.Add("looktarget", lookAtObject.Value.transform);
			} 
			
			if(!lookAtObject.IsNone || !lookAtVector.IsNone){
				hash.Add("looktime", lookTime.IsNone ? 0f : lookTime.Value);
			}
			DoiTween();	
		}
		
		public override void OnUpdate(){
			hash.Remove("position");
			if(transformPosition.IsNone){
				hash.Add("position", vectorPosition.IsNone ? Vector3.zero : vectorPosition.Value);
			} else {
				if(vectorPosition.IsNone){
					hash.Add("position", transformPosition.Value.transform);
				} else {
					if(space == Space.World) hash.Add("position", transformPosition.Value.transform.position + vectorPosition.Value);
					else  hash.Add("position", transformPosition.Value.transform.localPosition + vectorPosition.Value);
				}
				
			}
			DoiTween();	
		}
		
		public override void OnExit(){
			
		}
		
		void DoiTween()
		{
			iTween.MoveUpdate(go, hash);
		}
	}
}