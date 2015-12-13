// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Causes the device to vibrate for half a second.")]
	public class DeviceVibrate : FsmStateAction
	{
		public override void Reset()
		{}

		public override void OnEnter()
		{
#if (UNITY_IPHONE || UNITY_ANDROID)			
			Handheld.Vibrate();
#endif
            Finish();
        }
	}
}