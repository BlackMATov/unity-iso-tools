// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Gets a sub-string from a String Variable.")]
	public class GetSubstring : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;
		[RequiredField]
		public FsmInt startIndex;
		[RequiredField]
		public FsmInt length;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString storeResult;
		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			startIndex = 0;
			length = 1;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetSubstring();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoGetSubstring();
		}
		
		void DoGetSubstring()
		{
			if (stringVariable == null) return;
			if (storeResult == null) return;
			
			storeResult.Value = stringVariable.Value.Substring(startIndex.Value, length.Value);
		}
		
	}
}