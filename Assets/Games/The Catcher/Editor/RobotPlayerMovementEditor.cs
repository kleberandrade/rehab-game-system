using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RobotPlayerMovement))]
public class RobotPlayerMovementEditor : Editor
{
    private float m_InputAngle = 0.0f;

    public override void OnInspectorGUI()
    {
        RobotPlayerMovement m_Target = (RobotPlayerMovement)target;

        DrawDefaultInspector();

        if (!m_Target.Debug)
            return;

        m_InputAngle = GUILayout.HorizontalSlider(m_InputAngle, m_Target.LeftAngle, m_Target.RightAngle);
        m_Target.RobotAngle = m_InputAngle;
    }
}
