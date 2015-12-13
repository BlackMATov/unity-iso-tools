// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

/*
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Moves a Game Object's Rigid Body to a new position. To leave any axis unchanged, set variable to 'None'.")]
	public class MovePosition : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;
		public FsmFloat x;
		public FsmFloat y;
		public FsmFloat z;
		public Space space;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			vector = null;
			// default axis to variable dropdown with None selected.
			x = new FsmFloat { UseVariable = true };
			y = new FsmFloat { UseVariable = true };
			z = new FsmFloat { UseVariable = true };
			space = Space.Self;
			everyFrame = false;
		}

		/* Transform scale doesn't stick in OnEnter
		 * TODO: figure out why...
		public override void OnEnter()
		{
			DoSetPosition();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			DoMovePosition();
			
			if (!everyFrame)
				Finish();	
		}

		void DoMovePosition()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			if (go.rigidbody == null) return;
			
			// init position
			
			Vector3 position;

			if (vector.IsNone)
			{
				if (space == Space.World)
					position = go.rigidbody.position;
				else
					position = go.transform.TransformPoint(go.rigidbody.position);
			}
			else
			{
				position = vector.Value;
			}
			
			// override any axis

			if (!x.IsNone) position.x = x.Value;
			if (!y.IsNone) position.y = y.Value;
			if (!z.IsNone) position.z = z.Value;

			// apply
			
			if (space == Space.World)
				go.rigidbody.MovePosition(position);
			else
				go.rigidbody.MovePosition(go.transform.InverseTransformPoint(position))
		}


	}
}
*/