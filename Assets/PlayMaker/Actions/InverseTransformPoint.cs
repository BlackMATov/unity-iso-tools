// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Transforms position from world space to a Game Object's local space. The opposite of TransformPoint.")]
	public class InverseTransformPoint : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmVector3 worldPosition;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeResult;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			worldPosition = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoInverseTransformPoint();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			DoInverseTransformPoint();
		}
		
		void DoInverseTransformPoint()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if(go == null) return;
			
			storeResult.Value = go.transform.InverseTransformPoint(worldPosition.Value);
		}
	}
}

