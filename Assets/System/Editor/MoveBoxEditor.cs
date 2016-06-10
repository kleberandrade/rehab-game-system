using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveBox))]
public class MoveBoxEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MoveBox script = (MoveBox)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Execute"))
        {
            script.Execute(script.m_Target, script.m_GoTime);
        }
    }
}
