// (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.StateMachine)]
    [Tooltip("Get the values of multiple variables in another FSM and store in variables of the same name in this FSM.")]
    public class GetFsmVariables : FsmStateAction
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
        public FsmVar[] getVariables;

        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

        private GameObject cachedGO;
        private PlayMakerFSM sourceFsm;
        private INamedVariable[] sourceVariables;
        private NamedVariable[] targetVariables;

        public override void Reset()
        {
            gameObject = null;
            fsmName = "";
            getVariables = null;
        }

        void InitFsmVars()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return;
            }

            if (go != cachedGO)
            {
                sourceVariables = new INamedVariable[getVariables.Length];
                targetVariables = new NamedVariable[getVariables.Length];

                for (var i = 0; i < getVariables.Length; i++)
                {
                    var variableName = getVariables[i].variableName;
                    sourceFsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
                    sourceVariables[i] = sourceFsm.FsmVariables.GetVariable(variableName);
                    targetVariables[i] = Fsm.Variables.GetVariable(variableName);
                    getVariables[i].Type = targetVariables[i].VariableType;

                    if (!string.IsNullOrEmpty(variableName) && sourceVariables[i] == null)
                    {
                        LogWarning("Missing Variable: " + variableName);
                    }

                    cachedGO = go;
                }
            }
        }

        public override void OnEnter()
        {
            InitFsmVars();

            DoGetFsmVariables();
            
            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoGetFsmVariables();
        }

        void DoGetFsmVariables()
        {
            InitFsmVars();

            for (var i = 0; i < getVariables.Length; i++)
            {
                getVariables[i].GetValueFrom(sourceVariables[i]);
                getVariables[i].ApplyValueTo(targetVariables[i]);
            }
        }
    }
}
