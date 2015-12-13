// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
// micro script by Andrew Raphael Lukasik

#if (UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
#define UNITY_PRE_5_3
#endif

using UnityEngine;
#if !UNITY_PRE_5_3
using UnityEngine.SceneManagement;
#endif


namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Level)]
    [Tooltip("Restarts current level.")]
    public class RestartLevel : FsmStateAction
    {
        public override void OnEnter()
        {
#if UNITY_PRE_5_3
            Application.LoadLevel(Application.loadedLevelName);
#else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
#endif
            Finish();
        }
    }
}