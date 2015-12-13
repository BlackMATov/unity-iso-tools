// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Gets the Right n characters from a String.")]
	public class GetStringRight : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;

        [Tooltip("Number of characters to get.")]
		public FsmInt charCount;
		
        [RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString storeResult;

        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			charCount = 0;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetStringRight();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoGetStringRight();
		}
		
		void DoGetStringRight()
		{
			if (stringVariable.IsNone) return;
			if (storeResult.IsNone) return;
			
			var text = stringVariable.Value;
		    var count = Mathf.Clamp(charCount.Value, 0, text.Length);
			storeResult.Value = text.Substring(text.Length - count, count);
		}
		
	}
}