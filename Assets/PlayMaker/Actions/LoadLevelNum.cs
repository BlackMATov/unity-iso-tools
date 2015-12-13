// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

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
    [Tooltip("Loads a Level by Index number. Before you can load a level, you have to add it to the list of levels defined in File->Build Settings...")]
    public class LoadLevelNum : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The level index in File->Build Settings")]
        public FsmInt levelIndex;

        [Tooltip("Load the level additively, keeping the current scene.")]
        public bool additive;

        [Tooltip("Event to send after the level is loaded.")]
        public FsmEvent loadedEvent;

        [Tooltip("Keep this GameObject in the new level. NOTE: The GameObject and components is disabled then enabled on load; uncheck Reset On Disable to keep the active state.")]
        public FsmBool dontDestroyOnLoad;

        public override void Reset()
        {
            levelIndex = null;
            additive = false;
            loadedEvent = null;
            dontDestroyOnLoad = false;
        }

        public override void OnEnter()
        {
            if (dontDestroyOnLoad.Value)
            {
                // Have to get the root, since this FSM will be destroyed if a parent is destroyed.

                var root = Owner.transform.root;
                Object.DontDestroyOnLoad(root.gameObject);
            }

            if (additive)
            {
#if UNITY_PRE_5_3
                Application.LoadLevelAdditive(levelIndex.Value);
#else
                SceneManager.LoadScene(levelIndex.Value, LoadSceneMode.Additive);
#endif
            }
            else
            {
#if UNITY_PRE_5_3
                Application.LoadLevel(levelIndex.Value);
#else
                SceneManager.LoadScene(levelIndex.Value);
#endif
            }

            Fsm.Event(loadedEvent);
            Finish();
        }
    }
}