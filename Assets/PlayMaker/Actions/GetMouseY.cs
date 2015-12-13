// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets the Y Position of the mouse and stores it in a Float Variable.")]
	public class GetMouseY : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;
		public bool normalize;
		
		public override void Reset()
		{
			storeResult = null;
			normalize = true;
		}

		public override void OnEnter()
		{
			DoGetMouseY();
		}

		public override void OnUpdate()
		{
			DoGetMouseY();
		}
		
		void DoGetMouseY()
		{
			if (storeResult != null)
			{
				float ypos = Input.mousePosition.y;
				
				if (normalize)
					ypos /= Screen.height;
			
				storeResult.Value = ypos;
			}
		}
	}
}

