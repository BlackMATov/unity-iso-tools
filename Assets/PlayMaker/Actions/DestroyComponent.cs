// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Destroys a Component of an Object.")]
	public class DestroyComponent : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject that owns the Component.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.ScriptComponent)]
        [Tooltip("The name of the Component to destroy.")]
		public FsmString component;
				
		Component aComponent;

		public override void Reset()
		{
			aComponent = null;
			gameObject = null;
			component = null;
		}

		public override void OnEnter()
		{
			DoDestroyComponent(gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value);
			
			Finish();
		}

		
		void DoDestroyComponent(GameObject go)
		{
			aComponent = go.GetComponent(ReflectionUtils.GetGlobalType(component.Value));
			if (aComponent == null)
			{
				LogError("No such component: " + component.Value);
			}
			else
			{
				Object.Destroy(aComponent);
			}
		}
	}
}