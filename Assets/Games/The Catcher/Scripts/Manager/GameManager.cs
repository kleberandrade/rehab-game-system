using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameParameters Parameters;

    public float m_StartDelay = 10f;
    public float m_EndDelay = 3f;
    public float m_Padding = 0.1f;
    public string m_StartMessage = string.Empty;
    public string[] m_CalibrateMessages;
    public string m_PlayMessage = string.Empty;
    public Task[] m_CalibrateTasks;

    private GameState m_GameState = GameState.Calibrating;
    private Score m_Score;
    private Spawner m_Spawner;
    private Gameover m_Gameover;
    private Transform m_Player;
    private RobotPlayerMovement m_Robot;
    private DynamicDifficulty m_TaskManager;
    private TextHint m_TextHint;
    private StatsManager m_StatsManager;
    private bool m_TargetInGame = false;

    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;

    private int m_NumberOfTargets;
    private int m_CurrentTarget = 0;
    private float m_StartTime;
    private float m_EndTime;

    public bool m_DebugMode = false;
    public int m_DebugNumberOfTargets = 25;

    private void Start()
    {
        m_Score = FindObjectOfType<Score>();        
        m_TaskManager = FindObjectOfType<DynamicDifficulty>();
        m_Gameover = FindObjectOfType<Gameover>();
        m_TextHint = FindObjectOfType<TextHint>();
        m_StatsManager = FindObjectOfType<StatsManager>();
        m_Player = FindObjectOfType<PlayerMovement>().transform;
        m_Robot = FindObjectOfType<RobotPlayerMovement>();
        m_Spawner = FindObjectOfType<Spawner>();

        if (m_DebugMode || !PlayerPrefs.HasKey("NumberOfGoal"))
            m_NumberOfTargets = m_DebugNumberOfTargets;
        else
            m_NumberOfTargets = PlayerPrefs.GetInt("NumberOfGoal");

        Debug.Log(string.Format("Game with {0} targets.", m_NumberOfTargets));

        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SetupGlobalParameters();

        StartCoroutine(GameLoop());
    }

    public void SetupGlobalParameters()
    {
        Parameters = new GameParameters();
        Parameters.LeftScreen = m_Padding;
        Parameters.RightScreen = 1.0f - m_Padding;
        Parameters.DepthScreen = Helper.CameraDepht(m_Player.position);
        Parameters.MaxDistance = 0.5f;
        Parameters.MinDistance = 0.1f;
        Parameters.MaxSpeed = 1.0f;
        Parameters.MinSpeed = 0.1f;

        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Parameters.DepthScreen));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Parameters.DepthScreen));

        Parameters.Bottom = min.y;
        Parameters.Left = min.x;
        Parameters.Top = max.y;
        Parameters.Right = min.x;
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(Starting());
        yield return StartCoroutine(Calibrating());
        //SessionManager.Instance.NewSession();
        yield return StartCoroutine(Playing());
        yield return StartCoroutine(Ending());
    }

    private IEnumerator Starting()
    {
        m_Score.SetNumberOfTargets(m_NumberOfTargets);
        m_StartTime = Time.time;
        m_TextHint.Pulse(m_StartMessage, m_StartDelay - 2);
        yield return m_StartWait;
    }

    private IEnumerator Calibrating()
    {
        m_Robot.State = PlayerState.Calibration;

        for (int i = 0; i < m_CalibrateTasks.Length; i++)
        {
            float height = Mathf.Abs(GameManager.Parameters.Top - GameManager.Parameters.Bottom);
            float speed = (height * GameManager.Parameters.MinSpeed) + m_CalibrateTasks[i].Speed * (height * GameManager.Parameters.MaxSpeed);
            m_TextHint.Pulse(m_CalibrateMessages[i], height / speed * 0.6f);
            m_Spawner.ViewportAbsoluteSpawn(m_CalibrateTasks[i]);
            m_TargetInGame = true;

            while (m_TargetInGame)
                yield return null;
        }

        m_Robot.State = PlayerState.Playing;
        m_GameState = GameState.Playing;
    }

    private IEnumerator Playing()
    {
        m_TextHint.Pulse(m_PlayMessage, 5);
        m_TaskManager.InitializeRandomTasks();
        do
        {
            Task task = m_TaskManager.GetTask();
            m_Spawner.ViewportRelativeSpawn(task, m_Player.position);
            m_TargetInGame = true;

            while (m_TargetInGame)
                yield return null;

            m_TaskManager.EvaluationFitness(0.0f, 0.0f);

        } while (!IsFinish());
    }

    private IEnumerator Ending()
    {
        m_EndTime = Time.time;
        m_StatsManager.SetScore(100.0f * m_Score.Point / m_Score.Targets, ArrowType.Up);
        m_StatsManager.SetTime(Mathf.RoundToInt(m_EndTime - m_StartTime), ArrowType.Up);
        //m_StatsManager.SetSkill(string.Format("{0:0.0}",m_TaskManager.Skill), ArrowType.Up);
        //m_StatsManager.SetDifficulty(string.Format("{0:0.0}", m_TaskManager.Difficulty), ArrowType.Up);
        //m_StatsManager.SetRobotInit(m_TaskManager.RobotInit, ArrowType.Up);

        m_Gameover.Show();
        //SessionManager.Instance.SaveSession();

        yield return m_EndWait;
    }

    private bool IsFinish()
    {
        return m_CurrentTarget >= m_NumberOfTargets;
    }

    public void NextTarget(bool captured)
    {
        m_TargetInGame = false;

        if (m_GameState == GameState.Calibrating)
            return;

        m_Score.NextTarget();
        if (captured)
            m_Score.NextPoint();

        m_CurrentTarget++;

        if (IsFinish())
            return;
    }

    public bool IsCalibrating()
    {
        return true;
    }
}

public enum GameplayState
{
    Calibrating,
    Playing
}

[Serializable]
public struct GameParameters
{
    public float LeftScreen;
    public float RightScreen;
    public float DepthScreen;

    public float Left;
    public float Right;
    public float Top;
    public float Bottom;

    public float MinSpeed;
    public float MaxSpeed;

    public float MinDistance;
    public float MaxDistance;
}

public enum GameState
{
    Calibrating,
    Playing
}