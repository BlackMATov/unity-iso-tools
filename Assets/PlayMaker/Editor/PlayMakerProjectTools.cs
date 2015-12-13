#if (UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
#define UNITY_PRE_5_3
#endif

using System.IO;
using UnityEditor;
#if !UNITY_PRE_5_3
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

namespace HutongGames.PlayMakerEditor
{
    public class ProjectTools
    {
        // Change MenuRoot to move the Playmaker Menu
        // E.g., MenuRoot = "Plugins/PlayMaker/"
        private const string MenuRoot = "PlayMaker/";

        [MenuItem(MenuRoot + "Tools/Update All Loaded FSMs", false, 31)]
        public static void ReSaveAllLoadedFSMs()
        {
            SaveAllLoadedFSMs();
        }

        [MenuItem(MenuRoot + "Tools/Update All FSMs in Build", false, 32)]
        public static void ReSaveAllFSMsInBuild()
        {
            SaveAllFSMsInBuild();
        }

        /*WIP
        [MenuItem(MenuRoot + "Tools/Scan Scenes", false, 33)]
        public static void ScanScenesInProject()
        {
            FindAllScenes();
        }
*/

        private static void SaveAllLoadedFSMs()
        {
            Debug.Log("Checking loaded FSMs...");
            FsmEditor.RebuildFsmList();
            foreach (var fsm in FsmEditor.FsmList)
            {
                // Re-initialize loads data and forces a dirty check
                // so we can just call this and let it handle dirty etc.

                fsm.Reinitialize();
            }
        }

        private static void SaveAllFSMsInBuild()
        {
            // Allow the user to save his work!
#if UNITY_PRE_5_3
            if (!EditorApplication.SaveCurrentSceneIfUserWantsTo())
#else
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
#endif
            {
                return;
            }

            LoadPrefabsWithPlayMakerFSMComponents();

            foreach (var scene in EditorBuildSettings.scenes)
            {
                Debug.Log("Open Scene: " + scene.path);
#if UNITY_PRE_5_3
                EditorApplication.OpenScene(scene.path);
#else
                EditorSceneManager.OpenScene(scene.path);
#endif
                SaveAllLoadedFSMs();

#if UNITY_PRE_5_3
                if (!EditorApplication.SaveScene())
#else
                if (!EditorSceneManager.SaveOpenScenes())
#endif
                {
                    Debug.LogError("Could not save scene!");
                }
            }
        }

        private static void LoadPrefabsWithPlayMakerFSMComponents()
        {
            Debug.Log("Finding Prefabs with PlayMakerFSMs");

            var searchDirectory = new DirectoryInfo(Application.dataPath);
            var prefabFiles = searchDirectory.GetFiles("*.prefab", SearchOption.AllDirectories);

            foreach (var file in prefabFiles)
            {
                var filePath = file.FullName.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
                //Debug.Log(filePath + "\n" + Application.dataPath);

                var dependencies = AssetDatabase.GetDependencies(new[] { filePath });
                foreach (var dependency in dependencies)
                {
                    if (dependency.Contains("/PlayMaker.dll"))
                    {
                        Debug.Log("Found Prefab with FSM: " + filePath);
                        AssetDatabase.LoadAssetAtPath(filePath, typeof(GameObject));
                    }
                }
            }

            FsmEditor.RebuildFsmList();
        }

        /* WIP
        [Localizable(false)]
        private static void FindAllScenes()
        {
            Debug.Log("Finding all scenes...");

            var searchDirectory = new DirectoryInfo(Application.dataPath);
            var assetFiles = searchDirectory.GetFiles("*.unity", SearchOption.AllDirectories);

            foreach (var file in assetFiles)
            {
                var filePath = file.FullName.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
                var obj = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
                if (obj == null)
                {
                    //Debug.Log(filePath + ": null!");
                }
                else if (obj.GetType() == typeof(Object))
                {
                    Debug.Log(filePath);// + ": " + obj.GetType().FullName);
                }
                //var obj = AssetDatabase.
            }
        }
         */
    }
}

