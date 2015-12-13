using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.StateMachine)]
    [Tooltip("Set the value of a String Variable in another FSM.")]
    public class SetFsmEnum : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The GameObject that owns the FSM.")]
        public FsmOwnerDefault gameObject;

        [UIHint(UIHint.FsmName)]
        [Tooltip("Optional name of FSM on Game Object.")]
        public FsmString fsmName;

        [RequiredField]
        [UIHint(UIHint.FsmEnum)]
        [Tooltip("Enum variable name needs to match the FSM variable name on Game Object.")]
        public FsmString variableName; 

        [RequiredField]
        //[MatchFieldType("variableName")]
        public FsmEnum setValue;

        [Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;

        GameObject goLastFrame;
        string fsmNameLastFrame;

        PlayMakerFSM fsm;

        public override void Reset()
        {
            gameObject = null;
            fsmName = "";
            setValue = null;
        }

        public override void OnEnter()
        {
            DoSetFsmEnum();

            if (!everyFrame) 
            {
                Finish();
            }
        }

        void DoSetFsmEnum()
        {
            if (setValue == null) 
            {
                return;
            }

            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null) 
            {
                return;
            }

            // FIX: must check as well that the fsm name is different.
            if (go != goLastFrame || fsmName.Value != fsmNameLastFrame) 
            {
                goLastFrame = go;
                fsmNameLastFrame = fsmName.Value;
                // only get the fsm component if go or fsm name has changed

                fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
            }

            if (fsm == null) 
            {
                LogWarning("Could not find FSM: " + fsmName.Value);
                return;
            }
            
            var fsmEnum = fsm.FsmVariables.GetFsmEnum(variableName.Value);
            if (fsmEnum != null) 
            {
                fsmEnum.Value = setValue.Value;
            } 
            else 
            {
                LogWarning("Could not find variable: " + variableName.Value);
            }
        }

        public override void OnUpdate()
        {
            DoSetFsmEnum();
        }

    }
}
