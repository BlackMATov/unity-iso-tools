using System.Linq;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace HutongGames.PlayMakerEditor
{
    public class PlayMakerBuildCallbacks
    {
        [PostProcessSceneAttribute(2)]
        public static void OnPostprocessScene()
        {
            /* TODO: Figure out if we need to do this!
            // OnPostprocessScene is called when loading a scene in the editor 
            // Might not want to post process in that case...?
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }*/

            //Debug.Log("OnPostprocessScene");

            PlayMakerGlobals.IsBuilding = true;
            PlayMakerGlobals.InitApplicationFlags();

            var fsmList = Resources.FindObjectsOfTypeAll<PlayMakerFSM>();
            foreach (var playMakerFSM in fsmList)
            {
                //Debug.Log(FsmEditorUtility.GetFullFsmLabel(playMakerFSM));
                
                if (!Application.isPlaying) // actually making a build vs playing in editor
                {
                    playMakerFSM.Preprocess();
                }
            }

            PlayMakerGlobals.IsBuilding = false;

            //Debug.Log("EndPostProcessScene");
        }
    }
}
