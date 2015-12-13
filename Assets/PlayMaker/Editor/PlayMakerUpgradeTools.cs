using System;
using System.Collections.Generic;
using System.IO;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEditor;
using HutongGames.PlayMakerEditor;

public class PlayMakerUpgradeTools
{
    /*
    // Change MenuRoot to move the Playmaker Menu
    // E.g., MenuRoot = "Plugins/PlayMaker/"
    private const string MenuRoot = "PlayMaker/";

    [MenuItem(MenuRoot + "Tools/Re-Save All Loaded FSMs", false, 31)]
    public static void ReSaveAllLoadedFSMs()
    {
        LoadPrefabsWithPlayMakerFSMComponents();
        SaveAllLoadedFSMs();
        SaveAllTemplates();
    }

    [MenuItem(MenuRoot + "Tools/Re-Save All FSMs in Build", false, 32)]
    public static void ReSaveAllFSMsInBuild()
    {
        LoadPrefabsWithPlayMakerFSMComponents();
        SaveAllFSMsInBuild();
        SaveAllTemplates();
    }

    private static void SaveAllTemplates()
    {
        Debug.Log("Re-Saving All Templates...");

        FsmEditorUtility.BuildTemplateList();
        foreach (var template in FsmEditorUtility.TemplateList)
        {
            FsmEditor.SetFsmDirty(template.fsm, false);
            Debug.Log("Re-save Template: " + template.name);
        }
    }

    private static void SaveAllLoadedFSMs()
    {
        foreach (var fsm in FsmEditor.FsmList)
        {
            Debug.Log("Re-save FSM: " + FsmEditorUtility.GetFullFsmLabel(fsm));
            FsmEditor.SetFsmDirty(fsm, false);
        }
    }

    private static void SaveAllFSMsInBuild()
    {
        foreach (var scene in EditorBuildSettings.scenes)
        {
            Debug.Log("Open Scene: " + scene.path);
            EditorApplication.OpenScene(scene.path);
            FsmEditor.RebuildFsmList();
            SaveAllLoadedFSMs();
            EditorApplication.SaveScene();
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
     * */
}

