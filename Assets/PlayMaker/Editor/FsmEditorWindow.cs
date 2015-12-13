// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEditor;
using UnityEngine;

/* NOTE: Wrapper no longer needed in Unity 4.x
 * BUT changing it breaks saved layouts
 * SO wrap in namespace instead (which is also now supported in 4.x)
 */

// EditorWindow classes can't be called from a dll 
// so create a thin wrapper class as a workaround

namespace HutongGames.PlayMakerEditor
{
    [System.Serializable]
    class FsmEditorWindow : BaseEditorWindow
    {
        /// <summary>
        /// Open the Fsm Editor and optionally show the Welcome Screen
        /// </summary>
        public static void OpenWindow()
        {
            GetWindow<FsmEditorWindow>();

            if (!EditorApp.IsSourceCodeVersion)
            {
                if (EditorPrefs.GetBool(EditorPrefStrings.ShowWelcomeScreen, true))
                {
                    GetWindow<PlayMakerWelcomeWindow>(true);
                }

                /* Moved to WelcomeWindow.cs
                if (EditorPrefs.GetBool(EditorPrefStrings.ShowUpgradeGuide, true))
                {
                    GetWindow<PlayMakerUpgradeGuide>(true);
                }*/
            }
        }

        /// <summary>
        /// Open the Fsm Editor and select an Fsm Component
        /// </summary>
        public static void OpenWindow(PlayMakerFSM fsmComponent)
        {
            OpenWindow();

            FsmEditor.SelectFsm(fsmComponent.Fsm);
        }

        /// <summary>
        /// Open the Fsm Editor and select an Fsm Component
        /// </summary>
        public static void OpenWindow(FsmTemplate fsmTemplate)
        {
            OpenWindow();

            FsmEditor.SelectFsm(fsmTemplate.fsm);
        }

        /// <summary>
        /// Is the Fsm Editor open?
        /// </summary>
        public static bool IsOpen()
        {
            return instance != null;
        }

        private static FsmEditorWindow instance;

        [SerializeField]
        private FsmEditor fsmEditor;

        // tool windows (can't open them inside dll)

	[SerializeField] private FsmSelectorWindow fsmSelectorWindow;    
    [SerializeField] private FsmTemplateWindow fsmTemplateWindow;
    [SerializeField] private FsmStateWindow stateSelectorWindow;
    [SerializeField] private FsmActionWindow actionWindow;
    [SerializeField] private FsmErrorWindow errorWindow;
    [SerializeField] private TimelineWindow timelineWindow;
    [SerializeField] private FsmLogWindow logWindow;
    [SerializeField] private ContextToolWindow toolWindow;
    [SerializeField] private GlobalEventsWindow globalEventsWindow;
    [SerializeField] private GlobalVariablesWindow globalVariablesWindow;
    [SerializeField] private ReportWindow reportWindow;
    [SerializeField] private AboutWindow aboutWindow;

        // ReSharper disable UnusedMember.Local

        /// <summary>
        /// Delay initialization until first OnGUI to avoid interfering with runtime system intialization.
        /// </summary>
        public override void Initialize()
        {
            instance = this;

            if (fsmEditor == null)
            {
                fsmEditor = new FsmEditor();
            }

            fsmEditor.InitWindow(this);
            fsmEditor.OnEnable();
        }

        public override void DoGUI()
        {
            fsmEditor.OnGUI();

            /* Debug Repaint events
            if (Event.current.type == EventType.repaint)
            {
                Debug.Log("Repaint");
            }*/

            if (Event.current.type == EventType.ValidateCommand)
            {
                switch (Event.current.commandName)
                {
                    case "UndoRedoPerformed":
                    case "Cut":
                    case "Copy":
                    case "Paste":
                    case "SelectAll":
                        Event.current.Use();
                        break;
                }
            }

            if (Event.current.type == EventType.ExecuteCommand)
            {
                switch (Event.current.commandName)
                {
                    /* replaced with Undo.undoRedoPerformed callback added in Unity 4.3
                    case "UndoRedoPerformed":
                        FsmEditor.UndoRedoPerformed();
                        break;
                    */

                    case "Cut":
                        FsmEditor.Cut();
                        break;

                    case "Copy":
                        FsmEditor.Copy();
                        break;

                    case "Paste":
                        FsmEditor.Paste();
                        break;

                    case "SelectAll":
                        FsmEditor.SelectAll();
                        break;

                    case "OpenWelcomeWindow":
                        GetWindow<PlayMakerWelcomeWindow>();
                        break;

                    case "OpenToolWindow":
                        toolWindow = GetWindow<ContextToolWindow>();
                        break;

                    case "OpenFsmSelectorWindow":
                        fsmSelectorWindow = GetWindow<FsmSelectorWindow>();
                        fsmSelectorWindow.ShowUtility();
                        break;

                    case "OpenFsmTemplateWindow":
                        fsmTemplateWindow = GetWindow<FsmTemplateWindow>();
                        break;

                    case "OpenStateSelectorWindow":
                        stateSelectorWindow = GetWindow<FsmStateWindow>();
                        break;

                    case "OpenActionWindow":
                        actionWindow = GetWindow<FsmActionWindow>();
                        break;

                    case "OpenGlobalEventsWindow":
                        globalEventsWindow = GetWindow<FsmEventsWindow>();
                        break;

                    case "OpenGlobalVariablesWindow":
                        globalVariablesWindow = GetWindow<FsmGlobalsWindow>();
                        break;

                    case "OpenErrorWindow":
                        errorWindow = GetWindow<FsmErrorWindow>();
                        break;

                    case "OpenTimelineWindow":
                        timelineWindow = GetWindow<FsmTimelineWindow>();
                        break;

                    case "OpenFsmLogWindow":
                        logWindow = GetWindow<FsmLogWindow>();
                        break;

                    case "OpenAboutWindow":
                        aboutWindow = GetWindow<AboutWindow>();
                        break;

                    case "OpenReportWindow":
                        reportWindow = GetWindow<ReportWindow>();
                        break;

                    case "AddFsmComponent":
                        PlayMakerMainMenu.AddFsmToSelected();
                        break;

                    case "RepaintAll":
                        RepaintAllWindows();
                        break;

                    case "ChangeLanguage":
                        ResetWindowTitles();
                        break;
                }

                GUIUtility.ExitGUI();
            }
        }

        // called when you change editor language
        public void ResetWindowTitles()
        {
            if (toolWindow != null)
            {
                toolWindow.InitWindowTitle();
            }

            if (fsmSelectorWindow != null)
            {
                fsmSelectorWindow.InitWindowTitle();
            }

            if (stateSelectorWindow != null)
            {
                stateSelectorWindow.InitWindowTitle();
            }

            if (actionWindow != null)
            {
                actionWindow.InitWindowTitle();
            }

            if (globalEventsWindow != null)
            {
                globalEventsWindow.InitWindowTitle();
            }

            if (globalVariablesWindow != null)
            {
                globalVariablesWindow.InitWindowTitle();
            }

            if (errorWindow != null)
            {
                errorWindow.InitWindowTitle();
            }

            if (timelineWindow != null)
            {
                timelineWindow.InitWindowTitle();
            }

            if (logWindow != null)
            {
                logWindow.InitWindowTitle();
            }

            if (reportWindow != null)
            {
                reportWindow.InitWindowTitle();
            }

            if (fsmTemplateWindow != null)
            {
                fsmTemplateWindow.InitWindowTitle();
            }
        }

        public void RepaintAllWindows()
        {
            if (toolWindow != null)
            {
                toolWindow.Repaint();
            }

            if (fsmSelectorWindow != null)
            {
                fsmSelectorWindow.Repaint();
            }

            if (stateSelectorWindow != null)
            {
                stateSelectorWindow.Repaint();
            }

            if (actionWindow != null)
            {
                actionWindow.Repaint();
            }

            if (globalEventsWindow != null)
            {
                globalEventsWindow.Repaint();
            }

            if (globalVariablesWindow != null)
            {
                globalVariablesWindow.Repaint();
            }

            if (errorWindow != null)
            {
                errorWindow.Repaint();
            }

            if (timelineWindow != null)
            {
                timelineWindow.Repaint();
            }

            if (logWindow != null)
            {
                logWindow.Repaint();
            }

            if (reportWindow != null)
            {
                reportWindow.Repaint();
            }

            if (fsmTemplateWindow != null)
            {
                fsmTemplateWindow.Repaint();
            }

            Repaint();
        }

        private void Update()
        {
            if (Initialized && fsmEditor != null)
            {
                fsmEditor.Update();
            }
        }

        private void OnInspectorUpdate()
        {
            if (Initialized && fsmEditor != null)
            {
                fsmEditor.OnInspectorUpdate();
            }
        }

        private void OnFocus()
        {
            if (Initialized && fsmEditor != null)
            {
                fsmEditor.OnFocus();
            }
        }

        private void OnSelectionChange()
        {
            if (Initialized && fsmEditor != null)
            {
                fsmEditor.OnSelectionChange();
            }
        }

        private void OnHierarchyChange()
        {
            if (Initialized && fsmEditor != null)
            {
                fsmEditor.OnHierarchyChange();
            }
        }

        private void OnProjectChange()
        {
            if (Initialized && fsmEditor != null)
            {
                fsmEditor.OnProjectChange();
            }
        }

        private void OnDisable()
        {
            if (Initialized && fsmEditor != null)
            {
                fsmEditor.OnDisable();
            }

            instance = null;
        }

        private void OnDestroy()
        {
            if (toolWindow != null)
            {
                toolWindow.SafeClose();
            }

            if (fsmSelectorWindow != null)
            {
                fsmSelectorWindow.SafeClose();
            }

            if (fsmTemplateWindow != null)
            {
                fsmTemplateWindow.SafeClose();
            }

            if (stateSelectorWindow != null)
            {
                stateSelectorWindow.SafeClose();
            }

            if (actionWindow != null)
            {
                actionWindow.SafeClose();
            }

            if (globalVariablesWindow != null)
            {
                globalVariablesWindow.SafeClose();
            }

            if (globalEventsWindow != null)
            {
                globalEventsWindow.SafeClose();
            }

            if (errorWindow != null)
            {
                errorWindow.SafeClose();
            }

            if (timelineWindow != null)
            {
                timelineWindow.SafeClose();
            }

            if (logWindow != null)
            {
                logWindow.SafeClose();
            }

            if (reportWindow != null)
            {
                reportWindow.SafeClose();
            }

            if (aboutWindow != null)
            {
                aboutWindow.SafeClose();
            }

            if (Initialized && fsmEditor != null)
            {
                fsmEditor.OnDestroy();
            }
        }

        // ReSharper restore UnusedMember.Local
    }



}
