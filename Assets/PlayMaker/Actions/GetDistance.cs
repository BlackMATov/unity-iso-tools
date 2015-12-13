// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Measures the Distance betweens 2 Game Objects and stores the result in a Float Variable.")]
	public class GetDistance : FsmStateAction
	{
		[RequiredField]
        [Tooltip("Measure distance from this GameObject.")]
		public FsmOwnerDefault gameObject;
		
        [RequiredField]
		[Tooltip("Target GameObject.")]
        public FsmGameObject target;
		
        [RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the distance in a float variable.")]
		public FsmFloat storeResult;
		
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			target = null;
			storeResult = null;
			everyFrame = true;
		}
		
		public override void OnEnter()
		{
			DoGetDistance();

		    if (!everyFrame)
		    {
		        Finish();
		    }
		}
		public override void OnUpdate()
		{
			DoGetDistance();
		}		
		
		void DoGetDistance()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (go == null || target.Value == null || storeResult == null)
		    {
		        return;
		    }
					
			storeResult.Value = Vector3.Distance(go.transform.position, target.Value.transform.position);
		}

	}
}