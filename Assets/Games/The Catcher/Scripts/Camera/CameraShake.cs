using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float m_ShakeAmount = 0.1f;
    [SerializeField] private float m_DecreaseFactor = 3.0f;

    private Vector3 m_Origin;
    private Transform m_Transform;
    private float m_Shake = 0f;

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
    }

    void OnEnable()
    {
        m_Origin = m_Transform.localPosition;
    }

    private void Update()
    {
        if (m_Shake > 0)
        {
            m_Transform.localPosition = m_Origin + Random.insideUnitSphere * m_ShakeAmount;
            m_Shake -= Time.deltaTime * m_DecreaseFactor;
        }
        else
        {
            m_Shake = 0f;
            m_Transform.localPosition = m_Origin;
//            m_Transform.localPosition = m_Origin + Random.insideUnitSphere * 0.01f;
        }
    }

    public void Shake()
    {
        m_Shake = 1.0f;
    }
}
