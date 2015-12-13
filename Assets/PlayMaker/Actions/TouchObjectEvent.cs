// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Sends events when an object is touched. Optionally filter by a fingerID. NOTE: Uses the MainCamera!")]
	public class TouchObjectEvent : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Collider))]
		[Tooltip("The Game Object to detect touches on.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("How far from the camera is the Game Object pickable.")]
		public FsmFloat pickDistance;
		
		[Tooltip("Only detect touches that match this fingerID, or set to None.")]
		public FsmInt fingerId;
		
		[ActionSection("Events")]
		
		[Tooltip("Event to send on touch began.")]
		public FsmEvent touchBegan;
		
		[Tooltip("Event to send on touch moved.")]
		public FsmEvent touchMoved;
		
		[Tooltip("Event to send on stationary touch.")]
		public FsmEvent touchStationary;
		
		[Tooltip("Event to send on touch ended.")]
		public FsmEvent touchEnded;
		
		[Tooltip("Event to send on touch cancel.")]
		public FsmEvent touchCanceled;

		[ActionSection("Store Results")]
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the fingerId of the touch.")]
		public FsmInt storeFingerId;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the world position where the object was touched.")]
		public FsmVector3 storeHitPoint;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the surface normal vector where the object was touched.")]
		public FsmVector3 storeHitNormal;

		public override void Reset()
		{
			gameObject = null;
			pickDistance = 100;
			fingerId = new FsmInt { UseVariable = true };

			touchBegan = null;
			touchMoved = null;
			touchStationary = null;
			touchEnded = null;
			touchCanceled = null;

			storeFingerId = null;
			storeHitPoint = null;
			storeHitNormal = null;
		}

		public override void OnUpdate()
		{
			if (Camera.main == null)
			{
				LogError("No MainCamera defined!");
				Finish();
				return;
			}

			if (Input.touchCount > 0)
			{
				var go = Fsm.GetOwnerDefaultTarget(gameObject);
				if (go == null)
				{
					return;
				}

				foreach (var touch in Input.touches)
				{
					if (fingerId.IsNone || touch.fingerId == fingerId.Value)
					{
						var screenPos = touch.position;

						RaycastHit hitInfo;
						Physics.Raycast(Camera.main.ScreenPointToRay(screenPos), out hitInfo, pickDistance.Value);
						
						// Store hitInfo so it can be accessed by other actions
						// E.g., Get Raycast Hit Info
						Fsm.RaycastHitInfo = hitInfo;
						
						if (hitInfo.transform != null)
						{
							if (hitInfo.transform.gameObject == go)
							{
								storeFingerId.Value = touch.fingerId;
								storeHitPoint.Value = hitInfo.point;
								storeHitNormal.Value = hitInfo.normal;

								switch (touch.phase)
								{
									case TouchPhase.Began:
										Fsm.Event(touchBegan);
										return;

									case TouchPhase.Moved:
										Fsm.Event(touchMoved);
										return;

									case TouchPhase.Stationary:
										Fsm.Event(touchStationary);
										return;

									case TouchPhase.Ended:
										Fsm.Event(touchEnded);
										return;

									case TouchPhase.Canceled:
										Fsm.Event(touchCanceled);
										return;
								}
							}
						}
					}
				}
			}
		}
	}
}