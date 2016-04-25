using UnityEngine;
using System.Collections;

public class SpawnerPoint : MonoBehaviour
{
    public float m_TimeToSpawning = 0.5f;
    public Transform m_Target;

    private ObjectPooler m_ObjectPooler;
    private ParticleSystem m_Particles;
    private Transform m_Transform;
    private Score m_Score;
    private int m_NumberOfObjects;
    private WindWord m_WindWorld;
    private PlayerMovement m_PlayerMovement;

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
        m_PlayerMovement = FindObjectOfType<PlayerMovement>();
        m_PlayerMovement.LookAt(Vector3.zero);
        m_Particles.playOnAwake = false;
        m_Particles.loop = false;

        UpdatePosition();
    }

    public void Spawn()
    {
        if (!HasObjectToSpawner())
            return;

        StartCoroutine(Spawning());

        m_NumberOfObjects--;
    }

    public void Spawn(Task task)
    {
        if (!HasObjectToSpawner())
            return;

        StartCoroutine(Spawning(task));

        m_NumberOfObjects--;
    }

    private IEnumerator Spawning(Task task)
    {
        m_PlayerMovement.LookAt(Vector3.zero);
        UpdatePosition();
        m_Particles.Play();
        m_WindWorld.Fan();

        yield return new WaitForSeconds(m_TimeToSpawning);

        // Define a distancia e velocidade da tarefa
        GameObject go = m_ObjectPooler.NextObject();
        go.transform.position = m_Transform.position;
        go.SetActive(true);

        m_Score.Next();
        m_PlayerMovement.LookAt(m_Transform.position);
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
        return m_NumberOfObjects > 0;
    }
}
