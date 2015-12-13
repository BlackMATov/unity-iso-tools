// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets the pressed state of the specified Button and stores it in a Bool Variable. See Unity Input Manager docs.")]
	public class GetButton : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The name of the button. Set in the Unity Input Manager.")]
		public FsmString buttonName;		

		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a bool variable.")]
		public FsmBool storeResult;

        [Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			buttonName = "Fire1";
			storeResult = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoGetButton();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetButton();
		}

		void DoGetButton()
		{
			storeResult.Value = Input.GetButton(buttonName.Value);
		}
	}
}

