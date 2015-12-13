// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	// base type for GUI actions that need a Rect
	[Tooltip("iTween base action - don't use!")]
	public abstract class iTweenFsmAction : FsmStateAction
	{
		/*
			iTween does not allow to run simultaneous iTween of the same type. Please, have this in your mind. That means you can not perform MoveTo and MoveAdd at the same time on one object 
		
		*/
		public enum AxisRestriction{
		none,x,y,z,xy,xz,yz	
		}
		
		[ActionSection("Events")]
		public FsmEvent startEvent;
		public FsmEvent finishEvent;
		[Tooltip("Setting this to true will allow the animation to continue independent of the current time which is helpful for animating menus after a game has been paused by setting Time.timeScale=0.")]
		public FsmBool realTime;
		public FsmBool stopOnExit;
		public FsmBool loopDontFinish;
		
		internal iTweenFSMEvents itweenEvents;
				
		//Don't forget to asign this value in descendatns in order to stop iTween when stopOnExit is true
		protected string itweenType = "";
		protected int itweenID = -1;
		//Since all iTween gets param delay, this variable is set automatically in order to help descedants to pass this param
		
		public override void Reset()
		{
			startEvent = null;
			finishEvent = null;
			realTime = new FsmBool { Value = false };
			stopOnExit = new FsmBool { Value = true };
			loopDontFinish = new FsmBool { Value = true };
			itweenType = "";
		}
		
		protected void OnEnteriTween(FsmOwnerDefault anOwner)
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(anOwner);
            if (go == null) return;
            itweenEvents = go.AddComponent<iTweenFSMEvents>();
			itweenEvents.itweenFSMAction = this;
			iTweenFSMEvents.itweenIDCount++;
			itweenID = iTweenFSMEvents.itweenIDCount;
			itweenEvents.itweenID = iTweenFSMEvents.itweenIDCount;
			itweenEvents.donotfinish = loopDontFinish.IsNone ? false : loopDontFinish.Value; 
		}
		
		protected void IsLoop(bool aValue){
			if(itweenEvents != null) itweenEvents.islooping = aValue;	
		}
		
		protected void OnExitiTween(FsmOwnerDefault anOwner){
			GameObject go = Fsm.GetOwnerDefaultTarget(anOwner);
		    if (go == null) return; // iTween can auto-delete sometimes...?
			if(itweenEvents) GameObject.Destroy(itweenEvents);
			if(stopOnExit.IsNone) iTween.Stop(go, itweenType);
			else if(stopOnExit.Value) iTween.Stop(go, itweenType);
//			if(!stopOnExit.IsNone && stopOnExit.Value == true) {
//				Component[] itweens = go.GetComponents<iTween>();
//				for(int i = 0; i<itweens.Length;i++){
//					iTween itween = (iTween)itweens[i];
//					if(itween.type.ToLower().Contains(itweenType)){
//						GameObject.Destroy(itween);	
//					}
//				}
//			}
		}

	}
}