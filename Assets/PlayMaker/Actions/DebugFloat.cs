// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of a Float Variable in the PlayMaker Log Window.")]
	public class DebugFloat : BaseLogAction
	{
        [Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;
		
        [UIHint(UIHint.Variable)]
        [Tooltip("The Float variable to debug.")]
		public FsmFloat floatVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			floatVariable = null;
            base.Reset();
		}

		public override void OnEnter()
		{
			string text = "None";
			
			if (!floatVariable.IsNone)
			{
				text = floatVariable.Name + ": " + floatVariable.Value;
			}

			ActionHelpers.DebugLog(Fsm, logLevel, text, sendToUnityLog);

			Finish();
		}
	}
}