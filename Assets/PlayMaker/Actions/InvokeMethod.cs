// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
#endif

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Invokes a Method in a Behaviour attached to a Game Object. See Unity InvokeMethod docs.")]
	public class InvokeMethod : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The game object that owns the behaviour.")]
		public FsmOwnerDefault gameObject;
	
		[RequiredField]
		[UIHint(UIHint.Script)]
        [Tooltip("The behaviour that contains the method.")]
		public FsmString behaviour;

		[RequiredField]
		[UIHint(UIHint.Method)]
        [Tooltip("The name of the method to invoke.")]
		public FsmString methodName;
		
		[HasFloatSlider(0, 10)]
        [Tooltip("Optional time delay in seconds.")]
		public FsmFloat delay;

        [Tooltip("Call the method repeatedly.")]
		public FsmBool repeating;
		
		[HasFloatSlider(0, 10)]
        [Tooltip("Delay between repeated calls in seconds.")]
		public FsmFloat repeatDelay;
		
        [Tooltip("Stop calling the method when the state is exited.")]
		public FsmBool cancelOnExit;

#if UNITY_EDITOR

        private Type cachedType;
        private List<string> methodNames;

#endif

		public override void Reset()
		{
			gameObject = null;
			behaviour = null;
			methodName = "";
			delay = null;
			repeating = false;
			repeatDelay = 1;
			cancelOnExit = false;
		}

		MonoBehaviour component;

		public override void OnEnter()
		{
			DoInvokeMethod(Fsm.GetOwnerDefaultTarget(gameObject));

			Finish();
		}

		void DoInvokeMethod(GameObject go)
		{
			if (go == null)
			{
				return;
			}

			component = go.GetComponent(ReflectionUtils.GetGlobalType(behaviour.Value)) as MonoBehaviour;

			if (component == null)
			{
				LogWarning("InvokeMethod: " + go.name + " missing behaviour: " + behaviour.Value);
				return;
			}

			if (repeating.Value)
			{
				component.InvokeRepeating(methodName.Value, delay.Value, repeatDelay.Value);
			}
			else
			{
				component.Invoke(methodName.Value, delay.Value);
			}
		}

		public override void OnExit()
		{
			if (component == null)
			{
				return;
			}

			if (cancelOnExit.Value)
			{
				component.CancelInvoke(methodName.Value);
			}
		}

#if UNITY_EDITOR


        public override string ErrorCheck()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null || string.IsNullOrEmpty(behaviour.Value))
            {
                return string.Empty;
            }

            var type = ReflectionUtils.GetGlobalType(behaviour.Value);
            if (type == null)
            {
                return "Missing Behaviour: " + behaviour.Value;
            }

            if (cachedType != type)
            {
                cachedType = type;
                methodNames = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Select(m => m.Name).ToList();
            }

            if (!string.IsNullOrEmpty(methodName.Value))
            {
                if (!methodNames.Contains(methodName.Value))
                {
                    return "Missing Method: " + methodName.Value;
                }
            }
            return string.Empty;
        }

#endif

	}
}