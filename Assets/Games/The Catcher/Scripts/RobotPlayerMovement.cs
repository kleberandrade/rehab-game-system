using UnityEngine;
using System.Collections;

public class RobotPlayerMovement : MonoBehaviour
{
    private float m_PlayerDepth;
    private PlayerMovement m_PlayerMovement;

    [Range(0.0f, -90.0f)]
    public float m_MinRobotAngle = -90.0f;
    [Range(0.0f, 90.0f)]
    public float m_MaxRobotAngle = 90.0f;
    [Range(0.0f, -90.0f)]
    public float m_MinPlayerAngle = -90.0f;
    [Range(0.0f, 90.0f)]
    public float m_MaxPlayerAngle = 90.0f;
    [Range(0.0f, 1.0f)]
    public float m_ScreenLeft = 0.1f;
    [Range(0.0f, 1.0f)]
    public float m_ScreenRight = 0.9f;
    public bool m_Debug = false;
    [Range(-90.0f, 90.0f)]
    public double m_RobotAngle = 0.0f;

    private void Start ()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerDepth = Helper.CameraDepht(transform.position);
	}

    private void FixedUpdate()
    {
        m_RobotAngle = RehabNetManager.Instance.Connection.RobotPackage.Position;

        if (m_RobotAngle < m_MinPlayerAngle)
            m_MinPlayerAngle = (float)m_RobotAngle;

        if (m_RobotAngle > m_MaxPlayerAngle)
            m_MaxPlayerAngle = (float)m_RobotAngle;

        float horizontal = Helper.Normalization((float)m_RobotAngle, m_MinPlayerAngle, m_MaxPlayerAngle);
        horizontal = Helper.ViewportToWord(horizontal, m_ScreenLeft, m_ScreenRight, m_PlayerDepth);

        m_PlayerMovement.HorizontalMovement(horizontal);
	}
}
