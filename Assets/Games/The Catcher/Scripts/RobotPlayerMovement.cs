using UnityEngine;
using System.Collections;

public class RobotPlayerMovement : MonoBehaviour
{
    private Camera m_Camera;
    private float m_PlayerDepth;
    private Transform m_Transform;
    private PlayerMovement m_PlayerMovement;

    [SerializeField] private float m_MinRobotAngle = -90.0f;
    [SerializeField] private float m_MaxRobotAngle = 90.0f;
    [SerializeField] private float m_ScreenLeft = 0.1f;
    [SerializeField] private float m_ScreenRight = 0.9f;
    [SerializeField] private bool m_Debug = false;

    private double m_RobotAngle = 0.0f;

    public double RobotAngle
    {
        get { return m_RobotAngle; }
        set { m_RobotAngle = value; }
    }

    public bool Debug
    {
        get { return m_Debug; }
        set { m_Debug = value; }
    }

    public float LeftAngle
    {
        get { return m_MinRobotAngle; }
        set { m_MinRobotAngle = value; }
    }

    public float RightAngle
    {
        get { return m_MaxRobotAngle; }
        set { m_MaxRobotAngle = value; }
    }

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
        float horizontal = RobotAngleToHorizontalWorldPosition(m_RobotAngle, m_MinRobotAngle, m_MaxRobotAngle);
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
