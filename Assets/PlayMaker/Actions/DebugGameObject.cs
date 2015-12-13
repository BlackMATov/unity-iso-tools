// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of a Game Object Variable in the PlayMaker Log Window.")]
	public class DebugGameObject : BaseLogAction
	{
        [Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
        [Tooltip("The GameObject variable to debug.")]
		public FsmGameObject gameObject;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			gameObject = null;
            base.Reset();
		}

		public override void OnEnter()
		{
			var text = "None";
			
			if (!gameObject.IsNone)
			{
				text = gameObject.Name + ": " + gameObject;
			}
			
			ActionHelpers.DebugLog(Fsm, logLevel, text, sendToUnityLog);
			
			Finish();
		}
	}
}