// Small helper class to allow Fsm to call SetDirty
// Fsm is inside dll so cannot use #if UNITY_EDITOR

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
                EditorUtility.SetDirty(fsm.UsedInTemplate);
            }
            else if (fsm.Owner != null)
            {
                EditorUtility.SetDirty(fsm.Owner);
            }

#endif
        }
    }
}
