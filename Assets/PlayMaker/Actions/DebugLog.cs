// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Sends a log message to the PlayMaker Log Window.")]
	public class DebugLog : BaseLogAction
	{
        [Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

        [Tooltip("Text to send to the log.")]
		public FsmString text;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			text = "";
            base.Reset();
		}

		public override void OnEnter()
		{
		    if (!string.IsNullOrEmpty(text.Value))
		    {
		        ActionHelpers.DebugLog(Fsm, logLevel, text.Value, sendToUnityLog);
		    }

			Finish();
		}
	}
}