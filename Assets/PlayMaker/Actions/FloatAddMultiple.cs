// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Math)]
    [Tooltip("Adds multipe float variables to float variable.")]
    public class FloatAddMultiple : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [Tooltip("The float variables to add.")]
        public FsmFloat[] floatVariables;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Add to this variable.")]
        public FsmFloat addTo;

        [Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;

        public override void Reset()
        {
            floatVariables = null;
            addTo = null;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            DoFloatAdd();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoFloatAdd();
        }

        void DoFloatAdd()
        {
            for (var i = 0; i < floatVariables.Length; i++)
            {
                addTo.Value += floatVariables[i].Value;
            }
        }
    }
}