// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets a Bool Variable to True or False randomly.")]
	public class RandomBool : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		public override void Reset()
		{
			storeResult = null;
		}

		public override void OnEnter()
		{
			storeResult.Value = Random.Range(0, 100) < 50;
			
			Finish();
		}
	}
}