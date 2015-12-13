// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{	
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Stops location service updates. This could be useful for saving battery life.")]
	public class StopLocationServiceUpdates : FsmStateAction
	{		
		public override void Reset()
		{
		}

		public override void OnEnter()
        {
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8
  			Input.location.Stop();
#endif
            Finish();
		}
	}
}