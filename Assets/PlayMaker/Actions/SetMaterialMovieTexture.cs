// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

#if !(UNITY_IPHONE || UNITY_ANDROID || UNITY_FLASH || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE || UNITY_BLACKBERRY || UNITY_METRO || UNITY_WP8 || UNITY_PSM || UNITY_WEBGL)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets a named texture in a game object's material to a movie texture.")]
	public class SetMaterialMovieTexture : ComponentAction<Renderer>
	{
		[Tooltip("The GameObject that the material is applied to.")]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		[Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
		public FsmInt materialIndex;

		[Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
		public FsmMaterial material;
		
		[UIHint(UIHint.NamedTexture)]
		[Tooltip("A named texture in the shader.")]
		public FsmString namedTexture;

		[RequiredField]
		[ObjectType(typeof(MovieTexture))]
		public FsmObject movieTexture;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			material = null;
			namedTexture = "_MainTex";
			movieTexture = null;
		}

		public override void OnEnter()
		{
			DoSetMaterialTexture();
			Finish();
		}

		void DoSetMaterialTexture()
		{
			var movie = movieTexture.Value as MovieTexture;

			var namedTex = namedTexture.Value;
			if (namedTex == "") namedTex = "_MainTex";

			if (material.Value != null)
			{
				material.Value.SetTexture(namedTex, movie);
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
				renderer.material.SetTexture(namedTex, movie);
			}
			else if (renderer.materials.Length > materialIndex.Value)
			{
				var materials = renderer.materials;
				materials[materialIndex.Value].SetTexture(namedTex, movie);
				renderer.materials = materials;
			}
		}
	}
}

#endif