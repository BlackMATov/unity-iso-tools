// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Array)]
    [Tooltip("Resize an array.")]
    public class ArrayResize : FsmStateAction
    {
        [RequiredField] [UIHint(UIHint.Variable)] [Tooltip("The Array Variable to resize")] public FsmArray array;

        [Tooltip("The new size of the array.")] public FsmInt newSize;

        [UIHint(UIHint.FsmEvent)] [Tooltip("The event to trigger if the new size is out of range")] public FsmEvent
            sizeOutOfRangeEvent;

        public override void OnEnter()
        {
            if (newSize.Value >= 0)
            {
                array.Resize(newSize.Value);
            }
            else
            {
                LogError("Size out of range: " + newSize.Value);
                Fsm.Event(sizeOutOfRangeEvent);
            }

            Finish();
        }

        /* Should be disallowed by the UI now
        public override string ErrorCheck()
        {
            if (newSize.Value<0)
            {
                return "newSize must be a positive value.";
            }
            return "";
        }*/

    }
}
