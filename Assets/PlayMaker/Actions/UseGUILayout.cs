// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Turn GUILayout on/off. If you don't use GUILayout actions you can get some performace back by turning GUILayout off. This can make a difference on iOS platforms.")]
	public class UseGUILayout : FsmStateAction
	{
		[RequiredField]
		public bool turnOffGUIlayout;

		public override void Reset()
		{
			turnOffGUIlayout = true;
		}

		public override void OnEnter()
		{
			Fsm.Owner.useGUILayout = !turnOffGUIlayout;
			Finish();
		}
	}
}