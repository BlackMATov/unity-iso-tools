// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Rect)]
	[Tooltip("Sets the value of a Rect Variable.")]
	public class SetRectValue : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmRect rectVariable;
		[RequiredField]
		public FsmRect rectValue;
		public bool everyFrame;

		public override void Reset()
		{
			rectVariable = null;
			rectValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			rectVariable.Value = rectValue.Value;

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			rectVariable.Value = rectValue.Value;
		}
	}
}