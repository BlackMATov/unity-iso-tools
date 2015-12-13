// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Builds a String from other Strings.")]
	public class BuildString : FsmStateAction
	{
		[RequiredField]
        [Tooltip("Array of Strings to combine.")]
		public FsmString[] stringParts;

        [Tooltip("Separator to insert between each String. E.g. space character.")]
        public FsmString separator;

        [Tooltip("Add Separator to end of built string.")]
	    public FsmBool addToEnd;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the final String in a variable.")]
        public FsmString storeResult;

        [Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;
	    
        private string result;

		public override void Reset()
		{
			stringParts = new FsmString[3];
			separator = null;
		    addToEnd = true;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoBuildString();
			
			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			DoBuildString();
		}
		
		void DoBuildString()
		{
			if (storeResult == null) return;
			
			result = "";

		    for (var i = 0; i < stringParts.Length-1; i++)
		    {
		        result += stringParts[i];
		        result += separator.Value;
		    }
		    result += stringParts[stringParts.Length - 1];

		    if (addToEnd.Value)
		    {
		        result += separator.Value;
		    }

		    storeResult.Value = result;
		}
		
	}
}