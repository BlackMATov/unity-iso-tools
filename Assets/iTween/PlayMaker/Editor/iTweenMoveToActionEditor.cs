using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomActionEditor(typeof(iTweenMoveTo))]
public class iTweenMoveToActionEditor : CustomActionEditor 
{
    public override bool OnGUI()
    {
        return DrawDefaultInspector();
    }

    public override void OnSceneGUI()
    {
        // Live iTween path editing

        var iTween = target as iTweenMoveTo;
        if (iTween == null) // shouldn't happen!
        {
            return;
        }

        var fsm = target.Fsm;
        if (fsm == null) // shouldn't happen!
        {
            return;
        }

        if (iTween.transforms.Length >= 2)
        {
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2
            Undo.SetSnapshotTarget(target.Owner, Strings.Command_Adjust_iTween_Path);
#else
            Undo.RecordObject(target.Owner, Strings.Command_Adjust_iTween_Path);
#endif
            var tempVct3 = new Vector3[iTween.transforms.Length];
            for (var i = 0; i < iTween.transforms.Length; i++)
            {
                if (iTween.transforms[i].IsNone) tempVct3[i] = iTween.vectors[i].IsNone ? Vector3.zero : iTween.vectors[i].Value;
                else
                {
                    if (iTween.transforms[i].Value == null)
                    {
                        tempVct3[i] = iTween.vectors[i].IsNone ? Vector3.zero : iTween.vectors[i].Value;
                    }
                    else
                    {
                        tempVct3[i] = iTween.transforms[i].Value.transform.position +
                                        (iTween.vectors[i].IsNone ? Vector3.zero : iTween.vectors[i].Value);
                    }
                }
                tempVct3[i] = Handles.PositionHandle(tempVct3[i], Quaternion.identity);
                if (iTween.transforms[i].IsNone)
                {
                    if (!iTween.vectors[i].IsNone)
                    {
                        iTween.vectors[i].Value = tempVct3[i];
                    }
                }
                else
                {
                    if (iTween.transforms[i].Value == null)
                    {
                        if (!iTween.vectors[i].IsNone)
                        {
                            iTween.vectors[i].Value = tempVct3[i];
                        }
                    }
                    else
                    {
                        if (!iTween.vectors[i].IsNone)
                        {
                            iTween.vectors[i] = tempVct3[i] - iTween.transforms[i].Value.transform.position;
                        }
                    }
                }
            }

            Handles.Label(tempVct3[0], string.Format(Strings.iTween_Path_Editing_Label_Begin, fsm.Name));
            Handles.Label(tempVct3[tempVct3.Length - 1], string.Format(Strings.iTween_Path_Editing_Label_End, fsm.Name));

            if (GUI.changed)
            {
                FsmEditor.EditingActions();
                FsmEditor.Repaint(true);
            }
        }
   
    }
}
