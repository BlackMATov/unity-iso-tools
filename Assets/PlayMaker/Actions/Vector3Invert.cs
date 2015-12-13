// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Reverses the direction of a Vector3 Variable. Same as multiplying by -1.")]
	public class Vector3Invert : FsmStateAction
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
			vector3Variable.Value = vector3Variable.Value * -1;
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			vector3Variable.Value = vector3Variable.Value *  -1;
		}
	}
}

