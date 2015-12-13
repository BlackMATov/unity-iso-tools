// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Rotates a GameObject to look at a supplied Transform or Vector3 over time.")]
	public class iTweenLookUpdate : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Look at a transform position.")]
		public FsmGameObject transformTarget;
		[Tooltip("A target position the GameObject will look at. If Transform Target is defined this is used as a look offset.")]
		public FsmVector3 vectorTarget;

		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;
				
		[Tooltip("Restricts rotation to the supplied axis only. Just put there strinc like 'x' or 'xz'")]
		public iTweenFsmAction.AxisRestriction axis = iTweenFsmAction.AxisRestriction.none;
		
		System.Collections.Hashtable hash;			
		GameObject go;
		
		public override void Reset()
		{
			transformTarget = new FsmGameObject { UseVariable = true};
			vectorTarget = new FsmVector3 { UseVariable = true};
			time = 1f;
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
			if(transformTarget.IsNone){
				hash.Add("looktarget", vectorTarget.IsNone ? Vector3.zero : vectorTarget.Value);
			} else {
				if(vectorTarget.IsNone){
					hash.Add("looktarget", transformTarget.Value.transform);
				} else {
					hash.Add("looktarget", transformTarget.Value.transform.position + vectorTarget.Value);
				}
			}
			hash.Add("time", time.IsNone ? 1f : time.Value);
			hash.Add("axis", axis == iTweenFsmAction.AxisRestriction.none ? "" : System.Enum.GetName(typeof(iTweenFsmAction.AxisRestriction), axis));
			DoiTween();	
		}
		
		public override void OnExit(){
		}
		
		public override void OnUpdate(){
			hash.Remove("looktarget");
			if(transformTarget.IsNone){
				hash.Add("looktarget", vectorTarget.IsNone ? Vector3.zero : vectorTarget.Value);
			} else {
				if(vectorTarget.IsNone){
					hash.Add("looktarget", transformTarget.Value.transform);
				} else {
					hash.Add("looktarget", transformTarget.Value.transform.position + vectorTarget.Value);
				}
			}
			DoiTween();
		}
		
		void DoiTween()
		{
			iTween.LookUpdate(go, hash);	
		}
	}
}