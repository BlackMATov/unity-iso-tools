// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Forces a Game Object's Rigid Body to wake up.")]
	public class WakeUp : ComponentAction<Rigidbody>
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			DoWakeUp();
			Finish();
		}

		void DoWakeUp()
		{
			var go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;
		    if (UpdateCache(go))
		    {
		        rigidbody.WakeUp();
		    }
		}
	}
}