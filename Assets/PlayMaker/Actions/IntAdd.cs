// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Adds a value to an Integer Variable.")]
	public class IntAdd : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;
		[RequiredField]
		public FsmInt add;
		public bool everyFrame;

		public override void Reset()
		{
			intVariable = null;
			add = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			intVariable.Value += add.Value;
			
			if (!everyFrame)
				Finish();
		}
		
		// NOTE: very frame rate dependent!
		public override void OnUpdate()
		{
			intVariable.Value += add.Value;
		}
	}
}