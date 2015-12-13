// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayMakerGUI))]
class PlayMakerGUIInspector : Editor
{
	private PlayMakerGUI guiComponent;

	void OnEnable()
	{
		guiComponent = (PlayMakerGUI) target;

		guiComponent.drawStateLabels = EditorPrefs.GetBool(EditorPrefStrings.ShowStateLabelsInGameView);

		CheckForDuplicateComponents();
	}

	public override void OnInspectorGUI()
	{
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2
        EditorGUIUtility.LookLikeInspector();
#else
        EditorGUIUtility.labelWidth = 210;
#endif
        GUILayout.Label(Strings.Label_NOTES, EditorStyles.boldLabel);
		GUILayout.Label(Strings.Hint_PlayMakerGUI_Notes);
		GUILayout.Label(Strings.Label_General, EditorStyles.boldLabel);

		EditorGUI.indentLevel = 1;

		guiComponent.enableGUILayout = EditorGUILayout.Toggle(new GUIContent(Strings.Label_Enable_GUILayout,
		                                                               Strings.Tooltip_Enable_GUILayout),
																	   guiComponent.enableGUILayout);
		guiComponent.controlMouseCursor = EditorGUILayout.Toggle(new GUIContent(Strings.Label_Control_Mouse_Cursor,
		                                                                  Strings.Tooltip_Control_Mouse_Cursor),
																		  guiComponent.controlMouseCursor);

		guiComponent.previewOnGUI = EditorGUILayout.Toggle(new GUIContent(Strings.Label_Preview_GUI_Actions_While_Editing, Strings.Tooltip_Preview_GUI_Actions_While_Editing), guiComponent.previewOnGUI);

		EditorGUI.indentLevel = 0;
		GUILayout.Label(Strings.Label_Debugging, EditorStyles.boldLabel);
		EditorGUI.indentLevel = 1;

		var drawStateLabels = EditorGUILayout.Toggle(new GUIContent(Strings.Label_Draw_Active_State_Labels, Strings.Tooltip_Draw_Active_State_Labels), guiComponent.drawStateLabels);

		if (drawStateLabels != guiComponent.drawStateLabels)
		{
			guiComponent.drawStateLabels = drawStateLabels;
			EditorPrefs.SetBool(EditorPrefStrings.ShowStateLabelsInGameView, drawStateLabels);
		}

		GUI.enabled = guiComponent.drawStateLabels;
		//EditorGUI.indentLevel = 2;

		guiComponent.GUITextureStateLabels = EditorGUILayout.Toggle(new GUIContent(Strings.Label_GUITexture_State_Labels, Strings.Tooltip_GUITexture_State_Labels), guiComponent.GUITextureStateLabels);
		guiComponent.GUITextStateLabels = EditorGUILayout.Toggle(new GUIContent(Strings.Label_GUIText_State_Labels, Strings.Tooltip_GUIText_State_Labels), guiComponent.GUITextStateLabels);

		GUI.enabled = true;
		//EditorGUI.indentLevel = 1;

		guiComponent.filterLabelsWithDistance = EditorGUILayout.Toggle(new GUIContent(Strings.Label_Filter_State_Labels_With_Distance, Strings.Tooltip_Filter_State_Labels_With_Distance), guiComponent.filterLabelsWithDistance);

		GUI.enabled = guiComponent.filterLabelsWithDistance;

		guiComponent.maxLabelDistance = EditorGUILayout.FloatField(new GUIContent(Strings.Label_Camera_Distance, Strings.Tooltip_Camera_Distance), guiComponent.maxLabelDistance);

		if (GUI.changed)
		{
			CheckForDuplicateComponents();
		}
	}

	void CheckForDuplicateComponents()
	{
		var components = FindObjectsOfType(typeof(PlayMakerGUI));

		if (components.Length > 1)
		{
			if (EditorUtility.DisplayDialog(Strings.ProductName, Strings.Error_Multiple_PlayMakerGUI_components, Strings.Yes, Strings.No))
			{
				foreach (Object component in components)
				{
					if (component != target)
					{
						var behavior = (PlayMakerGUI)component as Behaviour;
						
						// Delete the game object if it only has the PlayMakerGUI component?

						if (behavior.gameObject.GetComponents(typeof(Component)).Length == 2) // every game object has a transform component
						{
                            if (EditorUtility.DisplayDialog(Strings.ProductName, string.Format(Strings.Dialog_Delete_Extra_PlayMakerGUI_GameObject, behavior.gameObject.name), Strings.Yes, Strings.No))
							{
								DestroyImmediate(behavior.gameObject);
							}
						}
						else
						{
							DestroyImmediate(component);
						}
					}
				}
			}
		}
	}

}
