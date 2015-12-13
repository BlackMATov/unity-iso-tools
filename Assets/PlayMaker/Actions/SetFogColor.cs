// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.RenderSettings)]
	[Tooltip("Sets the color of the Fog in the scene.")]
	public class SetFogColor : FsmStateAction
	{
		[RequiredField]
		public FsmColor fogColor;
		public bool everyFrame;

		public override void Reset()
		{
			fogColor = Color.white;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFogColor();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoSetFogColor();
		}
		
		void DoSetFogColor()
		{
			RenderSettings.fogColor = fogColor.Value;
		}
	}
}