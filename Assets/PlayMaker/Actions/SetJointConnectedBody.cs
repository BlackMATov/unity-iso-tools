// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Connect a joint to a game object.")]
	public class SetJointConnectedBody : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Joint))]
		[Tooltip("The joint to connect. Requires a Joint component.")]
		public FsmOwnerDefault joint;

		[CheckForComponent(typeof (Rigidbody))] 
		[Tooltip("The game object to connect to the Joint. Set to none to connect the Joint to the world.")] 
		public FsmGameObject rigidBody;

		public override void Reset()
		{
			joint = null;
			rigidBody = null;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(joint);
			if (go != null)
			{
				var jointComponent = go.GetComponent<Joint>();
				
				if (jointComponent != null)
				{
					jointComponent.connectedBody = rigidBody.Value == null ? null : rigidBody.Value.GetComponent<Rigidbody>();
				}
			}

			Finish();
		}
	}
}