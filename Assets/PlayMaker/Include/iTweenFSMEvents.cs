using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

public class iTweenFSMEvents : MonoBehaviour {
	static public int itweenIDCount = 0;
	public int itweenID = 0;
	public iTweenFsmAction itweenFSMAction = null;
	public bool donotfinish = false;
	public bool islooping = false;
	
	void iTweenOnStart(int aniTweenID){
		if(itweenID == aniTweenID){
			itweenFSMAction.Fsm.Event(itweenFSMAction.startEvent);
		}
	}
	
	void iTweenOnComplete(int aniTweenID){
		if(itweenID == aniTweenID) {
			if(islooping) {
				if(!donotfinish){
					itweenFSMAction.Fsm.Event(itweenFSMAction.finishEvent);
					itweenFSMAction.Finish();	
				}
			} else {
				itweenFSMAction.Fsm.Event(itweenFSMAction.finishEvent);
				itweenFSMAction.Finish();
			}
		}
	}
}

public enum iTweenFSMType{
	all,
	move,
	rotate,
	scale,
	shake,
	position,
	value,
	look
}
