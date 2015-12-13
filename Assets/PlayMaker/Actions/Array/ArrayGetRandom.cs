// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Get a Random item from an Array.")]
	public class ArrayGetRandom : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Array to use.")]
		public FsmArray array;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the value in a variable.")]
        [MatchElementType("array")]
        public FsmVar storeValue;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			array = null;
			storeValue =null;
            everyFrame = false;
		}
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			DoGetRandomValue();
			
			if (!everyFrame)
			{
				Finish();
			}
			
		}
		
		public override void OnUpdate()
		{
			DoGetRandomValue();
			
		}
		
		private void DoGetRandomValue()
		{
			if (storeValue.IsNone)
			{
				return;
			}

			storeValue.SetValue(array.Get(Random.Range(0,array.Length)));	
		}

		
	}
}

