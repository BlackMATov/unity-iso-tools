// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Clamps the Magnitude of Vector3 Variable.")]
	public class Vector3ClampMagnitude : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;
		[RequiredField]
		public FsmFloat maxLength;
		public bool everyFrame;

		public override void Reset()
		{
			vector3Variable = null;
			maxLength = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoVector3ClampMagnitude();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			DoVector3ClampMagnitude();
		}
		
		void DoVector3ClampMagnitude()
		{
			vector3Variable.Value = Vector3.ClampMagnitude(vector3Variable.Value, maxLength.Value);
		}
	}
}

