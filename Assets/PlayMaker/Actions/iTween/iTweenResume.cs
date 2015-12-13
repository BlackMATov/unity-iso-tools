// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Resume an iTween action.")]
	public class iTweenResume : FsmStateAction
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
				iTween.Resume();
			} else {
				if(inScene) {
					iTween.Resume(System.Enum.GetName(typeof(iTweenFSMType), iTweenType));
				} else {
					GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
					if (go == null) return;
					iTween.Resume(go, System.Enum.GetName(typeof(iTweenFSMType), iTweenType), includeChildren);
				}
			}
		}
	}
}