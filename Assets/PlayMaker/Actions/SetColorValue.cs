// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Sets the value of a Color Variable.")]
	public class SetColorValue : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmColor colorVariable;
		[RequiredField]
		public FsmColor color;
		public bool everyFrame;

		public override void Reset()
		{
			colorVariable = null;
			color = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetColorValue();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			DoSetColorValue();
		}
		
		void DoSetColorValue()
		{
			if (colorVariable != null)
				colorVariable.Value = color.Value;
		}
	}
}