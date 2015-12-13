// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Array)]
    [Tooltip("Sort items in an Array.")]
    public class ArraySort : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)] 
        [Tooltip("The Array to sort.")] 
        public FsmArray array;

        public override void Reset()
        {
            array = null;
        }

        public override void OnEnter()
        {
            var list = new List<object>(array.Values);
            list.Sort();
            array.Values = list.ToArray();

            Finish();
        }
    }
}