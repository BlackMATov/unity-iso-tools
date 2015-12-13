// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Sets the strength of the shadows cast by a Light.")]
	public class SetShadowStrength : ComponentAction<Light>
	{
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;
		public FsmFloat shadowStrength;
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			shadowStrength = 0.8f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetShadowStrength();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoSetShadowStrength();
		}
		
		void DoSetShadowStrength()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (UpdateCache(go))
		    {
		        light.shadowStrength = shadowStrength.Value;
		    }
		}
	}
}