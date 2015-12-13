// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Transforms position from world space into screen space. NOTE: Uses the MainCamera!")]
	public class WorldToScreenPoint : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("World position to transform into screen coordinates.")]
		public FsmVector3 worldPosition;
		[Tooltip("World X position.")]
		public FsmFloat worldX;
		[Tooltip("World Y position.")]
		public FsmFloat worldY;
		[Tooltip("World Z position.")]
		public FsmFloat worldZ;
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the screen position in a Vector3 Variable. Z will equal zero.")]
		public FsmVector3 storeScreenPoint;
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the screen X position in a Float Variable.")]
		public FsmFloat storeScreenX;
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the screen Y position in a Float Variable.")]
		public FsmFloat storeScreenY;
		[Tooltip("Normalize screen coordinates (0-1). Otherwise coordinates are in pixels.")]
		public FsmBool normalize;
		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			worldPosition = null;
			// default axis to variable dropdown with None selected.
			worldX = new FsmFloat { UseVariable = true };
			worldY = new FsmFloat { UseVariable = true };
			worldZ = new FsmFloat { UseVariable = true };
			storeScreenPoint = null;
			storeScreenX = null;
			storeScreenY = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoWorldToScreenPoint();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}

		public override void OnUpdate()
		{
			DoWorldToScreenPoint();
		}

		void DoWorldToScreenPoint()
		{
			if (Camera.main == null)
			{
				LogError("No MainCamera defined!");
				Finish();
				return;
			}

			var position = Vector3.zero;

			if(!worldPosition.IsNone) position = worldPosition.Value;

			if (!worldX.IsNone) position.x = worldX.Value;
			if (!worldY.IsNone) position.y = worldY.Value;
			if (!worldZ.IsNone) position.z = worldZ.Value;
			
			position = Camera.main.WorldToScreenPoint(position);
			
			if (normalize.Value)
			{
				position.x /= Screen.width;
				position.y /= Screen.height;
			}
			
			storeScreenPoint.Value = position;
			storeScreenX.Value = position.x;
			storeScreenY.Value = position.y;
		}


	}
}