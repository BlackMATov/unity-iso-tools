// Unity 5.1 introduced a new networking library. 
// Unless we define PLAYMAKER_LEGACY_NETWORK old network actions are disabled
#if !(UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || PLAYMAKER_LEGACY_NETWORK)
#define UNITY_NEW_NETWORK
#endif

// Some platforms do not support networking (at least the old network library)
#if (UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)
#define PLATFORM_NOT_SUPPORTED
#endif

using UnityEngine;
using System.Collections;


public class PlayMakerRPCProxy : MonoBehaviour
{
    public PlayMakerFSM[] fsms;

    public void Reset()
    {
        fsms = GetComponents<PlayMakerFSM>();
    }

#if !(PLATFORM_NOT_SUPPORTED || UNITY_NEW_NETWORK || PLAYMAKER_NO_NETWORK)
    [RPC]
#endif
    public void ForwardEvent(string eventName)
    {
        foreach (var fsm in fsms)
        {
            fsm.SendEvent(eventName);
        }
    }
}
