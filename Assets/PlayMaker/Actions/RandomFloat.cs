// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets a Float Variable to a random value between Min/Max.")]
	public class RandomFloat : FsmStateAction
	{
		[RequiredField]
		public FsmFloat min;
		[RequiredField]
		public FsmFloat max;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;

		public override void Reset()
		{
			min = 0f;
			max = 1f;
			storeResult = null;
		}

		public override void OnEnter()
		{
			storeResult.Value = Random.Range(min.Value, max.Value);
			
			Finish();
		}
	}
}