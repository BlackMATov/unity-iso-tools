// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Controls the appearance of Mouse Cursor.")]
	public class SetMouseCursor : FsmStateAction
	{
		public FsmTexture cursorTexture;
		public FsmBool hideCursor;
		public FsmBool lockCursor;
		
		public override void Reset()
		{
			cursorTexture = null;
			hideCursor = false;
			lockCursor = false;
		}
		
		public override void OnEnter()
		{
			PlayMakerGUI.LockCursor = lockCursor.Value;
			PlayMakerGUI.HideCursor = hideCursor.Value;
			PlayMakerGUI.MouseCursor = cursorTexture.Value;
			
			Finish();
		}
		
/*		
		public override void OnUpdate()
		{
			// not sure if there is a performance impact to setting these ever frame,
			// so only do it if it's changed.
			
			if (Screen.lockCursor != lockCursor.Value)
				Screen.lockCursor = lockCursor.Value;
			
			if (Screen.showCursor == hideCursor.Value)
				Screen.showCursor = !hideCursor.Value;
		}
		
		public override void OnGUI()
		{
			// draw custom cursor
			
			if (cursorTexture != null)
			{
				var mousePos = Input.mousePosition;
				var pos =  new Rect(mousePos.x - cursorTexture.width * 0.5f, 
					Screen.height - mousePos.y - cursorTexture.height * 0.5f, 
					cursorTexture.width, cursorTexture.height);
				
				GUI.DrawTexture(pos, cursorTexture);
			}
		}*/
	}
}
