// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Select a Random String from an array of Strings.")]
	public class SelectRandomString : FsmStateAction
	{
		[CompoundArray("Strings", "String", "Weight")]
		public FsmString[] strings;
		[HasFloatSlider(0, 1)]
		public FsmFloat[] weights;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString storeString;
		
		public override void Reset()
		{
			strings = new FsmString[3];
			weights = new FsmFloat[] {1,1,1};
			storeString = null;
		}

		public override void OnEnter()
		{
			DoSelectRandomString();
			Finish();
		}
		
		void DoSelectRandomString()
		{
			if (strings == null) return;
			if (strings.Length == 0) return;
			if (storeString == null) return;

			int randomIndex = ActionHelpers.GetRandomWeightedIndex(weights);
			
			if (randomIndex != -1)
			{
				storeString.Value = strings[randomIndex].Value;
			}
		}
	}
}