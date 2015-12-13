// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUIElement)]
	[Tooltip("Sets the Alpha of the GUITexture attached to a Game Object. Useful for fading GUI elements in/out.")]
	public class SetGUITextureAlpha : ComponentAction<GUITexture>
	{
		[RequiredField]
		[CheckForComponent(typeof(GUITexture))]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmFloat alpha;
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			alpha = 1.0f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGUITextureAlpha();
			
			if(!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoGUITextureAlpha();
		}
		
		void DoGUITextureAlpha()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(go))
			{
				var color = guiTexture.color;
				guiTexture.color = new Color(color.r, color.g, color.b, alpha.Value);
			}			
		}
	}
}