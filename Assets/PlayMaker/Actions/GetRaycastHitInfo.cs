// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets info on the last Raycast and store in variables.")]
	public class GetRaycastHitInfo : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the GameObject hit by the last Raycast and store it in a variable.")]
		public FsmGameObject gameObjectHit;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the world position of the ray hit point and store it in a variable.")]
		[Title("Hit Point")]
		public FsmVector3 point;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the normal at the hit point and store it in a variable.")]
		public FsmVector3 normal;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the distance along the ray to the hit point and store it in a variable.")]
		public FsmFloat distance;

        [Tooltip("Repeat every frame.")]
	    public bool everyFrame;

		public override void Reset()
		{
			gameObjectHit = null;
			point = null;
			normal = null;
			distance = null;
		    everyFrame = false;
		}

		void StoreRaycastInfo()
		{
			if (Fsm.RaycastHitInfo.collider != null)
			{
				gameObjectHit.Value = Fsm.RaycastHitInfo.collider.gameObject;
				point.Value = Fsm.RaycastHitInfo.point;
				normal.Value = Fsm.RaycastHitInfo.normal;
				distance.Value = Fsm.RaycastHitInfo.distance;
			}
		}

		public override void OnEnter()
		{
			StoreRaycastInfo();
			
            if (!everyFrame)
            {
                Finish();
            }
		}

        public override void OnUpdate()
        {
            StoreRaycastInfo();
        }
	}
}