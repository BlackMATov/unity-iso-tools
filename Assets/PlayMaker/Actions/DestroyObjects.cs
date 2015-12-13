// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Destroys GameObjects in an array.")]
	public class DestroyObjects : FsmStateAction
	{
        [RequiredField]
        [ArrayEditor(VariableType.GameObject)]
        [Tooltip("The GameObjects to destroy.")]
        public FsmArray gameObjects;

		[HasFloatSlider(0, 5)]
		[Tooltip("Optional delay before destroying the Game Objects.")]
		public FsmFloat delay;

		[Tooltip("Detach children before destroying the Game Objects.")]
		public FsmBool detachChildren;

		public override void Reset()
		{
			gameObjects = null;
			delay = 0;
		}

		public override void OnEnter()
		{
            var goArray = gameObjects.Values as GameObject[];
		    if (goArray != null)
		    {
                foreach (var go in goArray)
                {
                    if (go != null)
                    {
                        if (delay.Value <= 0)
                        {
                            Object.Destroy(go);
                        }
                        else
                        {
                            Object.Destroy(go, delay.Value);
                        }

                        if (detachChildren.Value)
                        {
                            go.transform.DetachChildren();
                        }
                    }
                }
		        
		    }

			Finish();
		}
	}
}