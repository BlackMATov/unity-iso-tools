// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUIElement)]
	[Tooltip("Sets the Texture used by the GUITexture attached to a Game Object.")]
	public class SetGUITexture : ComponentAction<GUITexture>
	{
		[RequiredField]
		[CheckForComponent(typeof(GUITexture))]
		[Tooltip("The GameObject that owns the GUITexture.")]
        public FsmOwnerDefault gameObject;

        [Tooltip("Texture to apply.")]
		public FsmTexture texture;
		
		public override void Reset()
		{
			gameObject = null;
			texture = null;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(go))
			{
				guiTexture.texture = texture.Value;
			}
			
			Finish();
		}
	}
}