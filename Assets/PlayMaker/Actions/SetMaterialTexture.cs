// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets a named texture in a game object's material.")]
	public class SetMaterialTexture : ComponentAction<Renderer>
	{
		[Tooltip("The GameObject that the material is applied to.")]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		[Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
		public FsmInt materialIndex;

		[Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
		public FsmMaterial material;

		[UIHint(UIHint.NamedTexture)]
		[Tooltip("A named parameter in the shader.")]
		public FsmString namedTexture;
		
		public FsmTexture texture;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			material = null;
			namedTexture = "_MainTex";
			texture = null;
		}

		public override void OnEnter()
		{
			DoSetMaterialTexture();
			Finish();
		}
		
		void DoSetMaterialTexture()
		{
			var namedTex = namedTexture.Value;
			if (namedTex == "") namedTex = "_MainTex";

			if (material.Value != null)
			{
				material.Value.SetTexture(namedTex, texture.Value);
				return;
			}

			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (!UpdateCache(go))
		    {
		        return;
		    }
			
			if (renderer.material == null)
			{
				LogError("Missing Material!");
				return;
			}
			
			if (materialIndex.Value == 0)
			{
				renderer.material.SetTexture(namedTex, texture.Value);
			}
			else if (renderer.materials.Length > materialIndex.Value)
			{
				var materials = renderer.materials;
				materials[materialIndex.Value].SetTexture(namedTex, texture.Value);
				renderer.materials = materials;
			}
		}
	}
}