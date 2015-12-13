// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Transform)]
    [Tooltip("Smoothly Rotates a Game Object so its forward vector points in the specified Direction. Lets you fire an event when minmagnitude is reached")]
    public class SmoothLookAtDirection : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The GameObject to rotate.")]
        public FsmOwnerDefault gameObject;

        [RequiredField]
        [Tooltip("The direction to smoothly rotate towards.")]
        public FsmVector3 targetDirection;

        [Tooltip("Only rotate if Target Direction Vector length is greater than this threshold.")]
        public FsmFloat minMagnitude;

        [Tooltip("Keep this vector pointing up as the GameObject rotates.")]
        public FsmVector3 upVector;

        [RequiredField]
        [Tooltip("Eliminate any tilt up/down as the GameObject rotates.")]
        public FsmBool keepVertical;

        [RequiredField]
        [HasFloatSlider(0.5f, 15)]
        [Tooltip("How quickly to rotate.")]
        public FsmFloat speed;

        [Tooltip("Perform in LateUpdate. This can help eliminate jitters in some situations.")]
        public bool lateUpdate;

        [Tooltip("Event to send if the direction difference is less than Min Magnitude.")]
        public FsmEvent finishEvent;

        GameObject previousGo; // track game object so we can re-initialize when it changes.
        Quaternion lastRotation;
        Quaternion desiredRotation;


        private bool _done;

        public override void Reset()
        {
            gameObject = null;
            targetDirection = new FsmVector3 { UseVariable = true };
            minMagnitude = 0.1f;
            upVector = new FsmVector3 { UseVariable = true };
            keepVertical = true;
            speed = 5;
            lateUpdate = true;
            finishEvent = null;
        }

        public override void OnEnter()
        {
            previousGo = null;
        }

        public override void OnUpdate()
        {
            if (!lateUpdate)
            {
                DoSmoothLookAtDirection();
            }
        }

        public override void OnLateUpdate()
        {
            if (lateUpdate)
            {
                DoSmoothLookAtDirection();
            }
        }

        void DoSmoothLookAtDirection()
        {
            if (targetDirection.IsNone)
            {
                return;
            }

            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return;
            }

            // re-initialize if game object has changed

            if (previousGo != go)
            {
                lastRotation = go.transform.rotation;
                desiredRotation = lastRotation;
                previousGo = go;
            }

            // desired direction

            var diff = targetDirection.Value;

            if (keepVertical.Value)
            {
                diff.y = 0;
            }


            desiredRotation = Quaternion.LookRotation(diff, upVector.IsNone ? Vector3.up : upVector.Value);

            lastRotation = Quaternion.Slerp(lastRotation, desiredRotation, speed.Value * Time.deltaTime);
            go.transform.rotation = lastRotation;


            if (Quaternion.Angle(desiredRotation, go.transform.rotation) < minMagnitude.Value)
            {
                if (!_done)
                {
                    _done = true;
                    Fsm.Event(finishEvent);
                    Finish();
                }
            }
            else
            {
                _done = false;
            }

        }
    }
}