// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Enum)]
    [Tooltip("Sets the value of an Enum Variable.")]
    public class SetEnumValue : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [Tooltip("The Enum Variable to set.")]
        public FsmEnum enumVariable;

        [MatchFieldType("enumVariable")]
        [Tooltip("The Enum value to set the variable to.")]
        public FsmEnum enumValue;

        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

        public override void Reset()
        {
            enumVariable = null;
            enumValue = null;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            DoSetEnumValue();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoSetEnumValue();
        }

        void DoSetEnumValue()
        {
            enumVariable.Value = enumValue.Value;
        }

    }
}