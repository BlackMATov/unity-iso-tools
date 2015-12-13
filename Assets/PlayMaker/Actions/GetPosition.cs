// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Gets the Position of a Game Object and stores it in a Vector3 Variable or each Axis in a Float Variable")]
	public class GetPosition : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;
		
		[UIHint(UIHint.Variable)]
		public FsmFloat x;
		
		[UIHint(UIHint.Variable)]
		public FsmFloat y;
		
		[UIHint(UIHint.Variable)]
		public FsmFloat z;
		
		public Space space;
		
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			vector = null;
			x = null;
			y = null;
			z = null;
			space = Space.World;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetPosition();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}

		public override void OnUpdate()
		{
			DoGetPosition();
		}

		void DoGetPosition()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			var position = space == Space.World ? go.transform.position : go.transform.localPosition;				
			
			vector.Value = position;
			x.Value = position.x;
			y.Value = position.y;
			z.Value = position.z;
		}


	}
}