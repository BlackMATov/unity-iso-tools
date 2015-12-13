// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Tests if a Game Object's Rigid Body is Kinematic.")]
	public class IsKinematic : ComponentAction<Rigidbody>
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;
		
		public FsmEvent trueEvent;
		
		public FsmEvent falseEvent;
		
		[UIHint(UIHint.Variable)]
		public FsmBool store;
		
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			trueEvent = null;
			falseEvent = null;
			store = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoIsKinematic();
			
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsKinematic();
		}

		void DoIsKinematic()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(go))
			{
                var isKinematic = rigidbody.isKinematic;
                store.Value = isKinematic;
                Fsm.Event(isKinematic ? trueEvent : falseEvent);
			}
		}
	}
}

