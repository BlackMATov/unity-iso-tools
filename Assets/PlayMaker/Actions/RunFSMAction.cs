// (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [Tooltip("Base class for actions that want to run a sub FSM.")]
    public abstract class RunFSMAction : FsmStateAction
    {
        protected Fsm runFsm;

        public override void Reset()
        {
            runFsm = null;
        }

        /// <summary>
        /// Forward global events to the sub FSM
        /// </summary>
        public override bool Event(FsmEvent fsmEvent)
        {
            if (runFsm != null && (fsmEvent.IsGlobal || fsmEvent.IsSystemEvent))
            {
                runFsm.Event(fsmEvent);
            }

            return false;
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

            runFsm.OnEnable();

            if (!runFsm.Started)
            {
                runFsm.Start();
            }

            CheckIfFinished();
        }

        public override void OnUpdate()
        {
            if (runFsm != null)
            {
                runFsm.Update();
                CheckIfFinished();
            }
            else
            {
                Finish();
            }
        }

        public override void OnFixedUpdate()
        {
            if (runFsm != null)
            {
                runFsm.FixedUpdate();
                CheckIfFinished();
            }
            else
            {
                Finish();
            }
        }

        public override void OnLateUpdate()
        {
            if (runFsm != null)
            {
                runFsm.LateUpdate();
                CheckIfFinished();
            }
            else
            {
                Finish();
            }
        }

        public override void DoTriggerEnter(Collider other)
        {
            if (runFsm.HandleTriggerEnter)
            {
                runFsm.OnTriggerEnter(other);
            }
        }

        public override void DoTriggerStay(Collider other)
        {
            if (runFsm.HandleTriggerStay)
            {
                runFsm.OnTriggerStay(other);
            }
        }

        public override void DoTriggerExit(Collider other)
        {
            if (runFsm.HandleTriggerExit)
            {
                runFsm.OnTriggerExit(other);
            }
        }

        public override void DoCollisionEnter(Collision collisionInfo)
        {
            if (runFsm.HandleCollisionEnter)
            {
                runFsm.OnCollisionEnter(collisionInfo);
            }
        }

        public override void DoCollisionStay(Collision collisionInfo)
        {
            if (runFsm.HandleCollisionStay)
            {
                runFsm.OnCollisionStay(collisionInfo);
            }
        }

        public override void DoCollisionExit(Collision collisionInfo)
        {
            if (runFsm.HandleCollisionExit)
            {
                runFsm.OnCollisionExit(collisionInfo);
            }
        }

        public override void DoControllerColliderHit(ControllerColliderHit collisionInfo)
        {
            runFsm.OnControllerColliderHit(collisionInfo);
        }

        public override void OnGUI()
        {
            if (runFsm != null && runFsm.HandleOnGUI)
            {
                runFsm.OnGUI();
            }
        }

        /// <summary>
        /// Stop the FSM on exiting the state
        /// </summary>
        public override void OnExit()
        {
            if (runFsm != null)
            {
                runFsm.Stop();
            }
        }

        protected virtual void CheckIfFinished()
        {
            if (runFsm == null || runFsm.Finished)
            {
                Finish();
            }
        }
    }
}
