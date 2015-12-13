// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Select a Random Vector3 from a Vector3 array.")]
	public class SelectRandomVector3 : FsmStateAction
	{
		[CompoundArray("Vectors", "Vector", "Weight")]
		public FsmVector3[] vector3Array;
		[HasFloatSlider(0, 1)]
		public FsmFloat[] weights;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeVector3;
		
		public override void Reset()
		{
			vector3Array = new FsmVector3[3];
			weights = new FsmFloat[] {1,1,1};
			storeVector3 = null;
		}

		public override void OnEnter()
		{
			DoSelectRandomColor();
			Finish();
		}
		
		void DoSelectRandomColor()
		{
			if (vector3Array == null) return;
			if (vector3Array.Length == 0) return;
			if (storeVector3 == null) return;

			int randomIndex = ActionHelpers.GetRandomWeightedIndex(weights);
			
			if (randomIndex != -1)
			{
				storeVector3.Value = vector3Array[randomIndex].Value;
			}
		}
	}
}