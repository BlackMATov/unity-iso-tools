// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Select a random Color from an array of Colors.")]
	public class SelectRandomColor : FsmStateAction
	{
		[CompoundArray("Colors", "Color", "Weight")]
		public FsmColor[] colors;
		[HasFloatSlider(0, 1)]
		public FsmFloat[] weights;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmColor storeColor;

		public override void Reset()
		{
			colors = new FsmColor[3];
			weights = new FsmFloat[] {1,1,1};
			storeColor = null;
		}

		public override void OnEnter()
		{
			DoSelectRandomColor();
			Finish();
		}
		
		void DoSelectRandomColor()
		{
			if (colors == null) return;
			if (colors.Length == 0) return;
			if (storeColor == null) return;
			
			int randomIndex = ActionHelpers.GetRandomWeightedIndex(weights);
			
			if (randomIndex != -1)
			{
				storeColor.Value = colors[randomIndex].Value;	
			}
		}
	}
}