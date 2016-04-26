using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Nut : MonoBehaviour 
{
    private static Score m_Score = null;
    private static SpawnerPoint m_Spawner = null;
    private static CameraShake m_CameraShake = null;

    [SerializeField] private AudioClip m_CollidedGroundAudioClip;
    [SerializeField] private AudioClip m_CollidedPlayerAudioClip;
    [SerializeField] private float m_Speed = 0.5f;
    [SerializeField] private float m_Time = 1.0f;

    private Animator m_Animator;
    private AudioSource m_AudioSource;
    private Collider m_Collider;
    private Rigidbody m_Rigidbody;
    private Transform m_Transform;
    private Quaternion m_OriginalRotate;

    private bool m_IsFalling = true;

    public float Speed
    {
        get { return m_Speed; }
        set { m_Speed = value; }
    }

    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Collider = GetComponent<Collider>();
        m_Transform = GetComponent<Transform>();
    }

    private void Start()
    {
        if (!m_Spawner)
            m_Spawner = FindObjectOfType<SpawnerPoint>();

        if (!m_Score)
            m_Score = FindObjectOfType<Score>();

        if (!m_CameraShake)
            m_CameraShake = FindObjectOfType<CameraShake>();

        m_OriginalRotate = m_Transform.rotation;
    }

    private void OnEnable()
    {
        m_IsFalling = true;
        m_Collider.enabled = true;
        m_Animator.SetBool("InGround", false);
        m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    private void FixedUpdate()
    {
        if (m_IsFalling)
            m_Rigidbody.velocity = Vector3.down * m_Speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_Transform.rotation = m_OriginalRotate;
            m_AudioSource.clip = m_CollidedGroundAudioClip;
            m_AudioSource.Play();
            m_IsFalling = false;
            m_Collider.enabled = false;
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            m_Animator.SetBool("InGround", true);
            //m_CameraShake.Shake();
            StartCoroutine(Destroy());
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2.0f);
        m_Spawner.Spawn();
        m_Rigidbody.constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds(1.0f);
        Disappear();
    }

    public void Captured()
    {
        m_Transform.rotation = m_OriginalRotate;
        m_IsFalling = false;
        m_Collider.enabled = false;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(Markingpoint());
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator Markingpoint()
    {
        float capturedTime = Time.time;
        Vector3 capturedPosition = m_Transform.position;
        Vector3 scorePosition = m_Score.WorldPoint(m_Transform.position);
        
        do
        {
            transform.position = Vector3.Lerp(capturedPosition, scorePosition, (Time.time - capturedTime) / m_Time);
            yield return null;
        } while (Vector3.Distance(transform.position, scorePosition) > 0.001f);

        m_Spawner.Spawn();
        m_Score.Up();
        Disappear();
    }
}
