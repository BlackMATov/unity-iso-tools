// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Array)]
    [Tooltip("Gets the number of items in an Array.")]
    public class ArrayLength : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [Tooltip("The Array Variable.")]
        public FsmArray array;

        [UIHint(UIHint.Variable)] 
        [Tooltip("Store the length in an Int Variable.")]
        public FsmInt length;

        [Tooltip("Repeat every frame. Useful if the array is changing and you're waiting for a particular length.")]
        public bool everyFrame;

        public override void Reset()
        {
            array = null;
            length = null;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            length.Value = array.Length;

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            length.Value = array.Length;
        }
    }
}