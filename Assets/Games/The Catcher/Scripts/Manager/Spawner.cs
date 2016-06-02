using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float m_TimeToSpawning = 0.5f;
    private ObjectPooler m_ObjectPooler;
    private ParticleSystem m_Particles;
    private Transform m_Transform;
    private WindWord m_WindWorld;
    private float m_Height;

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_ObjectPooler = GetComponent<ObjectPooler>();
        m_Particles = GetComponent<ParticleSystem>();
        m_WindWorld = GetComponent<WindWord>();
    }

	private void Start ()
    {
        m_Particles.playOnAwake = false;
        m_Particles.loop = false;

        Vector3 myPosition = m_Transform.position;
        myPosition.y = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, GameManager.Parameters.DepthScreen)).y + 1.0f;
        m_Transform.position = myPosition;

        m_Height = Mathf.Abs(GameManager.Parameters.Top - GameManager.Parameters.Bottom);
    }

    public void Spawn()
    {
        StartCoroutine(SpawningAtPosition());
    }

    public void Spawn(Vector3 position)
    {
        m_Transform.position = position;
        StartCoroutine(SpawningAtPosition());
    }

    private IEnumerator SpawningAtPosition()
    {
        m_Particles.Play();
        m_WindWorld.Fan();
        yield return new WaitForSeconds(m_TimeToSpawning);
        GameObject go = m_ObjectPooler.NextObject();
        go.transform.position = m_Transform.position;
        go.SetActive(true);
    }

    private IEnumerator SpawningAtPosition(Vector3 position, float speed)
    {
        m_Transform.position = position;
        m_Particles.Play();
        m_WindWorld.Fan();
        yield return new WaitForSeconds(m_TimeToSpawning);
        GameObject go = m_ObjectPooler.NextObject();
        go.transform.position = position;
        go.GetComponent<Nut>().Speed = speed;
        go.SetActive(true);
    }

    public void ViewportAbsoluteSpawn(Task task)
    {
        Vector3 myPosition = m_Transform.position;
        myPosition.x = Helper.ViewportToWord(task.Distance,
            GameManager.Parameters.LeftScreen,
            GameManager.Parameters.RightScreen,
            GameManager.Parameters.DepthScreen);

        float speed = (m_Height * GameManager.Parameters.MinSpeed) + task.Speed * (m_Height * GameManager.Parameters.MaxSpeed);
        StartCoroutine(SpawningAtPosition(myPosition, speed));
    }

    public void ViewportRelativeSpawn(Task task, Vector3 playerPosition)
    {
        Vector3 myPosition = m_Transform.position;

        // Converte a posição do personagem (mundo) para a viewport
        Vector3 targetPosition = playerPosition;
        targetPosition.x = Helper.WorldToViewport(targetPosition, 
            GameManager.Parameters.LeftScreen,
            GameManager.Parameters.RightScreen);

        // Define a direção de lançamento
        float direction = Random.Range(0.0f, 1.0f) <= targetPosition.x ? -1.0f : 1.0f;
        float distance = Helper.InverseNormalization(task.Distance, 
            GameManager.Parameters.MinDistance,
            GameManager.Parameters.MaxDistance);
        if (targetPosition.x + distance * direction < 0.0f || targetPosition.x + direction * distance > 1.0f)
            direction *= -1.0f;

        // Define a posição final do alvo
        targetPosition.x += distance * direction;
        myPosition.x = Helper.ViewportToWord(targetPosition.x,
            GameManager.Parameters.LeftScreen,
            GameManager.Parameters.RightScreen,
            GameManager.Parameters.DepthScreen);

        // Define a velocidade do alvo
        float speed = (m_Height * GameManager.Parameters.MinSpeed) + task.Speed * (m_Height * GameManager.Parameters.MaxSpeed);

        // Lança o alvo
        StartCoroutine(SpawningAtPosition(myPosition, speed));
    }

}


