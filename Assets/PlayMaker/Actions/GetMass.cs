// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets the Mass of a Game Object's Rigid Body.")]
	public class GetMass : ComponentAction<Rigidbody>
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
        [Tooltip("The GameObject that owns the Rigidbody")]
		public FsmOwnerDefault gameObject;
		
        [RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the mass in a float variable.")]
		public FsmFloat storeResult;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoGetMass();
			
			Finish();
		}

		void DoGetMass()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (UpdateCache(go))
		    {
                storeResult.Value = rigidbody.mass;
		    }
		}
	}
}