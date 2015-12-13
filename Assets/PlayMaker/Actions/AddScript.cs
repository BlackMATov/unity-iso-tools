// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Adds a Script to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Script on exiting the state.")]
	public class AddScript : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to add the script to.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("The Script to add to the GameObject.")]
		[UIHint(UIHint.ScriptComponent)]
		public FsmString script;
		
		[Tooltip("Remove the script from the GameObject when this State is exited.")]
		public FsmBool removeOnExit;

		Component addedComponent;

		public override void Reset()
		{
			gameObject = null;
			script = null;
		}

		public override void OnEnter()
		{
			DoAddComponent(gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value);
			
			Finish();
		}

		public override void OnExit()
		{
			if (removeOnExit.Value)
            {
                if (addedComponent != null)
                {
                    Object.Destroy(addedComponent);
                }
            }
		}

		void DoAddComponent(GameObject go)
		{
			addedComponent = go.AddComponent(ReflectionUtils.GetGlobalType(script.Value));

			if (addedComponent == null)
			{
				LogError("Can't add script: " + script.Value);
			}
		}
	}
}