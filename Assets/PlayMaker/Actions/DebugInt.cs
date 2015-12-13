// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of an Integer Variable in the PlayMaker Log Window.")]
	public class DebugInt : BaseLogAction
	{
        [Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
        [Tooltip("The Int variable to debug.")]
		public FsmInt intVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			intVariable = null;
		}

		public override void OnEnter()
		{
			var text = "None";
			
			if (!intVariable.IsNone)
			{
				text = intVariable.Name + ": " + intVariable.Value;
			}

			ActionHelpers.DebugLog(Fsm, logLevel, text, sendToUnityLog);

			Finish();
		}
	}
}