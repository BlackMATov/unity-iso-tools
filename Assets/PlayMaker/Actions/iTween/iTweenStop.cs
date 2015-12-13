// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Stop an iTween action.")]
	public class iTweenStop : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		public FsmString id;
		public iTweenFSMType iTweenType = iTweenFSMType.all;
		public bool includeChildren;
		public bool inScene;
		
		public override void Reset()
		{
			id = new FsmString{UseVariable = true};
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
			if(id.IsNone){
				if(iTweenType == iTweenFSMType.all){
					iTween.Stop();
				} else {
					if(inScene) {
						iTween.Stop(System.Enum.GetName(typeof(iTweenFSMType), iTweenType));
					} else {
						GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
						if (go == null) return;
						iTween.Stop(go, System.Enum.GetName(typeof(iTweenFSMType), iTweenType), includeChildren);
					}
				}
			} else {
				iTween.StopByName(id.Value);	
			}
		}
	}
}