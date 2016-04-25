using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float m_PitchRange = 0.1f;
    [SerializeField] private float m_DistanceToTurn = 2.0f;

    private AudioSource m_MovementAudio;
    private float m_OriginalPitch;
    private Vector3 m_TargetPosition = Vector3.zero;
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private Vector3 m_Movement;
    private Vector3 m_LastMovement;
    private float m_Speed = 0.0f;
    private bool m_IsWalking = false;

    private void Awake()
    {
        m_MovementAudio = GetComponent<AudioSource>();
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
    } 

    private void Start()
    {
        m_PitchRange = m_MovementAudio.pitch;
        m_Movement = m_Transform.position;
        m_LastMovement = m_Movement;
    }

    private void FixedUpdate()
    {
        CheckTurning();

        Move(m_Movement);

        Animating();

        m_LastMovement = m_Movement;

        FootStepAudio();
    }

    public void Move(Vector3 position)
    {
        m_Rigidbody.MovePosition(position);
    }

    private void CheckTurning()
    {
        if (Mathf.Abs(m_Movement.x - m_TargetPosition.x) > m_DistanceToTurn)
        { 
            if (m_Movement.x > m_LastMovement.x)
            {
                Turning(1);
            }
            else if (m_Movement.x < m_LastMovement.x)
            {
                Turning(-1);
            }
        }
    }

    private void Turning(float horizontal)
    {
        Vector3 targetDirection = new Vector3(horizontal, 0.0f, 0.0f);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        m_Rigidbody.MoveRotation(targetRotation);
    }

    public void HorizontalMovement(float horizontal)
    {
        m_Movement.Set(horizontal, m_Transform.position.y, m_Transform.position.z);
    }

    private void Animating()
    {
        float distance = Vector3.Distance(m_LastMovement, m_Movement);
        m_Speed = Mathf.Clamp(distance > 0.001f ? m_Speed + 0.25f : m_Speed - 0.2f, 0, 1);
        m_IsWalking = m_Speed > 0.1f;
        m_Animator.SetBool("IsWalking", m_IsWalking);
    }

    public void SetTargetPosition(Vector3 position)
    {
        m_TargetPosition = position;
    }

    private void FootStepAudio()
    {
        if (m_IsWalking)
        {
            m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
            m_MovementAudio.Play();
        }
        else
        {
            m_MovementAudio.Stop();
        }
    }
}