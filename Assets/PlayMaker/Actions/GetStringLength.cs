// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Gets the Length of a String.")]
	public class GetStringLength : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt storeResult;
		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetStringLength();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoGetStringLength();
		}
		
		void DoGetStringLength()
		{
			if (stringVariable == null) return;
			if (storeResult == null) return;
			
			storeResult.Value = stringVariable.Value.Length;
		}
		
	}
}