// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets the value of the specified Input Axis and stores it in a Float Variable. See Unity Input Manager docs.")]
	public class GetAxis : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The name of the axis. Set in the Unity Input Manager.")]
        public FsmString axisName;

        [Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range.")]
		public FsmFloat multiplier;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a float variable.")]
        public FsmFloat store;

        [Tooltip("Repeat every frame. Typically this would be set to True.")]
		public bool everyFrame;

		public override void Reset()
		{
			axisName = "";
			multiplier = 1.0f;
			store = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoGetAxis();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetAxis();
		}

		void DoGetAxis()
		{
			var axisValue = Input.GetAxis(axisName.Value);

			// if variable set to none, assume multiplier of 1
			if (!multiplier.IsNone)
			{
				axisValue *= multiplier.Value;
			}

			store.Value = axisValue;
		}
	}
}

