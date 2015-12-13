// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Adds a value to Vector3 Variable.")]
	public class Vector3Add : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;
		[RequiredField]
		public FsmVector3 addVector;
		public bool everyFrame;
		public bool perSecond;

		public override void Reset()
		{
			vector3Variable = null;
			addVector = new FsmVector3 { UseVariable = true };
			everyFrame = false;
			perSecond = false;
		}

		public override void OnEnter()
		{
			DoVector3Add();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			DoVector3Add();
		}
		
		void DoVector3Add()
		{
			if(perSecond)
				vector3Variable.Value = vector3Variable.Value + addVector.Value * Time.deltaTime;
			else
				vector3Variable.Value = vector3Variable.Value + addVector.Value;
				
		}
	}
}

