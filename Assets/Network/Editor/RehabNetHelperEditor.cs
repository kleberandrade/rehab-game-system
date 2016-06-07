using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RehabNetHelper))]
public class RehabNetHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RehabNetHelper script = (RehabNetHelper)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Helper"))
        { 
            script.Execute(script.m_StartPosition, script.m_EndPosition, script.m_Time);    
        }
    }
}
