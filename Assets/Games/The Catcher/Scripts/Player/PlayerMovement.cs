using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float m_PitchRange = 0.1f;
    public float m_DistanceToTurn = 2.0f;
    public float m_WalkingSpeed = 0.3f; // Min velocity for starting "is walking" animation
    public float m_SpeedRotation = 8.0f;
    public float m_Smoothing = 15.0f;
    private List<float> m_Speeds = new List<float>();

    private AudioSource m_MovementAudio;
    private float m_OriginalPitch;
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private Vector3 m_Movement;
    private Vector3 m_LastMovement;
    private Vector3 m_OriginalMovement;
    private float m_Speed = 0.0f; // Caution: Now this variable can have a negative value
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
        m_OriginalMovement = m_Transform.position;
        m_Movement = m_Transform.position;
        m_LastMovement = m_Movement;
    }

    private void FixedUpdate()
    {
        Move(m_Movement);

        CheckTurning();

        Animating();

        FootStepAudio();
    }

    public void Move(Vector3 position)
    {
        m_Rigidbody.MovePosition(position);
    }

    private void CheckTurning()
    {
        Turning(Mathf.Clamp(m_Speed / m_SpeedRotation, -1f, 1f));
    }

    private void Turning(float horizontal)
    {
        Vector3 targetDirection = new Vector3(-Mathf.Sin(horizontal * Mathf.PI / 2.0f), 0.0f, -Mathf.Cos(horizontal * Mathf.PI / 2.0f));
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(m_Transform.rotation, targetRotation, Time.deltaTime * m_Smoothing);
        m_Rigidbody.MoveRotation(newRotation);   
    }

    public void HorizontalMovement(float horizontal)
    {
        m_Movement.Set(horizontal, m_OriginalMovement.y, m_OriginalMovement.z);
    }

    private void Animating()
    {
        float distance = m_LastMovement.x - m_Movement.x;

        m_Speeds.Add(distance);
        if (m_Speeds.Count > 5)
            m_Speeds.RemoveAt(0);

        m_Speed = 0.0f;
        m_Speeds.ForEach(x => m_Speed += x);
        m_Speed /= (Time.deltaTime * 5.0f);

        m_LastMovement = m_Movement;
        m_IsWalking = Mathf.Abs(m_Speed) > m_WalkingSpeed;

        m_Animator.SetBool("IsWalking", m_IsWalking);
    }

    public float Speed
    {
        get { return m_Speed; }
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