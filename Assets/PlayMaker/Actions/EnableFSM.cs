// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Enables/Disables an FSM component on a GameObject.")]
	public class EnableFSM : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject that owns the FSM component.")]
		public FsmOwnerDefault gameObject;
		
        [UIHint(UIHint.FsmName)]
        [Tooltip("Optional name of FSM on GameObject. Useful if you have more than one FSM on a GameObject.")]
		public FsmString fsmName;

        [Tooltip("Set to True to enable, False to disable.")]
        public FsmBool enable;

        [Tooltip("Reset the initial enabled state when exiting the state.")]
        public FsmBool resetOnExit;

		private PlayMakerFSM fsmComponent;
		
		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			enable = true;
			resetOnExit = true;
		}

		public override void OnEnter()
		{
			DoEnableFSM();
			
			Finish();
		}

		void DoEnableFSM()
		{
			GameObject go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;

			if (go == null) return;
			
			if (!string.IsNullOrEmpty(fsmName.Value))
			{
				// find by FSM component name
				
				var fsmComponents = go.GetComponents<PlayMakerFSM>();
				foreach (var component in fsmComponents)
				{
					if (component.FsmName == fsmName.Value)
					{
						fsmComponent = component;
						break;
					}
				}
			}
			else
			{
				// get first FSM component
				
				fsmComponent = go.GetComponent<PlayMakerFSM>();
			}
			
			if (fsmComponent == null)
			{
				// TODO: Not sure if this is an error condition... 
				LogError("Missing FsmComponent!");
				return;
			}

			fsmComponent.enabled = enable.Value;
		}

		public override void OnExit()
		{
			if (fsmComponent == null) return;

			if (resetOnExit.Value)
			{
				fsmComponent.enabled = !enable.Value;
			}
		}

	}
}