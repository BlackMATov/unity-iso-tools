// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Logic)]
    [Tooltip("Sends an Event based on the value of an Enum Variable.")]
    public class EnumSwitch : FsmStateAction
    {
        [RequiredField] 
        [UIHint(UIHint.Variable)] 
        public FsmEnum enumVariable;

        [CompoundArray("Enum Switches", "Compare Enum Values", "Send")] 
        [MatchFieldType("enumVariable")] 
        public FsmEnum[] compareTo;      
        public FsmEvent[] sendEvent;

        public bool everyFrame;

        public override void Reset()
        {
            enumVariable = null;
            compareTo = new FsmEnum[0];
            sendEvent = new FsmEvent[0];
            everyFrame = false;
        }

        public override void OnEnter()
        {
            DoEnumSwitch();

            if (!everyFrame)
                Finish();
        }

        public override void OnUpdate()
        {
            DoEnumSwitch();
        }

        private void DoEnumSwitch()
        {
            if (enumVariable.IsNone)
                return;

            for (int i = 0; i < compareTo.Length; i++)
            {
                if (Equals(enumVariable.Value, compareTo[i].Value))
                {
                    Fsm.Event(sendEvent[i]);
                    return;
                }
            }
        }
    }
}