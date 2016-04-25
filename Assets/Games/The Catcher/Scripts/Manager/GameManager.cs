using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int m_NumberOfGoals = 300;
    [SerializeField] private float m_StartDelay = 3f;
    [SerializeField] private float m_EndDelay = 3f;
    [SerializeField] private StartCounter m_Counter;
    [SerializeField] private SpawnerPoint m_Spawner;
    [SerializeField] private Score m_Score;

    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;

    private int m_GoalNumber = 0;

    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        //yield return StartCoroutine(RoundCalibrating());
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

    private IEnumerator RoundCalibrating()
    {
        Debug.Log("Calibrando robô...");
        //while ()
        //{
            yield return null;
        //}
    }

    private IEnumerator RoundPlaying()
    {
        m_Spawner.Spawn();

        while (m_Spawner.HasObjectToSpawner())
        {
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        Debug.Log("Terminando o robô...");
        DisablePlayer();
        yield return m_EndWait;
    }

    private void DisablePlayer()
    {

    }
}
