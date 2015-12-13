// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Gets the rotation of the device around its z axis (into the screen). For example when you steer with the iPhone in a driving game.")]
	public class GetDeviceRoll : FsmStateAction
	{
		public enum BaseOrientation
		{
			Portrait,
			LandscapeLeft,
			LandscapeRight
		}
		
		[Tooltip("How the user is expected to hold the device (where angle will be zero).")]
		public BaseOrientation baseOrientation;
		[UIHint(UIHint.Variable)]
		public FsmFloat storeAngle;
		public FsmFloat limitAngle;
		public FsmFloat smoothing;
		public bool everyFrame;
		
		private float lastZAngle;
		
		public override void Reset()
		{
			baseOrientation = BaseOrientation.LandscapeLeft;
			storeAngle = null;
			limitAngle = new FsmFloat { UseVariable = true };
			smoothing = 5f;
			everyFrame = true;
		}
		
		public override void OnEnter()
		{
			DoGetDeviceRoll();
			
			if (!everyFrame)
				Finish();
		}
		

		public override void OnUpdate()
		{
			DoGetDeviceRoll();
		}
		
		void DoGetDeviceRoll()
		{
			float x = Input.acceleration.x;
			float y = Input.acceleration.y;
			float zAngle = 0;
			
			switch (baseOrientation) 
			{
			case BaseOrientation.Portrait:
				zAngle = -Mathf.Atan2(x, -y);
				break;
			case BaseOrientation.LandscapeLeft:
				zAngle = Mathf.Atan2(y, -x);
				break;
			case BaseOrientation.LandscapeRight:
				zAngle = -Mathf.Atan2(y, x);
				break;
			}
			
			if (!limitAngle.IsNone)
			{
				zAngle = Mathf.Clamp(Mathf.Rad2Deg * zAngle, -limitAngle.Value, limitAngle.Value);
			}
			
			if (smoothing.Value > 0)
			{
				zAngle = Mathf.LerpAngle(lastZAngle, zAngle, smoothing.Value * Time.deltaTime);
			}
			
			lastZAngle = zAngle;
			
			storeAngle.Value = zAngle;
		}
		
	}
}