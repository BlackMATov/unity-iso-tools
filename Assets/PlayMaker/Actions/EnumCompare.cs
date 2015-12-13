using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Logic)]
    [Tooltip("Compares 2 Enum values and sends Events based on the result.")]
    public class EnumCompare : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        public FsmEnum enumVariable;

        [MatchFieldType("enumVariable")]
        public FsmEnum compareTo;

        public FsmEvent equalEvent;

        public FsmEvent notEqualEvent;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the true/false result in a bool variable.")]
        public FsmBool storeResult;
        
        [Tooltip("Repeat every frame. Useful if the enum is changing over time.")]
        public bool everyFrame;

        public override void Reset()
        {
            enumVariable = null;
            compareTo = null;
            equalEvent = null;
            notEqualEvent = null;
            storeResult = null;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            DoEnumCompare();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoEnumCompare();
        }

        void DoEnumCompare()
        {
            if (enumVariable == null || compareTo == null) return;

            var equal = Equals(enumVariable.Value, compareTo.Value);

            if (storeResult != null)
            {
                storeResult.Value = equal;
            }

            if (equal && equalEvent != null)
            {
                Fsm.Event(equalEvent);
                return;
            }

            if (!equal && notEqualEvent != null)
            {
                Fsm.Event(notEqualEvent);
            }

        }

    }
}
