// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Array)]
    [Tooltip("Set the value at an index. Index must be between 0 and the number of items -1. First item is index 0.")]
    public class ArraySet : FsmStateAction
    {
        [RequiredField] 
        [UIHint(UIHint.Variable)] 
        [Tooltip("The Array Variable to use.")] 
        public FsmArray array;

        [Tooltip("The index into the array.")] 
        public FsmInt index;

        [RequiredField]
        [MatchElementType("array")] 
        [Tooltip("Set the value of the array at the specified index.")] 
        public FsmVar value;

        [Tooltip("Repeat every frame while the state is active.")] 
        public bool everyFrame;

        [ActionSection("Events")] 

        [UIHint(UIHint.FsmEvent)] 
        [Tooltip("The event to trigger if the index is out of range")] 
        public FsmEvent indexOutOfRange;

        public override void Reset()
        {
            array = null;
            index = null;
            value = null;
            everyFrame = false;
            indexOutOfRange = null;
        }

        public override void OnEnter()
        {
            DoGetValue();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoGetValue();
        }

        private void DoGetValue()
        {
            if (array.IsNone)
            {
                return;
            }

            if (index.Value >= 0 && index.Value < array.Length)
            {
                value.UpdateValue();
                array.Set(index.Value, value.GetValue());
            }
            else
            {
                //LogError("Index out of Range: " + index.Value);
                Fsm.Event(indexOutOfRange);
            }
        }

        /*
        public override string ErrorCheck()
        {
            if (index.Value<0 || index.Value >= array.Length)
            {
                return "Index out of Range. Please select an index between 0 and the number of items -1. First item is index 0.";
            }
            return "";
        }*/

    }
}
