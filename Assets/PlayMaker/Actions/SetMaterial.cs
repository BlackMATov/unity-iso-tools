// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets the material on a game object.")]
	public class SetMaterial : ComponentAction<Renderer>
	{
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;
		public FsmInt materialIndex;
		[RequiredField]
		public FsmMaterial material;

		public override void Reset()
		{
			gameObject = null;
			material = null;
			materialIndex = 0;
		}

		public override void OnEnter()
		{
			DoSetMaterial();
			
			Finish();
		}

		void DoSetMaterial()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (!UpdateCache(go))
		    {
		        return;
		    }

			if (materialIndex.Value == 0)
			{
				renderer.material = material.Value;
			}
			else if (renderer.materials.Length > materialIndex.Value)
			{
				var materials = renderer.materials;
				materials[materialIndex.Value] = material.Value;
				renderer.materials = materials;
			}
		}
	}
}