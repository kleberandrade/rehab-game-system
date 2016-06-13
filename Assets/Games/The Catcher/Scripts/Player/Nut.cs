using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Nut : MonoBehaviour 
{
    public AudioClip m_GroundAudioClip;
    public AudioClip m_CapturedAudioClip;
    public float m_Speed = 0.5f;

    private Animator m_Animator;
    private AudioSource m_AudioSource;
    private Collider m_Collider;
    private Rigidbody m_Rigidbody;
    private Transform m_Transform;
    private Quaternion m_OriginalRotate;
    private GameManager m_GameManager;
    private Vector3 m_Size;
    private Score m_Score;
    private TrailRenderer m_Trail;

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
        m_Trail = GetComponentInChildren<TrailRenderer>();
    }

    private void Start()
    {
        m_Score = FindObjectOfType<Score>();
        m_GameManager = FindObjectOfType<GameManager>();
        m_OriginalRotate = m_Transform.rotation;
        m_Size = m_Collider.bounds.size.y * m_Transform.lossyScale;
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
        {
            m_Rigidbody.velocity = Vector3.down * m_Speed;
            m_Rigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_Trail.Reset(this);

            m_Collider.enabled = false;
            m_IsFalling = false;

            m_Transform.rotation = m_OriginalRotate;
            m_AudioSource.clip = m_GroundAudioClip;
            m_AudioSource.Play();
            
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            m_Animator.SetBool("InGround", true);

            StartCoroutine(Destroy());
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1.0f);
        m_Rigidbody.constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds(0.5f);
        m_GameManager.NextTarget(false);
        StartCoroutine(Disappear());
    }

    public void Captured()
    {
        m_AudioSource.clip = m_CapturedAudioClip;
        m_AudioSource.Play();

        m_Transform.position = m_Transform.position - Vector3.up * 1.0f;
        m_Transform.rotation = m_OriginalRotate;
        m_IsFalling = false;
        m_Collider.enabled = false;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        m_GameManager.NextTarget(true);
        StartCoroutine(NavigationToScore());
    }

    private IEnumerator Disappear()
    {
        m_Trail.Reset(this);
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
    }

    public Vector3 Size
    {
        get { return m_Size; }
    }

    private IEnumerator NavigationToScore()
    {
        float capturedTime = Time.time;
        Vector3 capturedPosition = m_Transform.position;
        Vector3 scorePosition = m_Score.WorldPoint(m_Transform.position);

        while (Vector3.Distance(transform.position, scorePosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(capturedPosition, scorePosition, (Time.time - capturedTime) / 0.3f);
            yield return null;
        }

        StartCoroutine(Disappear());
    }
}
