// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Set the Tag on all children of a GameObject. Optionally filter by component.")]
	public class SetTagsOnChildren : FsmStateAction
	{
		[RequiredField]
		[Tooltip("GameObject Parent")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[UIHint(UIHint.Tag)]
		[Tooltip("Set Tag To...")]
		public FsmString tag;
		
		[UIHint(UIHint.ScriptComponent)]
		[Tooltip("Only set the Tag on children with this component.")]
		public FsmString filterByComponent;
		
		public override void Reset()
		{
			gameObject = null;
			tag = null;
			filterByComponent = null;
		}

	    private Type componentFilter;
		
		public override void OnEnter()
		{
            SetTag(Fsm.GetOwnerDefaultTarget(gameObject));
			
			Finish();
		}
		
		void SetTag(GameObject parent)
		{
			if (parent == null)
			{
				return;
			}

            if (string.IsNullOrEmpty(filterByComponent.Value)) // do all children
            {
                foreach (Transform child in parent.transform)
                {
                    child.gameObject.tag = tag.Value;
                }
            }
            else
            {
                UpdateComponentFilter();

                if (componentFilter != null) // filter by component
                {
                    var root = parent.GetComponentsInChildren(componentFilter);
                    foreach (var child in root)
                    {
                        child.gameObject.tag = tag.Value;
                    }
                }
            }

			Finish();
		}

        void UpdateComponentFilter()
        {
            componentFilter = ReflectionUtils.GetGlobalType(filterByComponent.Value);

            if (componentFilter == null) // try adding UnityEngine namespace
            {
                componentFilter = ReflectionUtils.GetGlobalType("UnityEngine." + filterByComponent.Value);
            }

            if (componentFilter == null)
            {
                Debug.LogWarning("Couldn't get type: " + filterByComponent.Value);
            }
        }
	}
}