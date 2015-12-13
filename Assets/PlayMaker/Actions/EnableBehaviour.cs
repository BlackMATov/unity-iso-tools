// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Enables/Disables a Behaviour on a GameObject. Optionally reset the Behaviour on exit - useful if you want the Behaviour to be active only while this state is active.")]
	public class EnableBehaviour : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject that owns the Behaviour.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Behaviour)]
        [Tooltip("The name of the Behaviour to enable/disable.")]
		public FsmString behaviour;
		
		[Tooltip("Optionally drag a component directly into this field (behavior name will be ignored).")]
		public Component component;
		
		[RequiredField]
        [Tooltip("Set to True to enable, False to disable.")]
		public FsmBool enable;
		
		public FsmBool resetOnExit;

		public override void Reset()
		{
			gameObject = null;
			behaviour = null;
			component = null;
			enable = true;
			resetOnExit = true;
		}

		Behaviour componentTarget;

		public override void OnEnter()
		{
			DoEnableBehaviour(Fsm.GetOwnerDefaultTarget(gameObject));
			
			Finish();
		}

		void DoEnableBehaviour(GameObject go)
		{
			if (go == null)
			{
				return;
			}

			if (component != null)
			{
				componentTarget = component as Behaviour;
			}
			else
			{
				componentTarget = go.GetComponent(ReflectionUtils.GetGlobalType(behaviour.Value)) as Behaviour;
			}

			if (componentTarget == null)
			{
				LogWarning(" " + go.name + " missing behaviour: " + behaviour.Value);
				return;
			}

			componentTarget.enabled = enable.Value;
		}

		public override void OnExit()
		{
			if (componentTarget == null)
			{
				return;
			}

			if (resetOnExit.Value)
			{
				componentTarget.enabled = !enable.Value;
			}
		}

	    public override string ErrorCheck()
	    {
	        var go = Fsm.GetOwnerDefaultTarget(gameObject);

	        if (go == null || component != null || behaviour.IsNone || string.IsNullOrEmpty(behaviour.Value))
	        {
	            return null;
	        }

	        var comp = go.GetComponent(ReflectionUtils.GetGlobalType(behaviour.Value)) as Behaviour;
	        return comp != null ? null : "Behaviour missing";
	    }
	}
}