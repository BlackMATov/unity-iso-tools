// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.RenderSettings)]
	[Tooltip("Enables/Disables Fog in the scene.")]
	public class EnableFog : FsmStateAction
	{
        [Tooltip("Set to True to enable, False to disable.")]
        public FsmBool enableFog;
		
        [Tooltip("Repeat every frame. Useful if the Enable Fog setting is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			enableFog = true;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			RenderSettings.fog = enableFog.Value;
			
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			RenderSettings.fog = enableFog.Value;
		}
	}
}