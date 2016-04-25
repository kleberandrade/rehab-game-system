using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Score))]
public class ScoreEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Score score = (Score) target;

        DrawDefaultInspector();

        if (GUILayout.Button("Score Up"))
        {
            score.Up();
        }

        if (GUILayout.Button("Goal Up"))
        {
            score.Next();
        }
    }
}
