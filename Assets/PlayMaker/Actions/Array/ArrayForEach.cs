using System;
using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Array)]
    [Tooltip("Iterate through the items in an Array and run an FSM on each item. NOTE: The FSM has to Finish before being run on the next item.")]
    public class ArrayForEach : RunFSMAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Array to iterate through.")]
        public FsmArray array;

        [HideTypeFilter]
        [MatchElementType("array")] 
        [UIHint(UIHint.Variable)] 
        [Tooltip("Store the item in a variable")]
        public FsmVar storeItem;

        [ActionSection("Run FSM")]

        public FsmTemplateControl fsmTemplateControl = new FsmTemplateControl();

        [Tooltip("Event to send after iterating through all items in the Array.")]
        public FsmEvent finishEvent;
    
        private int currentIndex;

        public override void Reset()
        {
            array = null;
            fsmTemplateControl = new FsmTemplateControl();
            runFsm = null;
        }

        /// <summary>
        /// Initialize FSM on awake so it doesn't cause hitches later
        /// </summary>
        public override void Awake()
        {
            if (array != null && fsmTemplateControl.fsmTemplate != null && Application.isPlaying)
            {
                runFsm = Fsm.CreateSubFsm(fsmTemplateControl);
            }
        }

	    public override void OnEnter()
	    {
            if (array == null || runFsm == null)
            {
                Finish();
                return;
            }

	        currentIndex = 0;
            StartFsm();
	    }

        public override void OnUpdate()
        {
            runFsm.Update();
            if (!runFsm.Finished)
            {
                return; // continue later
            }

            StartNextFsm();
        }

        public override void OnFixedUpdate()
        {
            runFsm.LateUpdate();
            if (!runFsm.Finished)
            {
                return; // continue later
            }

            StartNextFsm();
        }

        public override void OnLateUpdate()
        {
            runFsm.LateUpdate();
            if (!runFsm.Finished)
            {
                return; // continue later
            }

            StartNextFsm();
        }

        void StartNextFsm()
        {
            currentIndex++;
            StartFsm();
        }

        void StartFsm()
        {
            while (currentIndex < array.Length)
            {
                DoStartFsm();
                if (!runFsm.Finished)
                {
                    return; // continue later
                }
                currentIndex++;
            }

            Fsm.Event(finishEvent);
            Finish();
        }

        void DoStartFsm()
        {
            storeItem.SetValue(array.Values[currentIndex]);

            fsmTemplateControl.UpdateValues();
            fsmTemplateControl.ApplyOverrides(runFsm);

            runFsm.OnEnable();

            if (!runFsm.Started)
            {
                runFsm.Start();
            }
        }

        protected override void CheckIfFinished()
        {
        }
    }


}
