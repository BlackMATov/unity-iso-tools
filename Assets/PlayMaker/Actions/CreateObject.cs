// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Creates a Game Object, usually from a Prefab.")]
	public class CreateObject : FsmStateAction
	{
		[RequiredField]
		[Tooltip("GameObject to create. Usually a Prefab.")]
		public FsmGameObject gameObject;

		[Tooltip("Optional Spawn Point.")]
		public FsmGameObject spawnPoint;
		
		[Tooltip("Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
		public FsmVector3 position;
		
		[Tooltip("Rotation. NOTE: Overrides the rotation of the Spawn Point.")]
		public FsmVector3 rotation;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the created object.")]
		public FsmGameObject storeObject;

		[Tooltip("Use Network.Instantiate to create a Game Object on all clients in a networked game.")]
		public FsmBool networkInstantiate;

		[Tooltip("Usually 0. The group number allows you to group together network messages which allows you to filter them if so desired.")]
		public FsmInt networkGroup;

		public override void Reset()
		{
			gameObject = null;
			spawnPoint = null;
			position = new FsmVector3 { UseVariable = true };
			rotation = new FsmVector3 { UseVariable = true };
			storeObject = null;
			networkInstantiate = false;
			networkGroup = 0;
		}

		public override void OnEnter()
		{
			var go = gameObject.Value;
			
			if (go != null)
			{
				var spawnPosition = Vector3.zero;
				var spawnRotation = Vector3.zero;
				
				if (spawnPoint.Value != null)
				{
					spawnPosition = spawnPoint.Value.transform.position;
					
					if (!position.IsNone)
					{
						spawnPosition += position.Value;
					}
					
					spawnRotation = !rotation.IsNone ? rotation.Value : spawnPoint.Value.transform.eulerAngles;
				}
				else
				{
					if (!position.IsNone)
					{
						spawnPosition = position.Value;
					}
					
					if (!rotation.IsNone)
					{
						spawnRotation = rotation.Value;
					}
                }

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)
                GameObject newObject;

				if (!networkInstantiate.Value)
				{
					newObject = (GameObject)Object.Instantiate(go, spawnPosition, Quaternion.Euler(spawnRotation));
				}
				else
				{
					newObject = (GameObject)Network.Instantiate(go, spawnPosition, Quaternion.Euler(spawnRotation), networkGroup.Value);
				}
#else
                var newObject = (GameObject)Object.Instantiate(go, spawnPosition, Quaternion.Euler(spawnRotation));
#endif
                storeObject.Value = newObject;
				
				//newObject.transform.position = spawnPosition;
				//newObject.transform.eulerAngles = spawnRotation;
			}
			
			Finish();
		}

	}
}