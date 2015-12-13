// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Activates a Camera in the scene.")]
	public class CutToCamera : FsmStateAction
	{
		[RequiredField]
		public Camera camera;
		public bool makeMainCamera;
		public bool cutBackOnExit;

		Camera oldCamera;

		public override void Reset()
		{
			camera = null;
			makeMainCamera = true;
			cutBackOnExit = false;
		}

		public override void OnEnter()
		{
			if (camera == null)
			{
				LogError("Missing camera!");
				return;
			}

			oldCamera = Camera.main;

			SwitchCamera(Camera.main, camera);

			if (makeMainCamera)
				camera.tag = "MainCamera";
			
			Finish();
		}

		public override void OnExit()
		{
			if (cutBackOnExit)
			{
				SwitchCamera(camera, oldCamera);
			}
		}

		static void SwitchCamera(Camera camera1, Camera camera2)
		{
			if (camera1 != null)
			{
				camera1.enabled = false;
			}

			if (camera2 != null)
			{
				camera2.enabled = true;
			}
		}
	}
}