// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

#if !(UNITY_FLASH || UNITY_METRO)

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Application)]
	[Tooltip("Saves a Screenshot to the users MyPictures folder. TIP: Can be useful for automated testing and debugging.")]
	public class TakeScreenshot : FsmStateAction
	{
		[RequiredField]
		public FsmString filename;
		public bool autoNumber;

		private int screenshotCount;

		public override void Reset()
		{
			filename = "";
			autoNumber = false;
		}

		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(filename.Value)) return;

			string screenshotPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)+"/";
			string screenshotFullPath = screenshotPath + filename.Value + ".png";

			//Debug.Log(screenshotFullPath);

			if (autoNumber)
			{
				while (System.IO.File.Exists(screenshotFullPath)) 
				{
					screenshotCount++;
					screenshotFullPath = screenshotPath + filename.Value + screenshotCount + ".png";
				} 
			}

			Application.CaptureScreenshot(screenshotFullPath);
			
			Finish();
		}
	}
}

#endif