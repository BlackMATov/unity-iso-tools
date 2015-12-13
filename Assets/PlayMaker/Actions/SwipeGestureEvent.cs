// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// TODO: fairly basic right now
	// should have more options and be more robust, e.g., other fingers.
	
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Sends an event when a swipe is detected.")]
	public class SwipeGestureEvent : FsmStateAction
	{
		[Tooltip("How far a touch has to travel to be considered a swipe. Uses normalized distance (e.g. 1 = 1 screen diagonal distance). Should generally be a very small number.")]
		public FsmFloat minSwipeDistance;
		
		[Tooltip("Event to send when swipe left detected.")]
		public FsmEvent swipeLeftEvent;
		[Tooltip("Event to send when swipe right detected.")]
		public FsmEvent swipeRightEvent;
		[Tooltip("Event to send when swipe up detected.")]
		public FsmEvent swipeUpEvent;
		[Tooltip("Event to send when swipe down detected.")]
		public FsmEvent swipeDownEvent;
		
		// TODO
/*		[UIHint(UIHint.Variable)]
		[Tooltip("Store the speed of the swipe.")]
		public FsmFloat getSpeed;
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the distance the swipe traveled.")]
		public FsmFloat getDistance;*/
		
		float screenDiagonalSize;
		float minSwipeDistancePixels;
		bool touchStarted;
		Vector2 touchStartPos;
		//float touchStartTime;
		
		public override void Reset()
		{
			minSwipeDistance = 0.1f;
			swipeLeftEvent = null;
			swipeRightEvent = null;
			swipeUpEvent = null;
			swipeDownEvent = null;
		}
		
		public override void OnEnter()
		{
			screenDiagonalSize = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
			minSwipeDistancePixels = minSwipeDistance.Value * screenDiagonalSize;
		}
		
		public override void OnUpdate()
		{
			if (Input.touchCount > 0)
			{
				var touch = Input.touches[0];
				
				switch (touch.phase) 
				{
				case TouchPhase.Began:
					
					touchStarted = true;
					touchStartPos = touch.position;
					//touchStartTime = FsmTime.RealtimeSinceStartup;
					
					break;
					
				case TouchPhase.Ended:
					
					if (touchStarted)
					{
						TestForSwipeGesture(touch);
						touchStarted = false;
					}
					
					break;
					
				case TouchPhase.Canceled:
					
					touchStarted = false;
					
					break;
					
				case TouchPhase.Stationary:
					
/*					if (touchStarted)
					{
						// don't want idle time to count towards swipe
						
						touchStartPos = touch.position;
						touchStartTime = FsmTime.RealtimeSinceStartup;
					}*/
					
					break;

				case TouchPhase.Moved:
					
					break;
				}
			}
		}
		
		void TestForSwipeGesture(Touch touch)
		{
			// test min distance
			
			var lastPos = touch.position;
			var distance = Vector2.Distance(lastPos, touchStartPos);
			
			if (distance > minSwipeDistancePixels)
			{
				float dy = lastPos.y - touchStartPos.y;
				float dx = lastPos.x - touchStartPos.x;
				
				float angle = Mathf.Rad2Deg * Mathf.Atan2(dx, dy);
				
				angle = (360 + angle - 45) % 360;

				Debug.Log (angle);
				
				if (angle < 90)
				{
					Fsm.Event(swipeRightEvent);
				}
				else if (angle < 180)
				{
					Fsm.Event(swipeDownEvent);
				}
				else if (angle < 270)
				{
					Fsm.Event(swipeLeftEvent);
				}
				else 
				{
					Fsm.Event(swipeUpEvent);
				}
			}
		}
			
	}
}