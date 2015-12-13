// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Resets all Input. After ResetInputAxes all axes return to 0 and all buttons return to 0 for one frame")]
	public class ResetInputAxes : FsmStateAction
	{
		public override void Reset(){}
		
		public override void OnEnter()
		{
			Input.ResetInputAxes();
			Finish();
		}
	}
}