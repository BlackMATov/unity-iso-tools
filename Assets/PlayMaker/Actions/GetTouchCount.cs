// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Gets the number of Touches.")]
	public class GetTouchCount : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt storeCount;
		public bool everyFrame;
		
		public override void Reset()
		{
			storeCount = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetTouchCount();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoGetTouchCount();
		}
		
		void DoGetTouchCount()
		{
			storeCount.Value = Input.touchCount;
		}
		
	}
}