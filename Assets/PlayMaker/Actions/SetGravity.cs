// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Sets the gravity vector, or individual axis.")]
	public class SetGravity : FsmStateAction
	{
		public FsmVector3 vector;
		public FsmFloat x;
		public FsmFloat y;
		public FsmFloat z;
		public bool everyFrame;
		
		public override void Reset()
		{
			vector = null;
			x = new FsmFloat { UseVariable = true };
			y = new FsmFloat { UseVariable = true };
			z = new FsmFloat { UseVariable = true };
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetGravity();
			
			if (!everyFrame)
				Finish();		
		}
		
		public override void OnUpdate()
		{
			DoSetGravity();
		}
		
		void DoSetGravity()
		{
			Vector3 gravity = vector.Value;
			
			if (!x.IsNone)
				gravity.x = x.Value;
			if (!y.IsNone)
				gravity.y = y.Value;
			if (!z.IsNone)
				gravity.z = z.Value;
			
			Physics.gravity = gravity;
		}
	}
}