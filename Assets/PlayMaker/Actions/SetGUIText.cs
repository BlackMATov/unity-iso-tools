// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUIElement)]
	[Tooltip("Sets the Text used by the GUIText Component attached to a Game Object.")]
	public class SetGUIText : ComponentAction<GUIText>
	{
		[RequiredField]
		[CheckForComponent(typeof(GUIText))]
		public FsmOwnerDefault gameObject;
		public FsmString text;
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			text = "";
		}

		public override void OnEnter()
		{
			DoSetGUIText();

		    if (!everyFrame)
		    {
		        Finish();
		    }
		}
		
		public override void OnUpdate()
		{
			DoSetGUIText();
		}
		
		void DoSetGUIText()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (UpdateCache(go))
		    {
		        guiText.text = text.Value;
		    }
		}
	}
}