// (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.StateMachine)]
    [Tooltip("Get the value of a variable in another FSM and store it in a variable of the same name in this FSM.")]
    public class GetFsmVariable : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The GameObject that owns the FSM")]
        public FsmOwnerDefault gameObject;

        [UIHint(UIHint.FsmName)]
        [Tooltip("Optional name of FSM on Game Object")]
        public FsmString fsmName;
        
        [RequiredField]
        [HideTypeFilter]
        [UIHint(UIHint.Variable)]
        public FsmVar storeValue;

        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

        private GameObject cachedGO;
        private PlayMakerFSM sourceFsm;
        private INamedVariable sourceVariable;
        private NamedVariable targetVariable;

        public override void Reset()
        {
            gameObject = null;
            fsmName = "";
            storeValue = new FsmVar();
        }

        public override void OnEnter()
        {
            InitFsmVar();

            DoGetFsmVariable();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoGetFsmVariable();
        }

        void InitFsmVar()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return;
            }

            if (go != cachedGO)
            {
                sourceFsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
                sourceVariable = sourceFsm.FsmVariables.GetVariable(storeValue.variableName);
                targetVariable = Fsm.Variables.GetVariable(storeValue.variableName);
                storeValue.Type = targetVariable.VariableType;

                if (!string.IsNullOrEmpty(storeValue.variableName) && sourceVariable == null)
                {
                    LogWarning("Missing Variable: " + storeValue.variableName);
                }

                cachedGO = go;
            }
        }

        void DoGetFsmVariable()
        {
            if (storeValue.IsNone)
            {
                return;
            }

            InitFsmVar();
            storeValue.GetValueFrom(sourceVariable);
            storeValue.ApplyValueTo(targetVariable);
        }

#if UNITY_EDITOR
        public override string AutoName()
        {
            return ("Get FSM Variable: " + ActionHelpers.GetValueLabel(storeValue.NamedVar));
        }
#endif
    }
}
