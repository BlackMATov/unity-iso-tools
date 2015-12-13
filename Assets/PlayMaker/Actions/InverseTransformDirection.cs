// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Transforms a Direction from world space to a Game Object's local space. The opposite of TransformDirection.")]
	public class InverseTransformDirection : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmVector3 worldDirection;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeResult;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			worldDirection = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoInverseTransformDirection();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			DoInverseTransformDirection();
		}
		
		void DoInverseTransformDirection()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if(go == null) return;
			
			storeResult.Value = go.transform.InverseTransformDirection(worldDirection.Value);
		}
	}
}

