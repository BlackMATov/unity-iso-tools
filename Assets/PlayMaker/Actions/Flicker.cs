// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Effects)]
	[Tooltip("Randomly flickers a Game Object on/off.")]
	public class Flicker : ComponentAction<Renderer>
	{
		[RequiredField]
        [Tooltip("The GameObject to flicker.")]
		public FsmOwnerDefault gameObject;
		
		[HasFloatSlider(0, 1)]
        [Tooltip("The frequency of the flicker in seconds.")]
		public FsmFloat frequency;
		
		[HasFloatSlider(0, 1)]
        [Tooltip("Amount of time flicker is On (0-1). E.g. Use 0.95 for an occasional flicker.")]
		public FsmFloat amountOn;

        [Tooltip("Only effect the renderer, leaving other components active.")]
		public bool rendererOnly;

        [Tooltip("Ignore time scale. Useful if flickering UI when the game is paused.")]
		public bool realTime;
		
		private float startTime;
		private float timer;
		
		public override void Reset()
		{
			gameObject = null;
			frequency = 0.1f;
			amountOn = 0.5f;
			rendererOnly = true;	
			realTime = false;
		}
	
		public override void OnEnter()
		{
			startTime = FsmTime.RealtimeSinceStartup;
			timer = 0f;
		}
		
		public override void OnUpdate()
		{
			// get target

			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}
			
			// update time
			
			if (realTime)
			{
				timer = FsmTime.RealtimeSinceStartup - startTime;
			}
			else
			{
				timer += Time.deltaTime;
			}
			
			if (timer > frequency.Value)
			{
				var on = Random.Range(0f,1f) < amountOn.Value;

				// do flicker
				
				if (rendererOnly)
				{
					if (UpdateCache(go))
					{
						renderer.enabled = on;
					}
				}
				else
                {
#if UNITY_3_5 || UNITY_3_4
                    go.active = on;
#else				
                    go.SetActive(on);
#endif
                }
				
				// reset timer
				
				startTime = timer;
				timer = 0;
			}
		}


		
	}
}