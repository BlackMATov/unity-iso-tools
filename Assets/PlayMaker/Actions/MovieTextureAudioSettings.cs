// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

#if !(UNITY_IPHONE || UNITY_ANDROID || UNITY_FLASH || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE || UNITY_BLACKBERRY || UNITY_WP8 || UNITY_PSM || UNITY_WEBGL)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Movie)]
	[Tooltip("Sets the Game Object as the Audio Source associated with the Movie Texture. The Game Object must have an AudioSource Component.")]
	public class MovieTextureAudioSettings : FsmStateAction
	{
		[RequiredField]
		[ObjectType(typeof(MovieTexture))]
		public FsmObject movieTexture;

		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
		public FsmGameObject gameObject;
		
		// this gets overridden by AudioPlay...
		//public FsmFloat volume;

		public override void Reset()
		{
			movieTexture = null;
			gameObject = null;
			//volume = 1;
		}

		public override void OnEnter()
		{
			var movie = movieTexture.Value as MovieTexture;

			if (movie != null && gameObject.Value != null)
			{
			    var audio = gameObject.Value.GetComponent<AudioSource>();
				if (audio != null)
				{
					audio.clip = movie.audioClip;
					
					//if (!volume.IsNone)
					//	audio.volume = volume.Value;
				}
			}
				
			Finish();
		}
	}
}

#endif

