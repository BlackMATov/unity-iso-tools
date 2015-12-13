// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Convert)]
    [HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=1711.0")]
    [Tooltip("Converts Seconds to a String value representing the time.")]
	public class ConvertSecondsToString : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The seconds variable to convert.")]
		public FsmFloat secondsVariable;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("A string variable to store the time value.")]
		public FsmString stringVariable;
        
		[RequiredField]
		[Tooltip("Format. 0 for days, 1 is for hours, 2 for minutes, 3 for seconds and 4 for milliseconds. 5 for total days, 6 for total hours, 7 for total minutes, 8 for total seconds, 9 for total milliseconds, 10 for two digits milliseconds. so {2:D2} would just show the seconds of the current time, NOT the grand total number of seconds, the grand total of seconds would be {8:F0}")]
        public FsmString format;

		[Tooltip("Repeat every frame. Useful if the seconds variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			secondsVariable = null;
			stringVariable = null;
			everyFrame = false;
            format = "{1:D2}h:{2:D2}m:{3:D2}s:{10}ms";
		}

		public override void OnEnter()
		{
			DoConvertSecondsToString();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoConvertSecondsToString();
		}
		
		void DoConvertSecondsToString()
		{		
			TimeSpan t = TimeSpan.FromSeconds(secondsVariable.Value);
			
			string milliseconds_2D = t.Milliseconds.ToString("D3").PadLeft(2, '0');
			milliseconds_2D = milliseconds_2D.Substring(0,2);
			
    		stringVariable.Value = string.Format(format.Value, 
                        t.Days,
						t.Hours, 
                        t.Minutes, 
                        t.Seconds, 
                        t.Milliseconds,
						t.TotalDays,
						t.TotalHours,
						t.TotalMinutes,
						t.TotalSeconds,
						t.TotalMilliseconds,
						milliseconds_2D);
		}
	}
}