// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

#if (UNITY_EDITOR || UNITY_IPHONE || UNITY_ANDROID)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Plays a full-screen movie on a handheld device. Please consult the Unity docs for Handheld.PlayFullScreenMovie for proper usage.")]
	public class DevicePlayFullScreenMovie : FsmStateAction
	{
        [RequiredField]
		[Tooltip("Note that player will stream movie directly from the iPhone disc, therefore you have to provide movie as a separate files and not as an usual asset.\nYou will have to create a folder named StreamingAssets inside your Unity project (inside your Assets folder). Store your movies inside that folder. Unity will automatically copy contents of that folder into the iPhone application bundle.")]
		public FsmString moviePath;

		[RequiredField]
		[Tooltip("This action will initiate a transition that fades the screen from your current content to the designated background color of the player. When playback finishes, the player uses another fade effect to transition back to your content.")]
		public FsmColor fadeColor;

		[Tooltip("Options for displaying movie playback controls. See Unity docs.")]
		public FullScreenMovieControlMode movieControlMode;

		[Tooltip("Scaling modes for displaying movies.. See Unity docs.")]
		public FullScreenMovieScalingMode movieScalingMode;

        public override void Reset()
		{
			moviePath = "";
			fadeColor = Color.black;

			movieControlMode = FullScreenMovieControlMode.Full;
			movieScalingMode = FullScreenMovieScalingMode.AspectFit;
		}

		public override void OnEnter()
		{
            Handheld.PlayFullScreenMovie(moviePath.Value, fadeColor.Value, movieControlMode, movieScalingMode);
        }
	}
}

#endif