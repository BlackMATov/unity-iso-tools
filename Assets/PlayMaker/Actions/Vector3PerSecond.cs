// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Multiplies a Vector3 variable by Time.deltaTime. Useful for frame rate independent motion.")]
	public class Vector3PerSecond : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;
		public bool everyFrame;

		public override void Reset()
		{
			vector3Variable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			vector3Variable.Value = vector3Variable.Value * Time.deltaTime;
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			vector3Variable.Value = vector3Variable.Value * Time.deltaTime;
		}
	}
}

