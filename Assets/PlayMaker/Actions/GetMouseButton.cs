// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets the pressed state of the specified Mouse Button and stores it in a Bool Variable. See Unity Input Manager doc.")]
	public class GetMouseButton : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The mouse button to test.")]
		public MouseButton button;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the pressed state in a Bool Variable.")]
		public FsmBool storeResult;

		public override void Reset()
		{
			button = MouseButton.Left;
			storeResult = null;
		}

        public override void OnEnter()
        {
            storeResult.Value = Input.GetMouseButton((int)button);
        }

		public override void OnUpdate()
		{
			storeResult.Value = Input.GetMouseButton((int)button);
		}
	}
}

