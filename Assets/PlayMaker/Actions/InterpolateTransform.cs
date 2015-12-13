// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
/*
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Interpolates a Game Objects Transform (Position/Rotation/Scale). You can specify the Transform using a Game Object, or individual Position, Rotation, Scale vectors.")]
	public class InterpolateTransform : FsmStateAction
	{
		public InterpolationType mode;
		[RequiredField]
		[Tooltip("The Game Object to move.")]
		public FsmOwnerDefault gameObject;
		public FsmGameObject fromObject;
		public FsmVector3 fromPosition;
		public FsmVector3 fromRotation;
		public FsmVector3 fromScale;
		public FsmGameObject fromObject;
		public FsmVector3 fromPosition;
		public FsmVector3 fromRotation;
		public FsmVector3 fromScale;
		
		public override void Reset()
		{
			sendEvent = null;
		}

		public override void OnUpdate()
		{
			if (Input.anyKeyDown)
				Fsm.Event(sendEvent);
		}
	}
}
*/