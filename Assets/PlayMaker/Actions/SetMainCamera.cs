// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Sets the Main Camera.")]
	public class SetMainCamera : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Camera))]
		[Tooltip("The GameObject to set as the main camera (should have a Camera component).")]
		public FsmGameObject gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			if (gameObject.Value != null)
			{
				if (Camera.main != null)
				{
					Camera.main.gameObject.tag = "Untagged";
				}

				gameObject.Value.tag = "MainCamera";
			}

			Finish();
		}
	}
}