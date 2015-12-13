// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Flips the value of a Bool Variable.")]
	public class BoolFlip : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Bool variable to flip.")]
		public FsmBool boolVariable;

		public override void Reset()
		{
			boolVariable = null;
		}

		public override void OnEnter()
		{
			boolVariable.Value = !boolVariable.Value;
			Finish();		
		}
	}
}