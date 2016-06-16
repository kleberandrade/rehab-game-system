using UnityEngine;
using System.Collections;

public class AIPlayerMove : MonoBehaviour
{
    public float m_Speed = 80.0f;

    private RobotPlayerMovement m_Robot;
    private Transform m_Player;
    private MoveBox m_MoveBox;

    private void Awake()
    {
        m_Player = GetComponent<Transform>();
        m_Robot = GetComponent<RobotPlayerMovement>();
    }

    private void Start()
    {
        m_MoveBox = FindObjectOfType<MoveBox>();
    }

    private void Update()
    {
        if (m_MoveBox.m_State != MoveBoxState.None)
        {
            float horizontal = 0.0f;
            if (m_Player.position.x - m_MoveBox.m_Target.x < -0.2f)
                horizontal = 1;

            if (m_Player.position.x - m_MoveBox.m_Target.x > 0.2f)
                horizontal = -1;

            m_Robot.m_RobotAngle += horizontal * m_Speed * Time.deltaTime;
        }
    }
}
