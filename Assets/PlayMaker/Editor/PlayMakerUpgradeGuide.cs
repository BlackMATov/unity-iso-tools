using UnityEngine;
using UnityEditor;

/* Moved to WelcomeWindow.cs

namespace HutongGames.PlayMakerEditor
{
    /// <summary>
    /// Shows critical upgrade info for each version
    /// </summary>
    public class PlayMakerUpgradeGuide : EditorWindow
    {
        private const string urlReleaseNotes = "https://hutonggames.fogbugz.com/default.asp?W311";
        private const string urlTroubleshooting = "https://hutonggames.fogbugz.com/default.asp?W624";

        private bool showOnLoad;
        private Vector2 scrollPosition;

        public void OnEnable()
        {
            title = "PlayMaker";
            position = new Rect(100,100,350,400);
            minSize = new Vector2(350,200);

            showOnLoad = EditorPrefs.GetBool(EditorPrefStrings.ShowUpgradeGuide, true);
        }

        public void OnGUI()
        {
            FsmEditorStyles.Init();

            FsmEditorGUILayout.ToolWindowLargeTitle(this, "Upgrade Guide");

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.HelpBox("Always BACKUP projects before updating!\nUse Version Control to manage changes!", MessageType.Error);

            GUILayout.Label("Version 1.8.0", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("FSMs saved with 1.8.0 cannot be opened in earlier versions of PlayMaker! Please BACKUP projects!", MessageType.Warning);

            GUILayout.Label("Unity 5 Upgrade Notes", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("If you run into problems updating a Unity 4.x project please check the Troubleshooting guide on the PlayMaker Wiki.", MessageType.Warning);
            EditorGUILayout.HelpBox("Unity 5 removed component property shortcuts from GameObject. " +
                                    "\n\nThe Unity auto update process replaces these properties with GetComponent calls. " +
                                    "In many cases this is fine, but some third party actions and addons might need manual updating! " +
                                    "Please post on the PlayMaker forums and contact the original authors for help." +
                                    "\n\nIf you used these GameObject properties in Get Property or Set Property actions " +
                                    "they are no longer valid, and you need to instead point to the Component directly. " +
                                    "E.g., Drag the Component (NOT the GameObject) into the Target Object field." +
                                    "\n", MessageType.Warning);

            GUILayout.Label("Unity 4.6 Upgrade Notes", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Find support for the new Unity GUI online in our Addons page.", MessageType.Info);
            EditorGUILayout.HelpBox("PlayMakerGUI is only needed if you use OnGUI Actions. " +
                                    "If you don't use OnGUI actions un-check Auto-Add PlayMakerGUI in PlayMaker Preferences.", MessageType.Info);
 
            GUILayout.EndScrollView();

            GUILayout.FlexibleSpace();
            FsmEditorGUILayout.Divider();

            EditorGUI.BeginChangeCheck();
            var dontShowAgain = GUILayout.Toggle(!showOnLoad, "Don't Show Again Until Next Update");
            if (EditorGUI.EndChangeCheck())
            {
                showOnLoad = !dontShowAgain;
                EditorPrefs.SetBool(EditorPrefStrings.ShowUpgradeGuide, showOnLoad);
            }

            if (GUILayout.Button("Online Release Notes"))
            {
                Application.OpenURL(urlReleaseNotes);
            }
        }
    }
}
*/