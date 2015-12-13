// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.RenderSettings)]
	[Tooltip("Sets the size of light halos.")]
	public class SetHaloStrength : FsmStateAction
	{
		[RequiredField]
		public FsmFloat haloStrength;
		public bool everyFrame;

		public override void Reset()
		{
			haloStrength = 0.5f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetHaloStrength();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoSetHaloStrength();
		}
		
		void DoSetHaloStrength()
		{
			RenderSettings.haloStrength = haloStrength.Value;
		}
	}
}