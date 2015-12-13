// Thanks MaDDoX: http://hutonggames.com/playmakerforum/index.php?topic=159.0

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Material)]
    [Tooltip("Sets the visibility of a GameObject. Note: this action sets the GameObject Renderer's enabled state.")]
	public class SetVisibility : ComponentAction<Renderer>
    {
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		//[UIHint(UIHint.Variable)]
        [Tooltip("Should the object visibility be toggled?\nHas priority over the 'visible' setting")]
        public FsmBool toggle;
		
		//[UIHint(UIHint.Variable)]
		[Tooltip("Should the object be set to visible or invisible?")]
        public FsmBool visible;
        
		[Tooltip("Resets to the initial visibility when it leaves the state")]
        public bool resetOnExit;
        
		private bool initialVisibility;

		public override void Reset()
		{
			gameObject = null;
            toggle = false;
			visible = false;
            resetOnExit = true;
            initialVisibility = false;
		}
		
		public override void OnEnter()
		{
			DoSetVisibility(Fsm.GetOwnerDefaultTarget(gameObject));
            
            Finish();
		}

        void DoSetVisibility(GameObject go)
		{
            if (!UpdateCache(go))
            {
                return;
            }

            // remember initial visibility
            initialVisibility = renderer.enabled;

            // if 'toggle' is not set, simply sets visibility to new value
            if (toggle.Value == false) 
            {
                renderer.enabled = visible.Value;
                return;
            }
			
            // otherwise, toggles the visibility
            renderer.enabled = !renderer.enabled;
		}

        public override void OnExit()
        {
            if (resetOnExit)
            {
            	ResetVisibility();
            }
        }

        void ResetVisibility()
        {
            if (renderer != null)
            {
            	renderer.enabled = initialVisibility;
            }
        }

	}
}