using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int m_NumberOfGoals = 300;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    public StartCounter m_Counter;
    public SpawnerPoint m_Spawner;
    public DynamicDifficulty m_TaskManager;
    public Transform m_PlayerTransform;
    public Score m_Score;
    public Gameover m_Gameover;

    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;

    private void Start()
    {
        if (PlayerPrefs.HasKey("NumberOfGoal"))
            m_NumberOfGoals = PlayerPrefs.GetInt("NumberOfGoal");

        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());
    }

    private IEnumerator RoundStarting()
    {
        m_Spawner.SetNumberOfObjects(m_NumberOfGoals);
        m_Score.Reset(m_NumberOfGoals);
        m_Counter.BeginCounting();
        yield return m_StartWait;
    }

    private IEnumerator RoundPlaying()
    {
        m_Spawner.SetTaskManager(m_TaskManager);
        m_Spawner.Spawn();
        while (m_Spawner.HasObjectToSpawner())
        {
            SessionManager.Instance.AddPerformance("Position", RehabNetManager.Instance.Connection.RobotPackage.Position);
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        Debug.Log("Terminando o robô...");
        m_Gameover.Show(2.0f);
        yield return m_EndWait;
    }
}
