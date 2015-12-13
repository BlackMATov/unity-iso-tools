using System.ComponentModel;
using HutongGames.PlayMakerEditor;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(FsmTemplate))]
public class FsmTemplateEditor : Editor
{
    private SerializedProperty categoryProperty;
    private SerializedProperty descriptionProperty;
    private GUIStyle multiline;

    [Localizable(false)]
    public void OnEnable()
    {
        categoryProperty = serializedObject.FindProperty("category");
        descriptionProperty = serializedObject.FindProperty("fsm.description");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(categoryProperty);

        if (multiline == null)
        {
            multiline = new GUIStyle(EditorStyles.textField) { wordWrap = true };
        }
        descriptionProperty.stringValue = EditorGUILayout.TextArea(descriptionProperty.stringValue, multiline, GUILayout.MinHeight(60));

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button(Strings.FsmTemplateEditor_Open_In_Editor))
        {
            FsmEditorWindow.OpenWindow((FsmTemplate) target);
        }

        EditorGUILayout.HelpBox(Strings.Hint_Exporting_Templates, MessageType.None );
    }
}
