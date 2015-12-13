// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEditor;
using UnityEngine;
using HutongGames.PlayMakerEditor;

[CustomEditor(typeof(PlayMakerGlobals))]
class PlayMakerGlobalsInspector : Editor
{
	private PlayMakerGlobals globals;
	private bool refresh;

	private List<FsmVariable> variableList;

	void OnEnable()
	{
		//Debug.Log("PlayMakerGlobalsInspector: OnEnable");

		globals = target as PlayMakerGlobals;

		BuildVariableList();
	}

	public override void OnInspectorGUI()
	{
        EditorGUILayout.HelpBox(Strings.Hint_GlobalsInspector_Shows_DEFAULT_Values, MessageType.Info);
	
		if (refresh)
		{
			Refresh();
			return;
		}

		GUILayout.Label(Strings.Command_Global_Variables, EditorStyles.boldLabel);

		if (variableList.Count > 0)
		{

            var currentCategory = 0;
            for (var index = 0; index < variableList.Count; index++)
            {
                var fsmVariable = variableList[index];
                var categoryID = fsmVariable.CategoryID;
                if (categoryID > 0 && categoryID != currentCategory)
                {
                    currentCategory = categoryID;
                    GUILayout.Label(globals.Variables.Categories[currentCategory], EditorStyles.boldLabel);
                    //FsmEditorGUILayout.LightDivider();
                }

				var tooltip = fsmVariable.Name;

				if (!string.IsNullOrEmpty(fsmVariable.Tooltip))
				{
					tooltip += "\n" + fsmVariable.Tooltip;
				}

                if (fsmVariable.Type == VariableType.Array)
                {
                    GUILayout.Label(fsmVariable.Name);
                }
				fsmVariable.DoEditorGUI(new GUIContent(fsmVariable.Name, tooltip), true);
			}
		}
		else
		{
			GUILayout.Label(Strings.Label_None_In_Table);
		}

		GUILayout.Label(Strings.Label_Global_Events, EditorStyles.boldLabel);

		if (globals.Events.Count > 0)
		{
			foreach (var eventName in globals.Events)
			{
				GUILayout.Label(eventName);
			}
		}
		else
		{
			GUILayout.Label(Strings.Label_None_In_Table);
		}

        GUILayout.Space(5);

	    if (GUILayout.Button("Refresh"))
	    {
	        Refresh();
	    }

        GUILayout.Space(10);

        //FsmEditorGUILayout.Divider();

        if (GUILayout.Button(Strings.Command_Export_Globals))
        {
            FsmEditorUtility.ExportGlobals();
        }

        if (GUILayout.Button(Strings.Command_Import_Globals))
        {
            FsmEditorUtility.ImportGlobals();
        }
        EditorGUILayout.HelpBox(Strings.Hint_Export_Globals_Notes, MessageType.None);
	}

	void Refresh()
	{
		refresh = false;
		BuildVariableList();
		Repaint();
	}

	void BuildVariableList()
	{
		variableList = FsmVariable.GetFsmVariableList(globals);
	}
}
