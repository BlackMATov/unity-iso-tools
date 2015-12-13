// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Pause an iTween action.")]
	public class iTweenPause : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		public iTweenFSMType iTweenType = iTweenFSMType.all;
		public bool includeChildren;
		public bool inScene;
		
		public override void Reset()
		{
			iTweenType = iTweenFSMType.all;
			includeChildren = false;
			inScene = false;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			DoiTween();
			Finish();
		}
							
		void DoiTween()
		{
			if(iTweenType == iTweenFSMType.all){
				iTween.Pause();
			} else {
				if(inScene) {
					iTween.Pause(System.Enum.GetName(typeof(iTweenFSMType), iTweenType));
				} else {
					GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
					if (go == null) return;
					iTween.Pause(go, System.Enum.GetName(typeof(iTweenFSMType), iTweenType), includeChildren);
				}
			}
		}
	}
}