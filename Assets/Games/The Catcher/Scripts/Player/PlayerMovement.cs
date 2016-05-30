using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float m_PitchRange = 0.1f;
    public float m_DistanceToTurn = 2.0f;
    public float m_WalkingSpeed = 0.1f; // Min velocity for starting "is walking" animation
    public float m_SpeedMaxRot = 1.0f;

    public int m_nSmoothing = 15;
    private float[] m_AvgSpeed;
    private int m_iSmoothing = -5;

    private AudioSource m_MovementAudio;
    private float m_OriginalPitch;
    private Vector3 m_TargetPosition = Vector3.zero;
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private Vector3 m_Movement;
    private Vector3 m_LastMovement;
    private Vector3 m_OriginalMovement;
    private float m_LastTime; // Added for calculating speed
    [SerializeField] private float m_Speed = 0.0f; // Caution: Now this variable can have a negative value
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
        m_OriginalPitch = m_MovementAudio.pitch;
        m_Movement = m_Transform.position;
        m_OriginalMovement = m_Transform.position;
        m_LastMovement = m_Movement;
        m_LastTime = Time.time;
        m_AvgSpeed = new float[m_nSmoothing];
    }

    private void FixedUpdate()
    {
        CheckTurning();

        Move(m_Movement);

        Animating();

        FootStepAudio();
    }

    public void Move(Vector3 position)
    {
        m_Rigidbody.MovePosition(position);
    }

    private void CheckTurning()
    {
   /*     if (Mathf.Abs(m_Movement.x - m_TargetPosition.x) > m_DistanceToTurn)
        {
            if (m_Movement.x > m_LastMovement.x)
            {
                Turning(1);
            }
            else if (m_Movement.x < m_LastMovement.x)
            {
                Turning(-1);
            }
        }*/ 

        Turning(Mathf.Clamp(m_Speed / m_SpeedMaxRot, -1f, 1f));
    }

    private void Turning(float horizontal)
    {
        //Vector3 targetDirection = new Vector3(horizontal, 0.0f, 0.0f);
        Vector3 targetDirection = new Vector3(-Mathf.Sin(horizontal * Mathf.PI / 2), 0.0f, -Mathf.Cos(horizontal * Mathf.PI / 2));

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        m_Rigidbody.MoveRotation(targetRotation);
    }

    public void HorizontalMovement(float horizontal)
    {
        m_Movement.Set(horizontal, m_OriginalMovement.y, m_OriginalMovement.z);
    }

    private void Animating()
    {
        //float distance = Vector3.Distance(m_LastMovement, m_Movement);
        float distance = m_LastMovement.x - m_Movement.x;

        //m_Speed = Mathf.Clamp(distance > 0.001f ? m_Speed + 0.25f : m_Speed - 0.2f, 0, 1);
        if (m_iSmoothing < 0)
        {
            m_AvgSpeed[m_iSmoothing + 5] = distance / (Time.time - m_LastTime);
            m_Speed += m_AvgSpeed[m_iSmoothing + 5] / m_nSmoothing;
        }
        else
        {
            m_Speed -= m_AvgSpeed[m_iSmoothing] / m_nSmoothing;
            m_AvgSpeed[m_iSmoothing] = distance / (Time.time - m_LastTime);
            m_Speed += m_AvgSpeed[m_iSmoothing] / m_nSmoothing;
        }
        m_iSmoothing = (m_iSmoothing + 1) % (m_nSmoothing - 1);

        //m_Speed = distance / (Time.time - m_LastTime); // Calculating vector speed

        m_LastMovement = m_Movement;
        m_LastTime = Time.time;

        m_IsWalking = Mathf.Abs(m_Speed) > m_WalkingSpeed;

        m_Animator.SetBool("IsWalking", m_IsWalking);


    }

    public void LookAt(Vector3 position)
    {
        m_TargetPosition = position;
    }

    private void FootStepAudio()
    {
        if (m_IsWalking)
        {
            if (m_MovementAudio.isPlaying)
                return;

            m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
            m_MovementAudio.Play();
        }
        else
        {
            m_MovementAudio.Stop();
        }
    }
}