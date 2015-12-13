// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of an Object Variable in the PlayMaker Log Window.")]
	public class DebugObject : BaseLogAction
	{
        [Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
        [Tooltip("The Object variable to debug.")]
		public FsmObject fsmObject;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			fsmObject = null;
            base.Reset();
		}

		public override void OnEnter()
		{
			var text = "None";
			
			if (!fsmObject.IsNone)
			{
				text = fsmObject.Name + ": " + fsmObject;
			}
			
			ActionHelpers.DebugLog(Fsm, logLevel, text, sendToUnityLog);
			
			Finish();
		}
	}
}