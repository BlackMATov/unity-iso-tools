// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Activates/deactivates a Game Object. Use this to hide/show areas, or enable/disable many Behaviours at once.")]
	public class ActivateGameObject : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject to activate/deactivate.")]
        public FsmOwnerDefault gameObject;
		
		[RequiredField]
        [Tooltip("Check to activate, uncheck to deactivate Game Object.")]
        public FsmBool activate;
		
        [Tooltip("Recursively activate/deactivate all children.")]
		public FsmBool recursive; 
		
        [Tooltip("Reset the game objects when exiting this state. Useful if you want an object to be active only while this state is active.\nNote: Only applies to the last Game Object activated/deactivated (won't work if Game Object changes).")]
		public bool resetOnExit;
		
        [Tooltip("Repeat this action every frame. Useful if Activate changes over time.")]
		public bool everyFrame;
		
		// store the game object that we activated on enter
		// so we can de-activate it on exit.
		GameObject activatedGameObject;

		public override void Reset()
		{
			gameObject = null;
			activate = true;
			recursive = true;
			resetOnExit = false;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoActivateGameObject();
			
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoActivateGameObject();
		}

		public override void OnExit()
		{
			// the stored game object might be invalid now
			if (activatedGameObject == null)
			{
				return;
			}

			if (resetOnExit)
			{
				if (recursive.Value)
				{
#if UNITY_3_5 || UNITY_3_4
                    activatedGameObject.SetActiveRecursively(!activate.Value);
#else
                    SetActiveRecursively(activatedGameObject, !activate.Value);
#endif
                }
				else
                {
#if UNITY_3_5 || UNITY_3_4
                    activatedGameObject.active = !activate.Value;                
#else
                    activatedGameObject.SetActive(!activate.Value);
#endif
                }
			}
		}

		void DoActivateGameObject()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			
			if (go == null)
			{
				return;
			}
			
			if (recursive.Value)
            {
#if UNITY_3_5 || UNITY_3_4
                go.SetActiveRecursively(activate.Value);
#else               
                SetActiveRecursively(go, activate.Value);
#endif
            }
			else
            {
#if UNITY_3_5 || UNITY_3_4
                go.active = activate.Value;
#else				
                go.SetActive(activate.Value);
#endif
            }

			activatedGameObject = go;
        }

#if !(UNITY_3_5 || UNITY_3_4)
        public void SetActiveRecursively(GameObject go, bool state)
        {
            go.SetActive(state);
            foreach (Transform child in go.transform)
            {
                SetActiveRecursively(child.gameObject, state);
            }
        } 
#endif

#if UNITY_EDITOR
        public override string AutoName()
        {
            return (activate.Value ? "Activate " : "Deactivate ") + ActionHelpers.GetValueLabel(Fsm, gameObject);
        }
#endif
    }
}