using UnityEngine;
using System.Collections;

public class RobotPlayerMovement : MonoBehaviour
{
    private Camera m_Camera;
    private float m_PlayerDepth;
    private Transform m_Transform;
    private PlayerMovement m_PlayerMovement;

    [Range(0.0f, -90.0f)]
    public float m_MinRobotAngle = -90.0f;
    [Range(0.0f, 90.0f)]
    public float m_MaxRobotAngle = 90.0f;
    [Range(0.0f, -90.0f)]
    public float m_MinPlayerAngle = 0.0f;
    [Range(0.0f, 90.0f)]
    public float m_MaxPlayerAngle = 1.0f;
    [Range(0.0f, 1.0f)]
    public float m_ScreenLeft = 0.1f;
    [Range(0.0f, 1.0f)]
    public float m_ScreenRight = 0.9f;
    public bool m_Debug = false;
    [Range(-90.0f, 90.0f)]
    public double m_RobotAngle = 0.0f;

    private void Start ()
    {
        m_Transform = GetComponent<Transform>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_Camera = Camera.main;
        m_PlayerDepth = CalculatePlayerDepth();
	}

    private void FixedUpdate()
    {
        HorizontalMovement();
    }

	private void HorizontalMovement ()
    {
        if (m_RobotAngle < m_MinPlayerAngle)
            m_MinPlayerAngle = (float)m_RobotAngle;

        if (m_RobotAngle > m_MaxPlayerAngle)
            m_MaxPlayerAngle = (float)m_RobotAngle;

        float horizontal = RobotAngleToHorizontalWorldPosition(m_RobotAngle, m_MinPlayerAngle, m_MaxPlayerAngle);

        m_PlayerMovement.HorizontalMovement(horizontal);
	}

    private float CalculatePlayerDepth()
    {
        return Mathf.Abs(m_Camera.transform.position.z - m_Transform.position.z);
    }

    private float RobotAnglePositionNorm(double robotAngle, float min, float max)
    {
        return ((float)robotAngle - min) / (max - min);
    }

    private float RobotAngleToHorizontalWorldPosition(double robotAngle, float min, float max)
    {
        float robotPosition = RobotAnglePositionNorm(robotAngle, min, max);
        robotPosition = m_ScreenLeft + (m_ScreenRight - m_ScreenLeft) * robotPosition;
        return m_Camera.ViewportToWorldPoint(new Vector3(robotPosition, 0, m_PlayerDepth)).x;
    }
}
