// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Get the XYZ channels of a Vector3 Variable and store them in Float Variables.")]
	public class GetVector3XYZ : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;
		[UIHint(UIHint.Variable)]
		public FsmFloat storeX;		
		[UIHint(UIHint.Variable)]
		public FsmFloat storeY;		
		[UIHint(UIHint.Variable)]
		public FsmFloat storeZ;	
		public bool everyFrame;
		
		public override void Reset()
		{
			vector3Variable = null;
			storeX = null;
			storeY = null;
			storeZ = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetVector3XYZ();
			
			if(!everyFrame)
				Finish();
		}
		
		public override void OnUpdate ()
		{
			DoGetVector3XYZ();
		}
		
		void DoGetVector3XYZ()
		{
			if (vector3Variable == null) return;
			
			if (storeX != null)
				storeX.Value = vector3Variable.Value.x;

			if (storeY != null)
				storeY.Value = vector3Variable.Value.y;

			if (storeZ != null)
				storeZ.Value = vector3Variable.Value.z;
		}
	}
}