using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMakerEditor
{
    [CustomActionEditor(typeof (LookAt))]
    public class LookAtActionEditor : CustomActionEditor
    {
        public override bool OnGUI()
        {
            return DrawDefaultInspector();
        }

        public override void OnSceneGUI()
        {
            var lookAtAction = (LookAt) target;

            if (lookAtAction.UpdateLookAtPosition())
            {
                var go = target.Fsm.GetOwnerDefaultTarget(lookAtAction.gameObject);
                var goTransform = go.transform;
                var goPosition = goTransform.position;

                var lookAtPosition = lookAtAction.GetLookAtPosition();
                var lookAtVector = lookAtPosition - goPosition;
                var lookAtRotation = Quaternion.LookRotation(lookAtVector);
                var lookAtAngle = Vector3.Angle(goTransform.forward, lookAtVector);
                var lookAtNormal = Vector3.Cross(goTransform.forward, lookAtVector);

                var handleSize = HandleUtility.GetHandleSize(goPosition);
                var arrowSize = handleSize*0.2f;
                var distance = (lookAtPosition - goPosition).magnitude;

                var goTarget = lookAtAction.targetObject.Value;

                // Position handles

                if (!lookAtAction.targetPosition.IsNone)
                {
                    if (goTarget != null)
                    {
                        // Edit local offset from target object

                        var goTargetTransform = goTarget.transform;
                        var worldTargetPos = goTargetTransform.TransformPoint(lookAtAction.targetPosition.Value);

                    lookAtAction.targetPosition.Value = goTargetTransform.InverseTransformPoint(Handles.PositionHandle(worldTargetPos, goTarget.transform.rotation));
                        Handles.color = new Color(1, 1, 1, 0.2f);
                        Handles.DrawLine(goTargetTransform.position, lookAtAction.GetLookAtPositionWithVertical());
                    }
                    else
                    {
                        // Edit world position

                    lookAtAction.targetPosition.Value = Handles.PositionHandle(lookAtAction.targetPosition.Value, Quaternion.identity);
                    }
                }

                // Forward vector

                Handles.color = Color.blue;
                Handles.DrawLine(goPosition, goPosition + goTransform.forward*handleSize);

                // Lookat vector

                Handles.DrawLine(goPosition, lookAtPosition);
                Handles.ConeCap(0, goPosition + lookAtVector.normalized * (distance - arrowSize * 0.7f)  , lookAtRotation, arrowSize); // fudge factor to position cap correctly

                // Arc between vectors

                Handles.color = new Color(1, 1, 1, 0.2f);
                Handles.DrawSolidArc(goPosition, lookAtNormal, goTransform.forward, lookAtAngle, handleSize);

                // Show vertical offset

                if (lookAtAction.keepVertical.Value)
                {
                    Handles.DrawLine(lookAtPosition, lookAtAction.GetLookAtPositionWithVertical());
                }

                if (GUI.changed)
                {
                    FsmEditor.EditingActions();
                    FsmEditor.Repaint(true);
                }
            }
        }
    }
}
