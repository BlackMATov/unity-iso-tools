using HutongGames.PlayMakerEditor;
using UnityEditor;

[InitializeOnLoad]
public class PlayMakerUpdater 
{
    static PlayMakerUpdater()
    {
        // Delay until first update
        // Otherwise process gets stomped on by other Unity initializations
        // E.g., Unity loading last layout stomps on PlayMakerUpgradeGuide window.
        EditorApplication.update += Update;
    }

    static void Update()
    {
        EditorApplication.update -= Update;

        /*
        var showUpgradeGuide = EditorPrefs.GetBool(EditorPrefStrings.ShowUpgradeGuide, true);
        if (showUpgradeGuide)
        {
            EditorWindow.GetWindow<PlayMakerUpgradeGuide>(true);
        }*/
    }
}
