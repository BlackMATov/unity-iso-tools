// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Perform a raycast into the scene using screen coordinates and stores the results. Use Ray Distance to set how close the camera must be to pick the object. NOTE: Uses the MainCamera!")]
	public class ScreenPick : FsmStateAction
	{
		[Tooltip("A Vector3 screen position. Commonly stored by other actions.")]
		public FsmVector3 screenVector;
		[Tooltip ("X position on screen.")]
		public FsmFloat screenX;
		[Tooltip ("Y position on screen.")]
		public FsmFloat screenY;
		[Tooltip("Are the supplied screen coordinates normalized (0-1), or in pixels.")]
		public FsmBool normalized;
		[RequiredField]
		public FsmFloat rayDistance = 100f;
		[UIHint(UIHint.Variable)]
		public FsmBool storeDidPickObject;
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;
		[UIHint(UIHint.Variable)]
		public FsmVector3 storePoint;
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeNormal;
		[UIHint(UIHint.Variable)]
		public FsmFloat storeDistance;
		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;
		public bool everyFrame;
		
		public override void Reset()
		{
			screenVector = new FsmVector3 { UseVariable = true };
			screenX = new FsmFloat { UseVariable = true };
			screenY = new FsmFloat { UseVariable = true };
			normalized = false;
			rayDistance = 100f;
			storeDidPickObject = null;
			storeGameObject = null;
			storePoint = null;
			storeNormal = null;
			storeDistance = null;
			layerMask = new FsmInt[0];
			invertMask = false;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoScreenPick();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoScreenPick();
		}

		void DoScreenPick()
		{
			if (Camera.main == null)
			{
				LogError("No MainCamera defined!");
				Finish();
				return;
			}

			var rayStart = Vector3.zero;
			
			if (!screenVector.IsNone) rayStart = screenVector.Value;
			if (!screenX.IsNone) rayStart.x = screenX.Value;
			if (!screenY.IsNone) rayStart.y = screenY.Value;
			
			if (normalized.Value)
			{
				rayStart.x *= Screen.width;
				rayStart.y *= Screen.height;
			}
			
			RaycastHit hitInfo;
			var ray = Camera.main.ScreenPointToRay(rayStart);
			Physics.Raycast(ray, out hitInfo, rayDistance.Value, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));
			
			var didPick = hitInfo.collider != null;
			storeDidPickObject.Value = didPick;
			
			if (didPick)
			{
				storeGameObject.Value = hitInfo.collider.gameObject;
				storeDistance.Value = hitInfo.distance;
				storePoint.Value = hitInfo.point;
				storeNormal.Value = hitInfo.normal;
			}
			else
			{
				// TODO: not sure if this is the right strategy...
				storeGameObject.Value = null;
				storeDistance = Mathf.Infinity;
				storePoint.Value = Vector3.zero;
				storeNormal.Value = Vector3.zero;
			}
			
		}
	}
}