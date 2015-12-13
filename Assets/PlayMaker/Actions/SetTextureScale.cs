// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets the Scale of a named texture in a Game Object's Material. Useful for special effects.")]
	public class SetTextureScale : ComponentAction<Renderer>
	{
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;
		public FsmInt materialIndex;
		[UIHint(UIHint.NamedColor)]
		public FsmString namedTexture;
		[RequiredField]
		public FsmFloat scaleX;
		[RequiredField]
		public FsmFloat scaleY;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			namedTexture = "_MainTex";
			scaleX = 1;
			scaleY = 1;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetTextureScale();

		    if (!everyFrame)
		    {
		        Finish();
		    }
		}
		
		public override void OnUpdate()
		{
			DoSetTextureScale();
		}

		void DoSetTextureScale()
		{
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
				renderer.material.SetTextureScale(namedTexture.Value, new Vector2(scaleX.Value, scaleY.Value));
			}
			else if (renderer.materials.Length > materialIndex.Value)
			{
				var materials = renderer.materials;
				materials[materialIndex.Value].SetTextureScale(namedTexture.Value, new Vector2(scaleX.Value, scaleY.Value));
				renderer.materials = materials;
			}			
		}
	}
}