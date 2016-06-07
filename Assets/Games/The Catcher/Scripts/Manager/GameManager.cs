﻿using System;
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
    private PlayerMovement m_PlayerMovement;
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

    private bool m_CapturedTarget;

    #region [ Setup ]
    private void Start()
    {
        m_Score = FindObjectOfType<Score>();        
        m_TaskManager = FindObjectOfType<DynamicDifficulty>();
        m_Gameover = FindObjectOfType<Gameover>();
        m_TextHint = FindObjectOfType<TextHint>();
        m_StatsManager = FindObjectOfType<StatsManager>();
        m_Player = FindObjectOfType<PlayerMovement>().transform;
        m_Robot = FindObjectOfType<RobotPlayerMovement>();
        m_PlayerMovement = FindObjectOfType<PlayerMovement>();
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
    #endregion

    #region [ Game Loop ]
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(Starting());
        yield return StartCoroutine(Calibrating());
        //SessionManager.Instance.NewSession();
        yield return StartCoroutine(Playing());
        yield return StartCoroutine(Ending());
    }
    #endregion

    #region [ Starting ]
    private IEnumerator Starting()
    {
        m_Score.SetNumberOfTargets(m_NumberOfTargets);
        m_StartTime = Time.time;
        m_TextHint.Pulse(m_StartMessage, m_StartDelay - 2);
        yield return m_StartWait;
    }
    #endregion

    #region [ Calibrating ]
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
    #endregion

    private IEnumerator Playing()
    {
        m_TextHint.Pulse(m_PlayMessage, 5);
        m_TaskManager.InitializeRandomTasks();
        do
        {
            Task task = m_TaskManager.GetTask();
            m_Spawner.ViewportRelativeSpawn(task, m_Player.position);
            m_TargetInGame = true;

            float time = 0.0f;
            while (m_TargetInGame)
            {
                if (Mathf.Abs(m_PlayerMovement.Speed) <= 1.0f)
                    time += Time.deltaTime;

                yield return null;
            }

            float error;
            if (m_CapturedTarget)
            {
                error = 0.0f;
                time = time / m_Spawner.TimeToFall;
            }
            else
            {
                error = Mathf.Abs(m_Player.position.x - m_Spawner.CurrentTargetPosition.x);
                time = 0.0f;
            }

            Debug.Log(string.Format("Error {0} and Time {1}", error, time));
            m_TaskManager.EvaluationFitness(error, time);

        } while (!IsFinish());
    }

    #region [ Ending ]
    private IEnumerator Ending()
    {
        m_EndTime = Time.time;

        float score = 100.0f * m_Score.Point / m_Score.Targets;
        m_StatsManager.SetScore(string.Format("{0:0.00}", score), ArrowType.Up);

        float time = m_EndTime - m_StartTime;
        m_StatsManager.SetTime(string.Format("{0:0}", time), ArrowType.Up);

        float difficulty = m_TaskManager.Difficulty(m_NumberOfTargets);
        m_StatsManager.SetDifficulty(string.Format("{0:0.00}", difficulty), ArrowType.Up);

        float flow = difficulty / score * 100.0f;
        m_StatsManager.SetSkill(string.Format("{0:0.00}", flow), ArrowType.Up);
        
        //m_StatsManager.SetRobotInit(m_TaskManager.RobotInit, ArrowType.Up);
        m_Gameover.Show();
        SessionManager.Instance.SaveSession();
        yield return m_EndWait;
    }
    #endregion

    private bool IsFinish()
    {
        return m_CurrentTarget >= m_NumberOfTargets;
    }

    public void NextTarget(bool captured)
    {
        m_CapturedTarget = captured;
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