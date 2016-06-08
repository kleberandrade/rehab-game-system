using System.Collections;
using UnityEngine;

public class RehabNetManager : Singleton<RehabNetManager>
{
    private RehabNetConnection m_Connection;

    public RehabNetConnection Connection
    {
        get { return m_Connection; }
    }

    private float m_RepeatRate = 0.002f;

    private void Start()
    {
        m_Connection = GameObject.Find("RehabNetManager").GetComponent<RehabNetConnection>();
        m_Connection.Connect();
        InvokeRepeating("SendToNetwork", 1.0f, m_RepeatRate); // Delay necessary for communication. Coroutine + Delay did't work.
    }

    void SendToNetwork()
    {

        if (m_Connection.IsConnected)
            m_Connection.Send(m_Connection.GamePackage);
    }

    private void OnApplicationQuit()
    {
        CancelInvoke("SendToNetwork");
        m_Connection.Close();
    }
}
