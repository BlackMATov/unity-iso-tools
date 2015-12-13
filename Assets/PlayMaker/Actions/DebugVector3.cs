// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of a Vector3 Variable in the PlayMaker Log Window.")]
	public class DebugVector3 : BaseLogAction
	{
        [Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
        [Tooltip("The Vector3 variable to debug.")]
		public FsmVector3 vector3Variable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			vector3Variable = null;
            base.Reset();
		}

		public override void OnEnter()
		{
			var text = "None";
			
			if (!vector3Variable.IsNone)
			{
				text = vector3Variable.Name + ": " + vector3Variable.Value;
			}

			ActionHelpers.DebugLog(Fsm, logLevel, text, sendToUnityLog);

			Finish();
		}
	}
}