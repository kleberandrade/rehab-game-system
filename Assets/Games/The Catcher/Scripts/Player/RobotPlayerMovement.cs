using UnityEngine;
using UnityEngine.Events;

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
    private bool m_Pause = false;
    private bool m_Error = false;

    private void Start ()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
	}

    private void Update()
    {
        if (RehabNetManager.Instance.Connection.IsConnected)
        {
            if (RehabNetManager.Instance.Connection.RobotPackage.Status == (int)RehabNetRobotStatus.Error && !m_Error)
            {
                Time.timeScale = 0.0f;

                SystemDialogBox.Instance.Show(
                    "Erro",
                    "O robô apresenta erro. Deseja encerrar o jogo?",
                    new string[] { "Encerrar" },
                    new UnityAction[] { new UnityAction(Close) });

                m_Error = true;
            }

            if (RehabNetManager.Instance.Connection.RobotPackage.Status == (int)RehabNetRobotStatus.Running && m_Error)
            {
                m_Error = false;
                Time.timeScale = 1.0f;
                SystemDialogBox.Instance.Hide();
            }
        }

        if (m_Error)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_Pause = !m_Pause;

            if (m_Pause)
            {
                Time.timeScale = 1.0f - Time.timeScale;

                SystemDialogBox.Instance.Show(
                    "Pause",
                    "Deseja continuar?",
                    new string[] { "Continuar", "Fechar" },
                    new UnityAction[] { new UnityAction(Continue), new UnityAction(Close) }
                );
            }
            else
            {
                Continue();
            }
        }
    }


    private void Continue()
    {
        m_Pause = false;
        Time.timeScale = 1.0f;
        SystemDialogBox.Instance.Hide();
    }

    private void Close()
    {
        m_Pause = false;
        Time.timeScale = 1.0f;
        SystemDialogBox.Instance.Hide();
        LoadingScreenManager.LoadScene(3);
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