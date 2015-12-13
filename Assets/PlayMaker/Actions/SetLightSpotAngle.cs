// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Sets the Spot Angle of a Light.")]
	public class SetLightSpotAngle : ComponentAction<Light>
	{
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;
		public FsmFloat lightSpotAngle;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			lightSpotAngle = 20f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetLightRange();

		    if (!everyFrame)
		    {
		        Finish();
		    }
		}
		
		public override void OnUpdate()
		{
			DoSetLightRange();
		}
		
		void DoSetLightRange()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (UpdateCache(go))
		    {
                light.spotAngle = lightSpotAngle.Value;
		    }
		}
	}
}