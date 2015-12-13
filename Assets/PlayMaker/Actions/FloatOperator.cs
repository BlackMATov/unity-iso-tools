// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Performs math operations on 2 Floats: Add, Subtract, Multiply, Divide, Min, Max.")]
	public class FloatOperator : FsmStateAction
	{
		public enum Operation
		{
			Add,
			Subtract,
			Multiply,
			Divide,
			Min,
			Max
		}

		[RequiredField]
        [Tooltip("The first float.")]
		public FsmFloat float1;

		[RequiredField]
        [Tooltip("The second float.")]
		public FsmFloat float2;

        [Tooltip("The math operation to perform on the floats.")]
		public Operation operation = Operation.Add;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result of the operation in a float variable.")]
        public FsmFloat storeResult;
		
        [Tooltip("Repeat every frame. Useful if the variables are changing.")]
        public bool everyFrame;

		public override void Reset()
		{
			float1 = null;
			float2 = null;
			operation = Operation.Add;
			storeResult = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoFloatOperator();
			
			if (!everyFrame)
			{
			    Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoFloatOperator();
		}
		
		void DoFloatOperator()
		{
			var v1 = float1.Value;
			var v2 = float2.Value;

			switch (operation)
			{
				case Operation.Add:
					storeResult.Value = v1 + v2;
					break;

				case Operation.Subtract:
					storeResult.Value = v1 - v2;
					break;

				case Operation.Multiply:
					storeResult.Value = v1 * v2;
					break;

				case Operation.Divide:
					storeResult.Value = v1 / v2;
					break;

				case Operation.Min:
					storeResult.Value = Mathf.Min(v1, v2);
					break;

				case Operation.Max:
					storeResult.Value = Mathf.Max(v1, v2);
					break;
			}
		}
	}
}