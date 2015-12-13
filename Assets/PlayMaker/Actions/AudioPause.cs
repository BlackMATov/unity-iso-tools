// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Pauses playing the Audio Clip played by an Audio Source component on a Game Object.")]
	public class AudioPause : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
        [Tooltip("The GameObject with an Audio Source component.")]
        public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go != null)
			{
			    var audio = go.GetComponent<AudioSource>();
				if (audio != null)
				{
					audio.Pause();
				}
			}
			
			Finish();
		}
	}
}