// (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.StateMachine)]
    [Tooltip("Creates an FSM from a saved FSM Template.")]
    public class RunFSM : RunFSMAction
    {
        public FsmTemplateControl fsmTemplateControl = new FsmTemplateControl();

        [UIHint(UIHint.Variable)]
        public FsmInt storeID;

        [Tooltip("Event to send when the FSM has finished (usually because it ran a Finish FSM action).")]
        public FsmEvent finishEvent;

        public override void Reset()
        {
            fsmTemplateControl = new FsmTemplateControl();
            storeID = null;
            runFsm = null;
        }

        /// <summary>
        /// Initialize FSM on awake so it doesn't cause hitches later
        /// </summary>
        public override void Awake()
        {
            if (fsmTemplateControl.fsmTemplate != null && Application.isPlaying)
            {
                runFsm = Fsm.CreateSubFsm(fsmTemplateControl);
            }
        }

        /// <summary>
        /// Start the FSM on entering the state
        /// </summary>
        public override void OnEnter()
        {
            if (runFsm == null)
            {
                Finish();
                return;
            }

            fsmTemplateControl.UpdateValues();
            fsmTemplateControl.ApplyOverrides(runFsm);

            runFsm.OnEnable();

            if (!runFsm.Started)
            {
                runFsm.Start();
            }

            storeID.Value = fsmTemplateControl.ID;

            CheckIfFinished();
        }

        // Other functionality covered in RunFSMAction base class

        protected override void CheckIfFinished()
        {
            if (runFsm == null || runFsm.Finished)
            {
                Finish();
                Fsm.Event(finishEvent);
            }
        }
    }
}
