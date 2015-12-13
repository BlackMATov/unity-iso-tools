// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets the value of a Float Variable.")]
	public class SetFloatValue : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;
		[RequiredField]
		public FsmFloat floatValue;
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			floatValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			floatVariable.Value = floatValue.Value;
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			floatVariable.Value = floatValue.Value;
		}
	}
}