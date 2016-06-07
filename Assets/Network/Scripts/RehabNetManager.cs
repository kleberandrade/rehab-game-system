using System.Collections;
using UnityEngine;

public class RehabNetManager : Singleton<RehabNetManager>
{
    private RehabNetConnection m_Connection;

    public RehabNetConnection Connection
    {
        get { return m_Connection; }
    }

    public float m_RepeatRate = 0.002f;

    private void Start()
    {
        m_Connection = GameObject.Find("RehabNetManager").GetComponent<RehabNetConnection>();
        m_Connection.Connect();
        StartCoroutine(SendToNetwork());
    }

    private IEnumerator SendToNetwork()
    {
        yield return null;

        if (m_Connection.IsConnected)
            m_Connection.Send(m_Connection.GamePackage);
    }

    private void OnApplicationQuit()
    {
        StopCoroutine(SendToNetwork());
        m_Connection.Close();
    }
}
