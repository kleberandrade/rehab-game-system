using UnityEngine;
using System.Collections;

public class MoveBox : MonoBehaviour
{
    public Vector3 m_Target;
    public Transform m_Player;

    public float m_GoTime;
    public float m_BackTime = 1.0f;
    [Range(0.0f, 1.0f)]
    public float m_DelayRate = 0.1f;
    public float m_Delay;
    
    private float m_StartTime;
    private MoveBoxState m_State = MoveBoxState.None;

    public float m_HPadding = 0.1f;
    public float m_VPadding = 0.0f;
    public float m_Depth;

    private Vector3 m_Min;
    private Vector3 m_Max;

    public float m_CurrentTop;
    public float m_CurrentLeft;
    public float m_CurrentRight;
    public float m_CurrentBottom;

    private Vector3 m_MinViewport;
    private Vector3 m_MaxViewport;

    private float m_RobotTop;
    private float m_RobotLeft;
    private float m_RobotRight;
    private float m_RobotBottom;

    private float m_PlayerLeft = -90;
    private float m_PlayerRight = 90;
    private float m_PlayerBottom = -90;
    private float m_PlayerTop = 90;

    private Vector3 m_MinScreen;
    private Vector3 m_MaxScreen;

    private float m_ElapsedTime;

    public float m_Stiffness, m_Damping;

    private void Start ()
    {
        if (m_Player == null)
            m_Player = FindObjectOfType<PlayerMovement>().transform;

        m_Depth = Mathf.Abs(Camera.main.transform.position.z - m_Player.position.z);

        m_Min = Camera.main.ViewportToWorldPoint(new Vector3(0 + m_HPadding, 0 + m_VPadding, m_Depth));
        m_Max = Camera.main.ViewportToWorldPoint(new Vector3(1 - m_HPadding, 1 - m_VPadding, m_Depth));
        m_MinScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, m_Depth));
        m_MaxScreen = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, m_Depth));

        RehabNetManager.Instance.Connection.GamePackage.Left = -90.0f;
        RehabNetManager.Instance.Connection.GamePackage.Right = 90.0f;
    }

    private void Update()
    {
#if UNITY_EDITOR
        DrawBox();
#endif

        if (m_State == MoveBoxState.None)
            return;

        if (m_State == MoveBoxState.Go)
        {
            m_ElapsedTime = (Time.time - m_StartTime) / m_GoTime;

            m_CurrentLeft = Mathf.Lerp(m_Min.x, m_Target.x, m_ElapsedTime);
            m_CurrentRight = Mathf.Lerp(m_Max.x, m_Target.x, m_ElapsedTime);
            m_CurrentBottom = Mathf.Lerp(m_Min.y, m_Target.y, m_ElapsedTime);
            m_CurrentTop = Mathf.Lerp(m_Max.y, m_Target.y, m_ElapsedTime);
        }
        else
        {
            m_ElapsedTime = (Time.time - m_StartTime) / m_BackTime;

            m_CurrentLeft = Mathf.Lerp(m_Target.x, m_Min.x, m_ElapsedTime);
            m_CurrentRight = Mathf.Lerp(m_Target.x, m_Max.x, m_ElapsedTime);
            m_CurrentBottom = Mathf.Lerp(m_Target.y, m_Min.y, m_ElapsedTime);
            m_CurrentTop = Mathf.Lerp(m_Target.y, m_Max.y, m_ElapsedTime);
        }

        m_MinViewport = Camera.main.WorldToViewportPoint(new Vector3(m_CurrentLeft, m_CurrentBottom, 0.0f));
        m_MaxViewport = Camera.main.WorldToViewportPoint(new Vector3(m_CurrentRight, m_CurrentTop, 0.0f));

        m_RobotLeft = (m_MinViewport.x - m_HPadding) / ((1.0f - m_HPadding) - m_HPadding);
        m_RobotLeft = m_PlayerLeft + (m_PlayerRight - m_PlayerLeft) * m_RobotLeft;

        m_RobotTop = (m_MaxViewport.y - m_VPadding) / ((1.0f - m_VPadding) - m_VPadding);
        m_RobotTop = m_PlayerBottom + (m_PlayerTop - m_PlayerBottom) * m_RobotTop;

        m_RobotRight = (m_MaxViewport.x - m_HPadding) / ((1.0f - m_HPadding) - m_HPadding);
        m_RobotRight = m_PlayerLeft + (m_PlayerRight - m_PlayerLeft) * m_RobotRight;

        m_RobotBottom = (m_MinViewport.y - m_VPadding) / ((1.0f - m_VPadding) - m_VPadding);
        m_RobotBottom = m_PlayerBottom + (m_PlayerTop - m_PlayerBottom) * m_RobotBottom;

        RehabNetManager.Instance.Connection.GamePackage.Setpoint = 0.0;
        RehabNetManager.Instance.Connection.GamePackage.Stiffness = m_Stiffness;
        RehabNetManager.Instance.Connection.GamePackage.Damping = m_Damping;
        RehabNetManager.Instance.Connection.GamePackage.Right = -m_RobotRight;
        RehabNetManager.Instance.Connection.GamePackage.Left = -m_RobotLeft;
        }

    public void Execute(Vector3 target, float time)
    {
        StopCoroutine("Executing");

        m_Target = target;
        m_Delay = time * m_DelayRate;
        m_GoTime = time - m_Delay;
        
        StartCoroutine(Executing());
    }

    private IEnumerator Executing()
    {
        yield return new WaitForSeconds(m_Delay);

        m_ElapsedTime = 0.0f;
        m_StartTime = Time.time;
        m_State = MoveBoxState.Go;
        yield return new WaitForSeconds(m_GoTime);

        yield return new WaitForSeconds(m_BackTime * 0.1f);

        m_ElapsedTime = 0.0f;
        m_StartTime = Time.time;
        m_State = MoveBoxState.Back;
        yield return new WaitForSeconds(m_BackTime * 0.9f);

        Reset();
    }

    private void Reset()
    {
        m_CurrentLeft = m_Min.x;
        m_CurrentRight = m_Max.x;
        m_CurrentBottom = m_Min.y;
        m_CurrentTop = m_Max.y;
        m_State = MoveBoxState.None;
    }

    private void DrawBox()
    {
        // Tela total
        Debug.DrawLine(new Vector3(m_MinScreen.x, m_MinScreen.y, m_Player.position.z), new Vector3(m_MinScreen.x, m_MaxScreen.y, m_Player.position.z), Color.yellow);
        Debug.DrawLine(new Vector3(m_MinScreen.x, m_MaxScreen.y, m_Player.position.z), new Vector3(m_MaxScreen.x, m_MaxScreen.y, m_Player.position.z), Color.yellow);
        Debug.DrawLine(new Vector3(m_MaxScreen.x, m_MaxScreen.y, m_Player.position.z), new Vector3(m_MaxScreen.x, m_MinScreen.y, m_Player.position.z), Color.yellow);
        Debug.DrawLine(new Vector3(m_MaxScreen.x, m_MinScreen.y, m_Player.position.z), new Vector3(m_MinScreen.x, m_MinScreen.y, m_Player.position.z), Color.yellow);

        // Range utilizado pelo jogo
        Debug.DrawLine(new Vector3(m_Min.x, m_Min.y, m_Player.position.z), new Vector3(m_Min.x, m_Max.y, m_Player.position.z), Color.red);
        Debug.DrawLine(new Vector3(m_Min.x, m_Max.y, m_Player.position.z), new Vector3(m_Max.x, m_Max.y, m_Player.position.z), Color.red);
        Debug.DrawLine(new Vector3(m_Max.x, m_Max.y, m_Player.position.z), new Vector3(m_Max.x, m_Min.y, m_Player.position.z), Color.red);
        Debug.DrawLine(new Vector3(m_Max.x, m_Min.y, m_Player.position.z), new Vector3(m_Min.x, m_Min.y, m_Player.position.z), Color.red);

        // janela de tempo
        Debug.DrawLine(new Vector3(m_CurrentLeft, m_CurrentBottom, m_Player.position.z), new Vector3(m_CurrentLeft, m_CurrentTop, m_Player.position.z), Color.blue);
        Debug.DrawLine(new Vector3(m_CurrentLeft, m_CurrentTop, m_Player.position.z), new Vector3(m_CurrentRight, m_CurrentTop, m_Player.position.z), Color.blue);
        Debug.DrawLine(new Vector3(m_CurrentRight, m_CurrentTop, m_Player.position.z), new Vector3(m_CurrentRight, m_CurrentBottom, m_Player.position.z), Color.blue);
        Debug.DrawLine(new Vector3(m_CurrentRight, m_CurrentBottom, m_Player.position.z), new Vector3(m_CurrentLeft, m_CurrentBottom, m_Player.position.z), Color.blue);
    }
}

public enum MoveBoxState
{
    None,
    Go,
    Back
}