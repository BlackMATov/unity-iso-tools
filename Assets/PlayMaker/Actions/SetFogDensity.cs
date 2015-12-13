// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.RenderSettings)]
	[Tooltip("Sets the density of the Fog in the scene.")]
	public class SetFogDensity : FsmStateAction
	{
		[RequiredField]
		public FsmFloat fogDensity;
		public bool everyFrame;

		public override void Reset()
		{
			fogDensity = 0.5f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFogDensity();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoSetFogDensity();
		}
		
		void DoSetFogDensity()
		{
			RenderSettings.fogDensity = fogDensity.Value;
		}
	}
}