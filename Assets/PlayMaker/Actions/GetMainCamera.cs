// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
// Thanks to James Murchison for the original version of this script.

using UnityEngine;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Gets the camera tagged MainCamera from the scene")]
	public class GetMainCamera : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;
		
		public override void Reset ()
		{
			storeGameObject = null;
		}
		
		public override void OnEnter ()
		{
			storeGameObject.Value = Camera.main != null ? Camera.main.gameObject : null;
			
			Finish();
		}
	}
}