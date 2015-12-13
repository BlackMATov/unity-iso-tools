// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.RenderSettings)]
	[Tooltip("Sets the global Skybox.")]
	public class SetSkybox : FsmStateAction
	{
		public FsmMaterial skybox;
		[Tooltip("Repeat every frame. Useful if the Skybox is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			skybox = null;
		}

		public override void OnEnter()
		{
			RenderSettings.skybox = skybox.Value;

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			RenderSettings.skybox = skybox.Value;
		}
	}
}