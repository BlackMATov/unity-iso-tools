// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Array)]
    [Tooltip("Delete the item at an index. Index must be between 0 and the number of items -1. First item is index 0.")]
    public class ArrayDeleteAt : FsmStateAction
    {
        [RequiredField] [UIHint(UIHint.Variable)] [Tooltip("The Array Variable to use.")] public FsmArray array;

        [Tooltip("The index into the array.")] public FsmInt index;

        [ActionSection("Result")] [UIHint(UIHint.FsmEvent)] [Tooltip("The event to trigger if the index is out of range")] public FsmEvent indexOutOfRangeEvent;



        public override void Reset()
        {
            array = null;
            index = null;
            indexOutOfRangeEvent = null;
        }


        // Code that runs on entering the state.
        public override void OnEnter()
        {
            DoDeleteAt();

            Finish();
        }

        private void DoDeleteAt()
        {
            if (index.Value >= 0 && index.Value < array.Length)
            {
                List<object> _list = new List<object>(array.Values);
                _list.RemoveAt(index.Value);
                array.Values = _list.ToArray();
            }
            else
            {
                Fsm.Event(indexOutOfRangeEvent);
            }
        }

        /* Not sure it's a good idea to check range at edit time since it can change at runtime
        public override string ErrorCheck()
        {
            if (array.Length==0)
            {
                if (index.Value<0)
                {
                    return "Index out of Range. Please select a positive number. First item is index 0.";
                }
                return "";
            }

            if (index.Value<0 || index.Value >= array.Length)
            {
                return "Index out of Range. Please select an index between 0 and the number of items -1. First item is index 0.";
            }
            return "";
        }*/

    }
}
