// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Performs math operation on 2 Integers: Add, Subtract, Multiply, Divide, Min, Max.")]
	public class IntOperator : FsmStateAction
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
		public FsmInt integer1;
		[RequiredField]
		public FsmInt integer2;
		public Operation operation = Operation.Add;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt storeResult;
		public bool everyFrame;

		public override void Reset()
		{
			integer1 = null;
			integer2 = null;
			operation = Operation.Add;
			storeResult = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoIntOperator();
			
			if (!everyFrame)
				Finish();
		}
		
		// NOTE: very frame rate dependent!
		public override void OnUpdate()
		{
			DoIntOperator();
		}
		
		void DoIntOperator()
		{
			int v1 = integer1.Value;
			int v2 = integer2.Value;

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