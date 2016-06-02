using UnityEngine;

public class RobotPlayerMovement : MonoBehaviour
{
    private PlayerState m_State = PlayerState.None;
    private PlayerMovement m_PlayerMovement;

    [Range(-90.0f, 90.0f)]
    public float m_LeftPlayerAngle = -10.0f; // The start angle must be higher than patient's wrist min angle
    [Range(-90.0f, 90.0f)]
    public float m_RightPlayerAngle = 10.0f;  // The start angle must be lower than patient's wrist max angle
    [Range(-90.0f, 90.0f)]
    public double m_RobotAngle = 0.0f;
    private float m_Horizontal;

    private void Start ()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
	}

    private void FixedUpdate()
    {
        if (RehabNetManager.Instance.Connection.IsConnected)
        {
            m_RobotAngle = RehabNetManager.Instance.Connection.RobotPackage.Position;
            m_RobotAngle *= -1.0f;
        }

        if (m_State == PlayerState.Calibration || m_State == PlayerState.Playing)
        {
            if (m_RobotAngle < m_LeftPlayerAngle)
                m_LeftPlayerAngle = (float)m_RobotAngle;

            if (m_RobotAngle > m_RightPlayerAngle)
                m_RightPlayerAngle = (float)m_RobotAngle;
        }

        if (m_State == PlayerState.Playing)
            m_Horizontal = Helper.Normalization((float)m_RobotAngle, m_LeftPlayerAngle, m_RightPlayerAngle);
        else
            m_Horizontal = Helper.Normalization((float)m_RobotAngle, -90.0f, 90.0f);

        float h = m_Horizontal;

        m_Horizontal = Helper.ViewportToWord(m_Horizontal, 
            GameManager.Parameters.LeftScreen,
            GameManager.Parameters.RightScreen,
            GameManager.Parameters.DepthScreen);

        m_PlayerMovement.HorizontalMovement(m_Horizontal);
	}

    public PlayerState State
    {
        get { return m_State; }
        set { m_State = value; }
    }
}

public enum PlayerState
{
    None,
    Calibration,
    Playing
}