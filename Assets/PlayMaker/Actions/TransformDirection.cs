// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Transforms a Direction from a Game Object's local space to world space.")]
	public class TransformDirection : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmVector3 localDirection;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeResult;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			localDirection = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoTransformDirection();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			DoTransformDirection();
		}
		
		void DoTransformDirection()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if(go == null) return;
			
			storeResult.Value = go.transform.TransformDirection(localDirection.Value);
		}
	}
}

