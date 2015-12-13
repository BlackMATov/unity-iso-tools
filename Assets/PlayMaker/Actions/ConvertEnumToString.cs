// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Convert)]
    [Tooltip("Converts an Enum value to a String value.")]
    public class ConvertEnumToString : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("The Enum variable to convert.")]
        public FsmEnum enumVariable;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("The String variable to store the converted value.")]
        public FsmString stringVariable;

        [Tooltip("Repeat every frame. Useful if the Enum variable is changing.")]
        public bool everyFrame;

        public override void Reset()
        {
            enumVariable = null;
            stringVariable = null;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            DoConvertEnumToString();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoConvertEnumToString();
        }

        void DoConvertEnumToString()
        {
            stringVariable.Value = enumVariable.Value != null ? enumVariable.Value.ToString() : "";
        }
    }
}