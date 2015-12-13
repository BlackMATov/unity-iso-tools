// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Sets Field of View used by the Camera.")]
	public class SetCameraFOV : ComponentAction<Camera>
	{
		[RequiredField]
		[CheckForComponent(typeof(Camera))]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmFloat fieldOfView;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			fieldOfView = 50f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetCameraFOV();

		    if (!everyFrame)
		    {
		        Finish();
		    }
		}
		
		public override void OnUpdate()
		{
			DoSetCameraFOV();
		}
		
		void DoSetCameraFOV()
		{
		    var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (UpdateCache(go))
		    {
                camera.fieldOfView = fieldOfView.Value;
		    }
		}
	}
}