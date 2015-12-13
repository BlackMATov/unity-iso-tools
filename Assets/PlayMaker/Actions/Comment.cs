// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Adds a text area to the action list. NOTE: Doesn't do anything, just for notes...")]
	public class Comment : FsmStateAction
	{
		[UIHint(UIHint.Comment)]
		public string comment;

		public override void Reset()
		{
			comment = "";
		}

		public override void OnEnter()
		{
			Finish();
		}
	}
}