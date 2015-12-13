// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Checks if an Object has a Component. Optionally remove the Component on exiting the state.")]
	public class HasComponent : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		[UIHint(UIHint.ScriptComponent)]
		public FsmString component;
		public FsmBool removeOnExit;
		public FsmEvent trueEvent;
		public FsmEvent falseEvent;
		[UIHint(UIHint.Variable)]
		public FsmBool store;
		public bool everyFrame;

		Component aComponent;

		public override void Reset()
		{
			aComponent = null;
			gameObject = null;
			trueEvent = null;
			falseEvent = null;
			component = null;
			store = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoHasComponent(gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value);
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoHasComponent(gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value);
		}

		public override void OnExit()
		{
			if (removeOnExit.Value && aComponent != null)
			{
				Object.Destroy(aComponent);
			}
		}

		void DoHasComponent(GameObject go)
		{
			aComponent = go.GetComponent(ReflectionUtils.GetGlobalType(component.Value));

			Fsm.Event(aComponent != null ? trueEvent : falseEvent);
		}
	}
}