// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

#if !UNITY_FLASH

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Scales the GUI around a pivot point. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
	public class ScaleGUI : FsmStateAction
	{
		[RequiredField]
		public FsmFloat scaleX;
		
		[RequiredField]
		public FsmFloat scaleY;
		
		[RequiredField]
		public FsmFloat pivotX;
		
		[RequiredField]
		public FsmFloat pivotY;
		
		[Tooltip("Pivot point uses normalized coordinates. E.g. 0.5 is the center of the screen.")]
		public bool normalized;
		
		public bool applyGlobally;

		bool applied;
		
		public override void Reset()
		{
			scaleX = 1f;
			scaleY = 1f;
			pivotX = 0.5f;
			pivotY = 0.5f;
			normalized = true;
			applyGlobally = false;
		}

		public override void OnGUI()
		{
			if (applied)
			{
				return;
			}
			
			var scale = new Vector2(scaleX.Value, scaleY.Value);

			// Not allowed to scale to 0 - it breaks the GUI matrix

			if (Equals(scale.x, 0)) scale.x = 0.0001f;
			if (Equals(scale.y, 0)) scale.x = 0.0001f;

			var pivotPoint = new Vector2(pivotX.Value, pivotY.Value);
			
			if (normalized)
			{
				pivotPoint.x *= Screen.width;
				pivotPoint.y *= Screen.height;
			}
			
			GUIUtility.ScaleAroundPivot(scale, pivotPoint);
			
			if (applyGlobally)
			{
				PlayMakerGUI.GUIMatrix = GUI.matrix;
				applied = true;
			}
		}
		
		public override void OnUpdate()
		{
			applied = false;
		}
	}
}

#endif