// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Gets info on a touch event.")]
	public class GetTouchInfo : FsmStateAction
	{
		[Tooltip("Filter by a Finger ID. You can store a Finger ID in other Touch actions, e.g., Touch Event.")]
		public FsmInt fingerId;
		[Tooltip("If true, all screen coordinates are returned normalized (0-1), otherwise in pixels.")]
		public FsmBool normalize;
		[UIHint(UIHint.Variable)]
		public FsmVector3 storePosition;
		[UIHint(UIHint.Variable)]
		public FsmFloat storeX;
		[UIHint(UIHint.Variable)]
		public FsmFloat storeY;
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeDeltaPosition;
		[UIHint(UIHint.Variable)]
		public FsmFloat storeDeltaX;
		[UIHint(UIHint.Variable)]
		public FsmFloat storeDeltaY;
		[UIHint(UIHint.Variable)]
		public FsmFloat storeDeltaTime;
		[UIHint(UIHint.Variable)]
		public FsmInt storeTapCount;

		public bool everyFrame = true;
		
		float screenWidth;
		float screenHeight;
		
		public override void Reset()
		{
			fingerId = new FsmInt { UseVariable = true };
			normalize = true;
			storePosition = null;
			storeDeltaPosition = null;
			storeDeltaTime = null;
			storeTapCount = null;
			everyFrame = true;
		}
		
		public override void OnEnter()
		{
			screenWidth = Screen.width;
			screenHeight = Screen.height;

			DoGetTouchInfo();

			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoGetTouchInfo();
		}
		
		void DoGetTouchInfo()
		{
			if (Input.touchCount > 0)
			{
				foreach (var touch in Input.touches)
				{
					if (fingerId.IsNone || touch.fingerId == fingerId.Value)
					{
						float x = normalize.Value == false ? touch.position.x : touch.position.x / screenWidth;
						float y = normalize.Value == false ? touch.position.y : touch.position.y / screenHeight;
						
						if (!storePosition.IsNone)
						{
							storePosition.Value = new Vector3(x, y, 0);
						}
						
						storeX.Value = x;
						storeY.Value = y;

						float deltax = normalize.Value == false ? touch.deltaPosition.x : touch.deltaPosition.x / screenWidth;
						float deltay = normalize.Value == false ? touch.deltaPosition.y : touch.deltaPosition.y / screenHeight;
						
						if (!storeDeltaPosition.IsNone)
						{
							storeDeltaPosition.Value = new Vector3(deltax, deltay, 0);
						}

						storeDeltaX.Value = deltax;
						storeDeltaY.Value = deltay;
						
						storeDeltaTime.Value = touch.deltaTime;
						storeTapCount.Value = touch.tapCount;
					}
				}
			}
		}
		
		
	}
}