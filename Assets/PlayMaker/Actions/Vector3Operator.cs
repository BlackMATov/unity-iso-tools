// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Performs most possible operations on 2 Vector3: Dot product, Cross product, Distance, Angle, Project, Reflect, Add, Subtract, Multiply, Divide, Min, Max")]
	public class Vector3Operator : FsmStateAction
	{
		public enum Vector3Operation
		{
			DotProduct,
			CrossProduct,
			Distance,
			Angle,
			Project,
			Reflect,
			Add,
			Subtract,
			Multiply,
			Divide,
			Min,
			Max
		}

		[RequiredField]
		public FsmVector3 vector1;
		[RequiredField]
		public FsmVector3 vector2;
		public Vector3Operation operation = Vector3Operation.Add;

		[UIHint(UIHint.Variable)]
		public FsmVector3 storeVector3Result;

		[UIHint(UIHint.Variable)]
		public FsmFloat storeFloatResult;

		public bool everyFrame;

		public override void Reset()
		{
			vector1 = null;
			vector2 = null;
			operation = Vector3Operation.Add;
			storeVector3Result = null;
			storeFloatResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoVector3Operator();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoVector3Operator();
		}

		void DoVector3Operator()
		{
			var v1 = vector1.Value;
			var v2 = vector2.Value;

			switch (operation)
			{
				case Vector3Operation.DotProduct:
					storeFloatResult.Value = Vector3.Dot(v1, v2);
					break;

				case Vector3Operation.CrossProduct:
					storeVector3Result.Value = Vector3.Cross(v1, v2);
					break;

				case Vector3Operation.Distance:
					storeFloatResult.Value = Vector3.Distance(v1, v2);
					break;

				case Vector3Operation.Angle:
					storeFloatResult.Value = Vector3.Angle(v1, v2);
					break;

				case Vector3Operation.Project:
					storeVector3Result.Value = Vector3.Project(v1, v2);
					break;

				case Vector3Operation.Reflect:
					storeVector3Result.Value = Vector3.Reflect(v1, v2);
					break;

				case Vector3Operation.Add:
					storeVector3Result.Value = v1 + v2;
					break;

				case Vector3Operation.Subtract:
					storeVector3Result.Value = v1 - v2;
					break;

				case Vector3Operation.Multiply:
					// I know... this is a far reach and not usefull in 99% of cases. 
					// I do use it when I use vector3 as arrays recipients holding something else than a position in space.
					var multResult = Vector3.zero;
					multResult.x = v1.x * v2.x;
					multResult.y = v1.y * v2.y;
					multResult.z = v1.z * v2.z;
					storeVector3Result.Value = multResult;
					break;

				case Vector3Operation.Divide: // I know... this is a far reach and not usefull in 99% of cases.
					// I do use it when I use vector3 as arrays recipients holding something else than a position in space.
					var divResult = Vector3.zero;
					divResult.x = v1.x / v2.x;
					divResult.y = v1.y / v2.y;
					divResult.z = v1.z / v2.z;
					storeVector3Result.Value = divResult;
					break;

				case Vector3Operation.Min:
					storeVector3Result.Value = Vector3.Min(v1, v2);
					break;

				case Vector3Operation.Max:
					storeVector3Result.Value = Vector3.Max(v1, v2);
					break;
			}
		}
	}
}