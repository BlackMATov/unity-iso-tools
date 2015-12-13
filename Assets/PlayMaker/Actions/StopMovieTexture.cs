// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

#if !(UNITY_IPHONE || UNITY_ANDROID || UNITY_FLASH || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE || UNITY_BLACKBERRY || UNITY_METRO || UNITY_WP8 || UNITY_PSM || UNITY_WEBGL)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Movie)]
	[Tooltip("Stops playing the Movie Texture, and rewinds it to the beginning.")]
	public class StopMovieTexture : FsmStateAction
	{
		[RequiredField]
		[ObjectType(typeof(MovieTexture))]
		public FsmObject movieTexture;

		public override void Reset()
		{
			movieTexture = null;
		}

		public override void OnEnter()
		{
			var movie = movieTexture.Value as MovieTexture;

			if (movie != null)
			{
				movie.Stop();
			}

			Finish();
		}
	}
}

#endif