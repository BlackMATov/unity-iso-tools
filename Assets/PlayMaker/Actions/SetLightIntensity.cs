// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Sets the Intensity of a Light.")]
	public class SetLightIntensity : ComponentAction<Light>
	{
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;
		public FsmFloat lightIntensity;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			lightIntensity = 1f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetLightIntensity();

		    if (!everyFrame)
		    {
		        Finish();
		    }
		}
		
		public override void OnUpdate()
		{
			DoSetLightIntensity();
		}
		
		void DoSetLightIntensity()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (UpdateCache(go))
		    {
                light.intensity = lightIntensity.Value;
		    }
		}
	}
}