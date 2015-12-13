// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Transforms a Position from a Game Object's local space to world space.")]
	public class TransformPoint : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmVector3 localPosition;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeResult;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			localPosition = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoTransformPoint();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			DoTransformPoint();
		}
		
		void DoTransformPoint()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if(go == null) return;
			
			storeResult.Value = go.transform.TransformPoint(localPosition.Value);
		}
	}
}

