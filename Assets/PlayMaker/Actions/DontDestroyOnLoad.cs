// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Level)]
	[Tooltip("Makes the Game Object not be destroyed automatically when loading a new scene.")]
	public class DontDestroyOnLoad : FsmStateAction
	{
		[RequiredField]
        [Tooltip("GameObject to mark as DontDestroyOnLoad.")]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			// Have to get the root, since the game object will be destroyed if any of its parents are destroyed.
			
			Object.DontDestroyOnLoad(Owner.transform.root.gameObject);
			
			Finish();
		}
	}
}