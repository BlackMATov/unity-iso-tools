// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

/* NOTE: Wrapper no longer needed in Unity 4.x
 * BUT: changing it breaks saved layouts
 * SO: wrap in namespace instead (supported in 4.x)
 */

// EditorWindow classes can't be called from a dll in Unity 3.5
// so create a thin wrapper class as a workaround

namespace HutongGames.PlayMakerEditor
{
    internal class FsmSelectorWindow : FsmSelector
    {
    }
}
