using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    // Base class for logging actions 
    public abstract class BaseLogAction : FsmStateAction
    {
        public bool sendToUnityLog;

        public override void Reset()
        {
            sendToUnityLog = false;
        }
    }
}
