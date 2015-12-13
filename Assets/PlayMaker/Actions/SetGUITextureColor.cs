// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUIElement)]
	[Tooltip("Sets the Color of the GUITexture attached to a Game Object.")]
	public class SetGUITextureColor : ComponentAction<GUITexture>
	{
		[RequiredField]
		[CheckForComponent(typeof(GUITexture))]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmColor color;
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			color = Color.white;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetGUITextureColor();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetGUITextureColor();
		}

		void DoSetGUITextureColor()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(go))
			{
				guiTexture.color = color.Value;
			}
		}
	}
}