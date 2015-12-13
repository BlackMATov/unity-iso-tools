// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System.ComponentModel;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;

[Localizable(false)]
static class PlayMakerMainMenu
{
    // Change MenuRoot to move the Playmaker Menu
    // E.g., MenuRoot = "Plugins/PlayMaker/"
    private const string MenuRoot = "PlayMaker/"; 

	[MenuItem(MenuRoot + "PlayMaker Editor", false, 1)]
	public static void OpenFsmEditor()
	{
		FsmEditorWindow.OpenWindow();
	}

	#region EDITOR WINDOWS 

    // priority starts at 10, leaving room for more items above

    [MenuItem(MenuRoot + "Editor Windows/FSM Browser", true)]
	public static bool ValidateOpenFsmSelectorWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem(MenuRoot + "Editor Windows/FSM Browser", false, 10)]
	public static void OpenFsmSelectorWindow()
	{
		FsmEditor.OpenFsmSelectorWindow();
	}

	[MenuItem(MenuRoot + "Editor Windows/State Browser", true)]
	public static bool ValidateOpenStateSelectorWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem(MenuRoot + "Editor Windows/State Browser", false, 11)]
	public static void OpenStateSelectorWindow()
	{
		FsmEditor.OpenStateSelectorWindow();
	}

	[MenuItem(MenuRoot + "Editor Windows/Templates Browser", true)]
	public static bool ValidateOpenFsmTemplateWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem(MenuRoot + "Editor Windows/Templates Browser", false, 12)]
	public static void OpenFsmTemplateWindow()
	{
		FsmEditor.OpenFsmTemplateWindow();
	}

	[MenuItem(MenuRoot + "Editor Windows/Edit Tool Window", true)]
	public static bool ValidateOpenToolWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem(MenuRoot + "Editor Windows/Edit Tool Window", false, 13)]
	public static void OpenToolWindow()
	{
		FsmEditor.OpenToolWindow();
	}

	[MenuItem(MenuRoot + "Editor Windows/Action Browser", true)]
	public static bool ValidateOpenActionWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem(MenuRoot + "Editor Windows/Action Browser", false, 14)]
	public static void OpenActionWindow()
	{
		FsmEditor.OpenActionWindow();
	}

	[MenuItem(MenuRoot + "Editor Windows/Global Variables", true)]
	public static bool ValidateOpenGlobalVariablesWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem(MenuRoot + "Editor Windows/Global Variables", false, 15)]
	public static void OpenGlobalVariablesWindow()
	{
		FsmEditor.OpenGlobalVariablesWindow();
	}

	[MenuItem(MenuRoot + "Editor Windows/Event Browser", true)]
	public static bool ValidateOpenGlobalEventsWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem(MenuRoot + "Editor Windows/Event Browser", false, 16)]
	public static void OpenGlobalEventsWindow()
	{
		FsmEditor.OpenGlobalEventsWindow();
	}

	[MenuItem(MenuRoot + "Editor Windows/Log Window", true)]
	public static bool ValidateOpenFsmLogWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem(MenuRoot + "Editor Windows/Log Window", false, 17)]
	public static void OpenFsmLogWindow()
	{
		FsmEditor.OpenFsmLogWindow();
	}

    [MenuItem(MenuRoot + "Editor Windows/Timeline Log", true)]
    public static bool ValidateOpenTimelineWindow()
    {
        return FsmEditorWindow.IsOpen();
    }

    [MenuItem(MenuRoot + "Editor Windows/Timeline Log", false, 16)]
    public static void OpenTimelineWindow()
    {
        FsmEditor.OpenTimelineWindow();
    }

	[MenuItem(MenuRoot + "Editor Windows/Editor Log", true)]
	public static bool ValidateOpenReportWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem(MenuRoot + "Editor Windows/Editor Log", false, 18)]
	public static void OpenReportWindow()
	{
		FsmEditor.OpenReportWindow();
	}

/* Enable when window is implemeneted
    [MenuItem(MenuRoot + "Editor Windows/Search", true)]
    public static bool ValidateOpenSearchWindow()
    {
        return FsmEditorWindow.IsOpen();
    }

    [MenuItem(MenuRoot + "Editor Windows/Search", false, 19)]
    public static void OpenSearchWindow()
    {
        FsmEditor.OpenSearchWindow();
    }
*/

	#endregion

	#region COMPONENTS

    // priority starts at 30, leaving room for more items above

	[MenuItem(MenuRoot + "Components/Add FSM To Selected Objects", true)]
	public static bool ValidateAddFsmToSelected()
	{
		return Selection.activeGameObject != null;
	}

	[MenuItem(MenuRoot + "Components/Add FSM To Selected Objects", false, 19)]
	public static void AddFsmToSelected()
	{
		FsmBuilder.AddFsmToSelected();
		//PlayMakerFSM playmakerFSM = Selection.activeGameObject.AddComponent<PlayMakerFSM>();
		//FsmEditor.SelectFsm(playmakerFSM.Fsm);
	}

	[MenuItem(MenuRoot + "Components/Add PlayMakerGUI to Scene", true)]
	public static bool ValidateAddPlayMakerGUI()
	{
		return (Object.FindObjectOfType(typeof(PlayMakerGUI)) as PlayMakerGUI) == null;
	}

	[MenuItem(MenuRoot + "Components/Add PlayMakerGUI to Scene", false, 20)]
	public static void AddPlayMakerGUI()
	{
		PlayMakerGUI.Instance.enabled = true;
	}

    /* Uncomment to make asset
    [MenuItem("Assets/Create/PlayMakerPrefs")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<PlayMakerPrefs>();
    }*/

	#endregion

	#region TOOLS

	[MenuItem(MenuRoot + "Tools/Load All PlayMaker Prefabs In Project", false, 25)]
	public static void LoadAllPrefabsInProject()
	{
		var paths = FsmEditorUtility.LoadAllPrefabsInProject();

		if (paths.Count == 0)
		{
			EditorUtility.DisplayDialog("Loading PlayMaker Prefabs", "No PlayMaker Prefabs Found!", "OK");
		}
		else
		{
			EditorUtility.DisplayDialog("Loaded PlayMaker Prefabs", "Prefabs found: " + paths.Count +"\nCheck console for details...", "OK");
		}
	}

	[MenuItem(MenuRoot + "Tools/Custom Action Wizard", false, 26)]
    public static void CreateWizard()
	{
		EditorWindow.GetWindow<PlayMakerCustomActionWizard>(true);
	}

	[MenuItem(MenuRoot + "Tools/Export Globals", false, 27)]
    public static void ExportGlobals()
	{
		FsmEditorUtility.ExportGlobals();
	}

	
	[MenuItem(MenuRoot + "Tools/Import Globals", false, 28)]
    public static void ImportGlobals()
	{
		FsmEditorUtility.ImportGlobals();
	}

	[MenuItem(MenuRoot + "Tools/Documentation Helpers", false, 29)]
    public static void DocHelpers()
	{
		EditorWindow.GetWindow<PlayMakerDocHelpers>(true);
	}

#if UNITY_5_0 || UNITY_5
    [MenuItem(MenuRoot + "Tools/Run AutoUpdater", false, 30)]
    public static void RunAutoUpdater()
    {
        PlayMakerAutoUpdater.RunAutoUpdate();
    }
#endif

	#endregion

	#region DOCUMENTATION

	[MenuItem(MenuRoot + "Online Resources/HutongGames", false, 35)]
	public static void HutongGames()
	{
		Application.OpenURL("http://www.hutonggames.com/");
	}

    [MenuItem(MenuRoot + "Online Resources/Online Manual", false, 36)]
	public static void OnlineManual()
	{
		EditorCommands.OpenWikiHelp();
		//Application.OpenURL("https://hutonggames.fogbugz.com/default.asp?W1");
	}

    [MenuItem(MenuRoot + "Online Resources/Video Tutorials", false, 37)]
	public static void VideoTutorials()
	{
		Application.OpenURL("http://www.screencast.com/users/HutongGames/folders/PlayMaker");
	}

    [MenuItem(MenuRoot + "Online Resources/YouTube Channel", false, 38)]
	public static void YouTubeChannel()
	{
		Application.OpenURL("http://www.youtube.com/user/HutongGamesLLC");
	}

    [MenuItem(MenuRoot + "Online Resources/PlayMaker Forums", false, 39)]
	public static void PlayMakerForum()
	{
		Application.OpenURL("http://hutonggames.com/playmakerforum/");
	}

	//[MenuItem(MenuRoot + "Documentation/")]
    [MenuItem(MenuRoot + "Online Resources/Release Notes", false, 40)]
	public static void ReleaseNotes()
	{
		EditorCommands.OpenWikiPage(WikiPages.ReleaseNotes);
		//Application.OpenURL("https://hutonggames.fogbugz.com/default.asp?W311");
	}

	#endregion

    [MenuItem(MenuRoot + "Tools/Submit Bug Report", false, 30)]
    public static void SubmitBug()
	{
		EditorWindow.GetWindow<PlayMakerBugReportWindow>(true);
	}

    [MenuItem(MenuRoot + "Welcome Screen", false, 45)]
	public static void OpenWelcomeWindow()
	{
		EditorWindow.GetWindow<PlayMakerWelcomeWindow>(true);
	}

	//http://u3d.as/content/hutong-games-llc/playmaker/1Az

/*	[MenuItem(MenuRoot + "Check For Updates")]
	public static void CheckForUpdates()
	{
		AssetStore.Open("1z");
	}*/
    
    /* Moved to WelcomeWindow.cs
    [MenuItem(MenuRoot + "Upgrade Guide", false, 46)]
    public static void OpenUpgradeGuide()
    {
        EditorWindow.GetWindow<PlayMakerUpgradeGuide>(true);
    }*/

	[MenuItem(MenuRoot + "About PlayMaker...", false, 47)]
	public static void OpenAboutWindow()
	{
		EditorWindow.GetWindow<AboutWindow>(true);
    }


    #region ADDONS

    
    [MenuItem(MenuRoot + "Addons/Addons Online")]
    public static void OpenAddonsWiki()
    {
        Application.OpenURL("https://hutonggames.fogbugz.com/default.asp?W714");
    }

#if !UNITY_5

    /* No longer needed...
    [MenuItem(MenuRoot + "Addons/BlackBerry Add-on")]
    public static void GetBlackBerryAddon()
    {
        UnityEditorInternal.AssetStore.Open("content/10530");
    }*/

    [MenuItem(MenuRoot + "Addons/Windows Phone 8 Add-on")]
    public static void GetWindowsPhone8Addon()
    {
        UnityEditorInternal.AssetStore.Open("content/10602");
    }

#endif

    #endregion
}
