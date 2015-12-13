// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Controls whether physics affects the Game Object.")]
	public class SetIsKinematic : ComponentAction<Rigidbody>
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmBool isKinematic;

		public override void Reset()
		{
			gameObject = null;
			isKinematic = false;
		}

		public override void OnEnter()
		{
			DoSetIsKinematic();
			Finish();
		}

		void DoSetIsKinematic()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (UpdateCache(go))
		    {
		        rigidbody.isKinematic = isKinematic.Value;
		    }
		}
	}
}

