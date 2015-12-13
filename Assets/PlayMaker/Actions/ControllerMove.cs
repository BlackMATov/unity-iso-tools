// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Character)]
	[Tooltip("Moves a Game Object with a Character Controller. See also Controller Simple Move. NOTE: It is recommended that you make only one call to Move or SimpleMove per frame.")]
	public class ControllerMove : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(CharacterController))]
		[Tooltip("The GameObject to move.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("The movement vector.")]
		public FsmVector3 moveVector;
		
		[Tooltip("Move in local or word space.")]
		public Space space;
		
		[Tooltip("Movement vector is defined in units per second. Makes movement frame rate independent.")]
		public FsmBool perSecond;
		
		private GameObject previousGo; // remember so we can get new controller only when it changes.
		private CharacterController controller;
		
		public override void Reset()
		{
			gameObject = null;
			moveVector = new FsmVector3 {UseVariable = true};
			space = Space.World;
			perSecond = true;
		}

		public override void OnUpdate()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
		
			if (go != previousGo)
			{
				controller = go.GetComponent<CharacterController>();
				previousGo = go;
			}
			
			if (controller != null)
			{
				var move = space == Space.World ? moveVector.Value : go.transform.TransformDirection(moveVector.Value);

				if (perSecond.Value)
				{
					controller.Move(move * Time.deltaTime);
				}
				else
				{
					controller.Move(move);
				}
			}
		}
	}
}
