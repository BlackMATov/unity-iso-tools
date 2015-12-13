// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;
using System.Collections.Generic;
using HutongGames.Editor;

/// <summary>
/// Custom inspector for PlayMakerFSM
/// </summary>
[CustomEditor(typeof(PlayMakerFSM))]
public class FsmComponentInspector : Editor
{
    private bool initialized;

    // Inspector targets

    private PlayMakerFSM fsmComponent;   // Inspector target
    private FsmTemplate fsmTemplate;     // template used by fsmComponent

    // Inspector foldout states

    private bool showSettings = true;
    private bool showControls = true;
    private bool showDebug = false;
    //private bool showInfo;
    //private bool showStates;
    //private bool showEvents;
    //private bool showVariables;

    // Collect easily editable references to fsmComponent.Fsm.Variables

    List<FsmVariable> fsmVariables = new List<FsmVariable>();

    public void OnEnable()
    {
        //Debug.Log("FsmComponentInspector: OnEnable");

        fsmComponent = target as PlayMakerFSM;
    }

    // Can't do this in OnEnable because it interferes with FSM init
    // when playing in the editor
    void Initialize()
    {
        fsmTemplate = fsmComponent.FsmTemplate;
        RefreshTemplate();
        BuildFsmVariableList();
        initialized = true;
    }

    [Localizable(false)]
    public override void OnInspectorGUI()
    {
        if (fsmComponent == null) return; // shouldn't happen

        if (!initialized)
        {
            Initialize();
        }

        if (!fsmComponent.Fsm.Initialized)
        {
            // Note: cannot do this in OnEnable since is breaks FSMs in editor
            // Safe to do now since FSM would already have initialized itself
            fsmComponent.Fsm.Init(fsmComponent);
        }

        FsmEditorStyles.Init();

        var fsm = fsmComponent.Fsm;  // grab Fsm for convenience

        if (fsm.States.Length > 50) // a little arbitrary, but better than nothing!
        {
            EditorGUILayout.HelpBox("NOTE: Collapse this inspector for better editor performance with large FSMs.", MessageType.None);
        }

        // Edit FSM name

        EditorGUILayout.BeginHorizontal();
        showSettings = true; //GUILayout.Toggle(showSettings, "x"); // TODO
        fsm.Name = EditorGUILayout.TextField(fsm.Name);
        if (GUILayout.Button(FsmEditorContent.EditFsmButton, GUILayout.MaxWidth(45)))
        {
            OpenInEditor(fsmComponent);
            GUIUtility.ExitGUI();
        }
        EditorGUILayout.EndHorizontal();

        if (showSettings)
        {
            // Edit FSM Template

            EditorGUILayout.BeginHorizontal();
            var template = (FsmTemplate)EditorGUILayout.ObjectField(FsmEditorContent.UseTemplateLabel, fsmComponent.FsmTemplate, typeof(FsmTemplate), false);
            if (template != fsmComponent.FsmTemplate)
            {
                SelectTemplate(template);
            }
            if (GUILayout.Button(FsmEditorContent.BrowseTemplateButton, GUILayout.MaxWidth(45)))
            {
                DoSelectTemplateMenu();
            }
            EditorGUILayout.EndHorizontal();

            // Disable GUI that can't be edited if referencing a template

            EditorGUI.BeginDisabledGroup(!Application.isPlaying && fsmComponent.FsmTemplate != null);

            if (fsmComponent.FsmTemplate != null)
            {
                // next few fields should show template values
                template = fsmComponent.FsmTemplate;
                fsm = template.fsm;
            }

            // Edit Description

            fsm.Description = FsmEditorGUILayout.TextAreaWithHint(fsm.Description, Strings.Label_Description___, GUILayout.MinHeight(60));

            // Edit Help Url (lets the user link to documentation for the FSM)

            EditorGUILayout.BeginHorizontal();
            fsm.DocUrl = FsmEditorGUILayout.TextFieldWithHint(fsm.DocUrl, Strings.Tooltip_Documentation_Url);
            EditorGUI.BeginDisabledGroup(!string.IsNullOrEmpty(fsm.DocUrl));
            if (FsmEditorGUILayout.HelpButton())
            {
                Application.OpenURL(fsm.DocUrl);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            // Settings

            EditorGUI.BeginDisabledGroup(!Application.isPlaying && FsmEditor.SelectedFsmUsesTemplate);

            fsm.MaxLoopCountOverride = EditorGUILayout.IntField(FsmEditorContent.MaxLoopOverrideLabel, fsm.MaxLoopCountOverride);
            fsm.RestartOnEnable = GUILayout.Toggle(fsm.RestartOnEnable, FsmEditorContent.ResetOnDisableLabel);

            EditorGUI.EndDisabledGroup(); // Settings
            EditorGUI.EndDisabledGroup(); // Uses template

            // stop showing template values
            fsm = fsmComponent.Fsm;
        }

        // Controls Section

        // Show FSM variables with Inspector option checked

        FsmEditorGUILayout.LightDivider();
        showControls = EditorGUILayout.Foldout(showControls, FsmEditorContent.FsmControlsLabel, FsmEditorStyles.CategoryFoldout);
        if (showControls)
        {
            BuildFsmVariableList();

            var currentCategory = 0;
            for (var index = 0; index < fsmVariables.Count; index++)
            {
                var fsmVariable = fsmVariables[index];
                if (fsmVariable.ShowInInspector)
                {
                    var categoryID = fsmVariable.CategoryID;
                    if (categoryID > 0 && categoryID != currentCategory)
                    {
                        currentCategory = categoryID;
                        GUILayout.Label(fsmComponent.Fsm.Variables.Categories[categoryID], EditorStyles.boldLabel);
                        FsmEditorGUILayout.LightDivider();
                    }
                    fsmVariable.DoInspectorGUI(FsmEditorContent.TempContent(fsmVariable.Name, fsmVariable.Name + (!string.IsNullOrEmpty(fsmVariable.Tooltip) ? ":\n" + fsmVariable.Tooltip : "")));
                }
            }
        }

        // Show events with Inspector option checked
        // These become buttons that the user can press to send the events

        if (showControls)
        {
            foreach (var fsmEvent in fsm.ExposedEvents)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(fsmEvent.Name);
                if (GUILayout.Button(fsmEvent.Name))
                {
                    fsm.Event(fsmEvent);
                    FsmEditor.RepaintAll();
                }
                GUILayout.EndHorizontal();
            }
        }

        // DEBUG 

        FsmEditorGUILayout.LightDivider();
        showDebug = EditorGUILayout.Foldout(showDebug, "Debug", FsmEditorStyles.CategoryFoldout);
        if (showDebug)
        {
            fsm.ShowStateLabel = GUILayout.Toggle(fsm.ShowStateLabel, FsmEditorContent.ShowStateLabelsLabel);
            fsm.EnableBreakpoints = GUILayout.Toggle(fsm.EnableBreakpoints, FsmEditorContent.EnableBreakpointsLabel);
            fsm.EnableDebugFlow = GUILayout.Toggle(fsm.EnableDebugFlow, FsmEditorContent.EnableDebugFlowLabel);
        }

        #region INFO
        // Show general info about the FSM

        /* Is this useful...?
        EditorGUI.indentLevel = 0;
        FsmEditorGUILayout.LightDivider();
        showInfo = EditorGUILayout.Foldout(showInfo, Strings.Label_Info, FsmEditorStyles.CategoryFoldout);
        if (showInfo)
        {
            EditorGUI.indentLevel = 1;

            showStates = EditorGUILayout.Foldout(showStates, string.Format(Strings.Label_States_Count, fsm.States.Length));
            if (showStates)
            {
                string states = "";

                if (fsm.States.Length > 0)
                {
                    foreach (var state in fsm.States)
                    {
                        states += FsmEditorStyles.tab2 + state.Name + FsmEditorStyles.newline;
                    }
                    states = states.Substring(0, states.Length - 1);
                }
                else
                {
                    states = FsmEditorStyles.tab2 + Strings.Label_None_In_Table;
                }
                GUILayout.Label(states);
            }

            showEvents = EditorGUILayout.Foldout(showEvents, string.Format(Strings.Label_Events_Count, fsm.Events.Length));
            if (showEvents)
            {
                var events = "";

                if (fsm.Events.Length > 0)
                {
                    foreach (var fsmEvent in fsm.Events)
                    {
                        events += FsmEditorStyles.tab2 + fsmEvent.Name + FsmEditorStyles.newline;
                    }
                    events = events.Substring(0, events.Length - 1);
                }
                else
                {
                    events = FsmEditorStyles.tab2 + Strings.Label_None_In_Table;
                }
                GUILayout.Label(events);
            }

            showVariables = EditorGUILayout.Foldout(showVariables, string.Format(Strings.Label_Variables_Count, fsmVariables.Count));
            if (showVariables)
            {
                var variables = "";

                if (fsmVariables.Count > 0)
                {
                    foreach (var fsmVar in fsmVariables)
                    {
                        variables += FsmEditorStyles.tab2 + fsmVar.Name + FsmEditorStyles.newline;
                    }
                    variables = variables.Substring(0, variables.Length - 1);
                }
                else
                {
                    variables = FsmEditorStyles.tab2 + Strings.Label_None_In_Table;
                }
                GUILayout.Label(variables);
            }
        }*/
        #endregion


        // Manual refresh if template has been edited

        if (fsmTemplate != null)
        {
            if (GUILayout.Button(FsmEditorContent.RefreshTemplateLabel))
            {
                RefreshTemplate();
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(fsmComponent);
            FsmEditor.RepaintAll();
        }
    }

    /// <summary>
    /// Open the specified FSM in the Playmaker Editor
    /// </summary>
    public static void OpenInEditor(PlayMakerFSM fsmComponent)
    {
        if (FsmEditor.Instance == null)
        {
            FsmEditorWindow.OpenWindow(fsmComponent);
        }
        else
        {
            EditorWindow.FocusWindowIfItsOpen<FsmEditorWindow>();
            FsmEditor.SelectFsm(fsmComponent.FsmTemplate == null ? fsmComponent.Fsm : fsmComponent.FsmTemplate.fsm);
        }
    }

    /// <summary>
    /// Open the specified FSM in the Playmaker Editor
    /// </summary>
    public static void OpenInEditor(Fsm fsm)
    {
        if (fsm.Owner != null)
        {
            OpenInEditor(fsm.Owner as PlayMakerFSM);
        }
    }

    /// <summary>
    /// Open the first PlayMakerFSM on a GameObject in the Playmaker Editor
    /// </summary>
    public static void OpenInEditor(GameObject go)
    {
        if (go != null)
        {
            OpenInEditor(FsmEditorUtility.FindFsmOnGameObject(go));
        }
    }

    /// <summary>
    /// The fsmVariables list contains easily editable references to FSM variables
    /// (Similar in concept to SerializedProperty)
    /// </summary>
    void BuildFsmVariableList()
    {
        fsmVariables = FsmVariable.GetFsmVariableList(target);
        fsmVariables = fsmVariables.Where(x => x.ShowInInspector).ToList();
    }

    #region Templates

    void SelectTemplate(object userdata)
    {
        SelectTemplate(userdata as FsmTemplate);
    }

    void SelectTemplate(FsmTemplate template)
    {
        if (template == fsmComponent.FsmTemplate)
        {
            return; // don't want to lose overridden variables
        }
        
        UndoUtility.RegisterUndo(fsmComponent, "PlayMaker : Set FSM Template");
        fsmComponent.SetFsmTemplate(template);
        fsmTemplate = template;
        BuildFsmVariableList();
        EditorUtility.SetDirty(fsmComponent);

        FsmEditor.RefreshInspector(); // Keep Playmaker Editor in sync
    }

    void ClearTemplate()
    {
        fsmComponent.Reset();
        fsmTemplate = null;

        BuildFsmVariableList();

        // If we were editing the template in the Playmaker editor
        // handle this gracefully by reselecting the base FSM

        if (FsmEditor.SelectedFsmComponent == fsmComponent)
        {
            FsmEditor.SelectFsm(fsmComponent.Fsm);
        }
    }

    /// <summary>
    /// A template can change since it was selected.
    /// This method refreshes the UI to reflect any changes
    /// while keeping any variable overrides that the use has made
    /// </summary>
    void RefreshTemplate()
    {
        if (fsmTemplate == null || Application.isPlaying)
        {
            return;
        }

        // we want to keep the existing overrides
        // so we copy the current FsmVariables

        var currentValues = new FsmVariables(fsmComponent.Fsm.Variables);

        // then we update the template

        fsmComponent.SetFsmTemplate(fsmTemplate);

        // finally we apply the original overrides back to the new FsmVariables

        fsmComponent.Fsm.Variables.OverrideVariableValues(currentValues);

        // and refresh the UI

        BuildFsmVariableList();

        FsmEditor.RefreshInspector();
    }

    void DoSelectTemplateMenu()
    {
        var menu = new GenericMenu();

        var templates = (FsmTemplate[])Resources.FindObjectsOfTypeAll(typeof(FsmTemplate));

        menu.AddItem(new GUIContent(Strings.Menu_None), false, ClearTemplate);

        foreach (var template in templates)
        {
            const string submenu = "/";
            menu.AddItem(new GUIContent(template.Category + submenu + template.name), fsmComponent.FsmTemplate == template, SelectTemplate, template);
        }

        menu.ShowAsContext();
    }

    #endregion

    /// <summary>
    /// Actions can use OnSceneGUI to display interactive gizmos
    /// </summary>
    public void OnSceneGUI()
    {
        FsmEditor.OnSceneGUI();
    }
}


