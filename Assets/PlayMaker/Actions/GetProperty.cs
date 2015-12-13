// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

#if !UNITY_FLASH

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.UnityObject)]
	[Tooltip("Gets the value of any public property or field on the targeted Unity Object and stores it in a variable. E.g., Drag and drop any component attached to a Game Object to access its properties.")]
	public class GetProperty : FsmStateAction
	{
		public FsmProperty targetProperty;
		public bool everyFrame;

		public override void Reset()
		{
			targetProperty = new FsmProperty { setProperty = false };
			everyFrame = false;
		}

		public override void OnEnter()
		{
			targetProperty.GetValue();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			targetProperty.GetValue();
		}

#if UNITY_EDITOR
        public override string AutoName()
        {
            var name = string.IsNullOrEmpty(targetProperty.PropertyName) ? "[none]" : targetProperty.PropertyName;
            return "Get Property: "+ name;
            //var value = ActionHelpers.GetValueLabel(targetProperty.GetVariable());
            //return string.Format("Get {0} to {1}", name, value);
        }
#endif
	}
}

#endif