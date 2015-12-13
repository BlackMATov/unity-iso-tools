// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
// Added Ignore Owner option. Thanks Nueral Echo: http://hutonggames.com/playmakerforum/index.php?topic=71.0

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Finds the closest object to the specified Game Object.\nOptionally filter by Tag and Visibility.")]
	public class FindClosest : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to measure from.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[UIHint(UIHint.Tag)]
		[Tooltip("Only consider objects with this Tag. NOTE: It's generally a lot quicker to find objects with a Tag!")]
		public FsmString withTag;
		
		[Tooltip("If checked, ignores the object that owns this FSM.")]
		public FsmBool ignoreOwner;

		[Tooltip("Only consider objects visible to the camera.")]
		public FsmBool mustBeVisible;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the closest object.")]
		public FsmGameObject storeObject;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the distance to the closest object.")]
		public FsmFloat storeDistance;
		
		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		
		public override void Reset()
		{
			gameObject = null;	
			withTag = "Untagged";
			ignoreOwner = true;
			mustBeVisible = false;
			storeObject = null;
			storeDistance = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFindClosest();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoFindClosest();
		}

		void DoFindClosest()
		{
			var go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;

			GameObject[] objects; // objects to consider

			if (string.IsNullOrEmpty(withTag.Value) || withTag.Value == "Untagged")
			{
				objects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
			}
			else
			{
				objects = GameObject.FindGameObjectsWithTag(withTag.Value);
			}	
			
			GameObject closestObj = null;
			var closestDist = Mathf.Infinity;

			foreach (var obj in objects)
			{
				if (ignoreOwner.Value && obj == Owner)
				{
					continue;
				}
				
				if (mustBeVisible.Value && !ActionHelpers.IsVisible(obj))
				{
					continue;
				}
				
				var dist = (go.transform.position - obj.transform.position).sqrMagnitude;
				if (dist < closestDist)
				{
					closestDist = dist;
					closestObj = obj;
				}
			}

			storeObject.Value = closestObj;
			
			if (!storeDistance.IsNone)
			{
				storeDistance.Value = Mathf.Sqrt(closestDist);
			}
		}
	}
}