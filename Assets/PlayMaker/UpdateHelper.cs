// Small helper class to allow Fsm to call SetDirty
// Fsm is inside dll so cannot use #if UNITY_EDITOR

#if (UNITY_4_3 || UNITY_4_5 || UNITY_4_6)
#define UNITY_PRE_5_0
#endif

#if (UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
#define UNITY_PRE_5_3
#endif

using UnityEngine;

namespace HutongGames.PlayMaker
{
    /// <summary>
    /// Playmaker runtime code can't call unity editor code
    /// This class is a workaround, allowing runtime code to call EditorUtility.SetDirty
    /// </summary>
    public class UpdateHelper
    {
        public static void SetDirty(Fsm fsm)
        {
#if UNITY_EDITOR

            if (fsm == null || fsm.OwnerObject == null) return;

            //Debug.Log("SetDirty: " + FsmUtility.GetFullFsmLabel(fsm));

            fsm.Preprocessed = false; // force pre-process to run again

            if (fsm.UsedInTemplate != null)
            {
                UnityEditor.EditorUtility.SetDirty(fsm.UsedInTemplate);
            }
            else if (fsm.Owner != null)
            {
                UnityEditor.EditorUtility.SetDirty(fsm.Owner);
#if !UNITY_PRE_5_3
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(fsm.Owner.gameObject.scene);
#elif !UNITY_PRE_5_0
                // Not sure if we need to do this...?
                UnityEditor.EditorApplication.MarkSceneDirty();
#endif
            }
#endif
        }
    }
}
