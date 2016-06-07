using UnityEngine;
using System.Collections;

public class RehabNetHelper : MonoBehaviour
{
    public float m_StartPosition;
    public float m_EndPosition;
    public float m_Time;
    [Range(0.0f, 1.0f)]
    public float m_DelayRate = 0.1f;
    public float m_Delay;
    private float m_StartTime;
    public float m_Setpoint;
    public bool m_Enable = false;
    private Transform m_Transform;

    public float SetPoint
    {
        get { return m_Setpoint; }
    }

    public bool Enable
    {
        get { return m_Enable; }
    }

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
    }

    private void FixedUpdate ()
    {
        if (!m_Enable)
            return;

        float time = (Time.time - m_StartTime) / m_Time;
        m_Setpoint = Mathf.Lerp(m_StartPosition, m_EndPosition, time);
        m_Transform.position = new Vector3(m_Setpoint, m_Transform.position.y, m_Transform.position.z);
	}

    public void Execute(float start, float end, float time)
    {
        StopCoroutine("Executing");
        
        m_StartPosition = start;
        m_EndPosition = end;
        m_Time = time;
        m_Delay = time * m_DelayRate;

        StartCoroutine(Executing());
    }

    private IEnumerator Executing()
    {
        m_Enable = false;
        yield return new WaitForSeconds(m_Delay);
        m_StartTime = Time.time;
        m_Enable = true;
        yield return new WaitForSeconds(m_Time);
        m_Enable = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
