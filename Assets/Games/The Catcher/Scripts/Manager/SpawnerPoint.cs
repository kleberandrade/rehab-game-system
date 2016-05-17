using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerPoint : MonoBehaviour
{
    public float m_TimeToSpawning = 0.5f;
    public Transform m_Target;
    public float m_ScreenLeft = 0.1f;
    public float m_ScreenRight = 0.9f;

    private ObjectPooler m_ObjectPooler;
    private ParticleSystem m_Particles;
    private Transform m_Transform;
    private Score m_Score;
    private int m_NumberOfObjects;
    private WindWord m_WindWorld;
    private PlayerMovement m_PlayerMovement;
    private DynamicDifficulty m_TaskManager;
    private List<Task> m_TasksToCalibration = new List<Task>();

    private Vector3 m_StartPlayerPosition;
    private Vector3 m_LastPlayerPosition;
    private Vector3 m_FinalTargetPosition;
    private float m_ElapsedTime = 0.0f;
    private float m_TotalTime = 0.0f;
    private bool m_IsPlaying = false;

    public void SetTaskManager(DynamicDifficulty taskManager)
    {
        m_TaskManager = taskManager;
    }

    private void Awake()
    {
        m_Score = FindObjectOfType<Score>();
        m_Transform = GetComponent<Transform>();
        m_ObjectPooler = GetComponent<ObjectPooler>();
        m_Particles = GetComponent<ParticleSystem>();
        m_WindWorld = GetComponent<WindWord>();
    }

    private void Start()
    {
        m_TasksToCalibration.Add(new Task(0.5f, 0.1f));
        m_TasksToCalibration.Add(new Task(0.0f, 0.1f));
        m_TasksToCalibration.Add(new Task(1.0f, 0.1f));
        m_TasksToCalibration.Add(new Task(0.0f, 0.1f));
        m_TasksToCalibration.Add(new Task(1.0f, 0.1f));
        m_TasksToCalibration.Add(new Task(0.5f, 0.1f));

        m_PlayerMovement = FindObjectOfType<PlayerMovement>();
        m_PlayerMovement.LookAt(Vector3.zero);
        m_Particles.playOnAwake = false;
        m_Particles.loop = false;

        UpdatePosition();
    }

    public void Spawn()
    {
        m_IsPlaying = false;

        if (!HasObjectToSpawner())
            return;

        if (m_TasksToCalibration.Count > 0)
            StartCoroutine(Spawning(null));
        else
            StartCoroutine(Spawning(m_TaskManager.NextTask()));

        m_NumberOfObjects--;
    }

    private IEnumerator Spawning(Task task)
    {
        m_PlayerMovement.LookAt(Vector3.zero);
        UpdatePosition();

        // Pega uma castanha do pool de objetos
        GameObject go = m_ObjectPooler.NextObject();
        Vector3 targetPosition = m_Transform.position;

        if (m_TasksToCalibration.Count > 0)
        {
            Debug.Log("[Target] Calibration in " + m_TasksToCalibration[0].ToString());
            targetPosition.x = Helper.ViewportToWord(m_TasksToCalibration[0].Distance, m_ScreenLeft, m_ScreenRight, Helper.CameraDepht(m_Transform.position));
            m_Transform.position = targetPosition;
            go.transform.position = m_Transform.position;

            Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Helper.CameraDepht(go.transform.position)));
            Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Helper.CameraDepht(go.transform.position)));
            float diff = Mathf.Abs(max.y - min.y);
            go.GetComponent<Nut>().Speed = (diff * 0.1f) + m_TasksToCalibration[0].Speed * (diff * 0.6f);
            m_TasksToCalibration.RemoveAt(0);
        }
        else
        {
            // Converte a posição do mundo objeto para a viewport
            targetPosition.x = Helper.WorldToViewport(m_Transform.position, m_ScreenLeft, m_ScreenRight);

            // Define a direção de lançamento
            float direction = Random.Range(0.0f, 1.0f) <= targetPosition.x ? -1.0f : 1.0f;
            float distance = Helper.InverseNormalization(task.Distance, 0.1f, 0.5f);
            if (targetPosition.x + distance * direction < 0.0f || targetPosition.x + direction * distance > 1.0f)
                direction *= -1.0f;

            // Define a posição final do alvo
            //targetPosition.x = Helper.ViewportToWord(targetPosition.x + direction * task.Distance, m_ScreenLeft, m_ScreenRight, Helper.CameraDepht(m_Transform.position));
            targetPosition.x += distance * direction;
            targetPosition.x = Helper.ViewportToWord(targetPosition.x, m_ScreenLeft, m_ScreenRight, Helper.CameraDepht(m_Transform.position));
            m_Transform.position = targetPosition;

            go.transform.position = m_Transform.position;
            Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Helper.CameraDepht(go.transform.position)));
            Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Helper.CameraDepht(go.transform.position)));
            float diff = Mathf.Abs(max.y - min.y);
            go.GetComponent<Nut>().Speed = (diff * 0.1f) + task.Speed * (diff * 0.6f);
        }

        m_Particles.Play();
        m_WindWorld.Fan();

        m_ElapsedTime = 0.0f;
        m_TotalTime = 0.0f;
        m_IsPlaying = true;
        yield return new WaitForSeconds(m_TimeToSpawning);

        go.SetActive(true);

        m_Score.Next();
        m_PlayerMovement.LookAt(m_Transform.position);

        // Guarda a posição inicial do jogador
        m_StartPlayerPosition = m_PlayerMovement.transform.position;
        // Guarda a posição final do alvo
        m_FinalTargetPosition = m_Transform.position;
        m_FinalTargetPosition.z = m_StartPlayerPosition.z;
        m_FinalTargetPosition.y = m_StartPlayerPosition.y;
    }

    private IEnumerator Spawning()
    {
        m_PlayerMovement.LookAt(Vector3.zero);
        UpdatePosition();
        m_Particles.Play();
        m_WindWorld.Fan();

        yield return new WaitForSeconds(m_TimeToSpawning);

        GameObject go = m_ObjectPooler.NextObject();
        go.transform.position = m_Transform.position;
        go.SetActive(true);

        m_Score.Next();
        m_PlayerMovement.LookAt(m_Transform.position);
    }

    private void UpdatePosition()
    {
        Vector3 position = m_Target.position;
        position.y = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Mathf.Abs(Camera.main.transform.position.z - m_Transform.position.z))).y;
        position.y += 2.0f;
        m_Transform.position = position;
    }

    public void SetNumberOfObjects(int numberOfObjects)
    {
        m_NumberOfObjects = numberOfObjects;
    }

    public bool HasObjectToSpawner()
    {
        return m_NumberOfObjects >= 0;
    }

    public void Update()
    {
        if (Vector3.Distance(m_StartPlayerPosition, m_FinalTargetPosition) > 0.1f)
        {
            if (Vector3.Distance(m_StartPlayerPosition, m_LastPlayerPosition) > 0.1f)
            {
                m_ElapsedTime += Time.deltaTime;
            }
        }

        m_TotalTime += Time.deltaTime;
        m_LastPlayerPosition = m_PlayerMovement.transform.position;
    }

    public void TargetCaptured()
    {
        m_TaskManager.Evaluation(0.0f, m_ElapsedTime/m_TotalTime);
        m_TotalTime = 0.0f;
    }

    public void TargetFail()
    {
        //       Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, m_PlayerMovement.transform.position.z));
        //       Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, m_PlayerMovement.transform.position.z));
        //       float diff = Mathf.Abs(max.x - min.x);

        //       m_TaskManager.Evaluation(Mathf.Abs(m_PlayerMovement.transform.position.x - m_FinalTargetPosition.x) / diff, 0.0f);
        m_TaskManager.Evaluation(Mathf.Abs(m_PlayerMovement.transform.position.x - m_FinalTargetPosition.x), 0.0f);
        m_TotalTime = 0.0f;
    }
}
