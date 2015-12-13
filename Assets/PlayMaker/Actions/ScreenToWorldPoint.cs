// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Transforms position from screen space into world space. NOTE: Uses the MainCamera!")]
	public class ScreenToWorldPoint : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Screen position as a vector.")]
		public FsmVector3 screenVector;
		[Tooltip("Screen X position in pixels or normalized. See Normalized.")]
		public FsmFloat screenX;
		[Tooltip("Screen X position in pixels or normalized. See Normalized.")]
		public FsmFloat screenY;
		[Tooltip("Distance into the screen in world units.")]
		public FsmFloat screenZ;
		[Tooltip("If true, X/Y coordinates are considered normalized (0-1), otherwise they are expected to be in pixels")]
		public FsmBool normalized;
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the world position in a vector3 variable.")]
		public FsmVector3 storeWorldVector;
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the world X position in a float variable.")]
		public FsmFloat storeWorldX;
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the world Y position in a float variable.")]
		public FsmFloat storeWorldY;
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the world Z position in a float variable.")]
		public FsmFloat storeWorldZ;
		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			screenVector = null;
			// default axis to variable dropdown with None selected.
			screenX = new FsmFloat { UseVariable = true };
			screenY = new FsmFloat { UseVariable = true };
			screenZ = 1;
			normalized = false;
			storeWorldVector = null;
			storeWorldX = null;
			storeWorldY = null;
			storeWorldZ = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoScreenToWorldPoint();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}

		public override void OnUpdate()
		{
			DoScreenToWorldPoint();
		}

		void DoScreenToWorldPoint()
		{
			if (Camera.main == null)
			{
				LogError("No MainCamera defined!");
				Finish();
				return;
			}

			var position = Vector3.zero;

			if(!screenVector.IsNone) position = screenVector.Value;

			if (!screenX.IsNone) position.x = screenX.Value;
			if (!screenY.IsNone) position.y = screenY.Value;
			if (!screenZ.IsNone) position.z = screenZ.Value;
			
			if (normalized.Value)
			{
				position.x *= Screen.width;
				position.y *= Screen.height;
			}
			
			position = Camera.main.ScreenToWorldPoint(position);

			storeWorldVector.Value = position;
			storeWorldX.Value = position.x;
			storeWorldY.Value = position.y;
			storeWorldZ.Value = position.z;
		}


	}
}