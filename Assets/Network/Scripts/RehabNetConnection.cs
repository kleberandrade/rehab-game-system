using UnityEngine;
using System.Net.Sockets;
using System;
using System.Net;

public class RehabNetConnection : MonoBehaviour
{
    #region [ Constants ]
    private const int k_BufferSize = 32;
    private const string k_ServerHostName = "192.168.1.2";
    private const int k_ServerPort = 3333;
    #endregion

    #region [ Variables ]
    private byte[] m_BufferRead = new byte[k_BufferSize];
    private Socket m_ClientSocket = null;
    private RehabNetGamePackage m_SendGamePackage = new RehabNetGamePackage();
    private RehabNetRobotPackage m_ReceiveRobotPackage = new RehabNetRobotPackage();

    private RehabNetState m_RehabNetState = RehabNetState.None;
    private RehabNetRobotStatus m_LastRehabNetRoboStatus;
    #endregion

    #region [ Properties ]
    public bool IsConnected
    {
        get { return m_ClientSocket != null && m_ClientSocket.Connected; }
    }

    public RehabNetGamePackage GamePackage
    {
        get { return m_SendGamePackage; }
        set { m_SendGamePackage = value; }
    }

    public RehabNetRobotPackage RobotPackage
    {
        get { return m_ReceiveRobotPackage; }
        set { m_ReceiveRobotPackage = value; }
    }

    #endregion

    #region [ Connect Method ]
    public void Connect()
    {
        m_RehabNetState = RehabNetState.Connecting;
        //Debug.Log(string.Format("Trying to connect in {0}:{1}", k_ServerHostName, k_ServerPort));

        if (IsConnected)
            return;

        try
        {
            m_ClientSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            m_ClientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(k_ServerHostName),
                k_ServerPort),
                new AsyncCallback(ConnectCallback),
                null);
        }
        catch
        {
            Close();
            if (m_RehabNetState != RehabNetState.Disconnecting)
                Connect();
        }
    }

    private void ConnectCallback(IAsyncResult asyncResult)
    {
        try
        {
            m_ClientSocket.EndConnect(asyncResult);
            //Debug.Log(string.Format("Connected to {0}", m_ClientSocket.RemoteEndPoint.ToString()));
            m_RehabNetState = RehabNetState.Playing;
            Send(m_SendGamePackage);
        }
        catch 
        {
            Close();
            if (m_RehabNetState != RehabNetState.Disconnecting)
                Connect();
        }
    }
    #endregion


    #region [ Send Method ]
    public void Send()
    {
        Send(m_SendGamePackage);
    }

    public void Send(IRehabNetPackage package)
    {
        try
        {
            byte[] buffer = package.Encode();
            m_ClientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
        }
        catch
        {
            Close();
            if (m_RehabNetState != RehabNetState.Disconnecting)
                Connect();
        }
    }

    private void SendCallback(IAsyncResult asyncResult)
    {
        try
        {
            m_ClientSocket.EndSend(asyncResult);
            Receive();
        }
        catch
        {
            Close();
            if (m_RehabNetState != RehabNetState.Disconnecting)
                Connect();
        }
    }
    #endregion

    #region [ Receive Method ]
    public void Receive()
    {
        try
        {
            m_ClientSocket.BeginReceive(m_BufferRead, 0, 12, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
        }
        catch
        { 
            Close();
            if (m_RehabNetState != RehabNetState.Disconnecting)
                Connect();
        }
    }

    private void ReceiveCallback(IAsyncResult asyncResult)
    {
        try
        {
            int received = m_ClientSocket.EndReceive(asyncResult);

            if (received == 0)
            {
                Connect();
                return;
            }

            m_ReceiveRobotPackage.Decode(m_BufferRead);

            if (m_LastRehabNetRoboStatus == RehabNetRobotStatus.Homing && (RehabNetRobotStatus)m_ReceiveRobotPackage.Status == RehabNetRobotStatus.Running)
            {
                m_SendGamePackage.Control = (int)RehabNetGameControl.Running;
                SessionManager.Instance.IsHoming = true;
            }

            if (m_SendGamePackage.Control == (int)RehabNetGameControl.Disconnect)
            {
                Close();
            }

            m_LastRehabNetRoboStatus = (RehabNetRobotStatus)m_ReceiveRobotPackage.Status;
        }
        catch
        {
            Close();
            if (m_RehabNetState != RehabNetState.Disconnecting)
                Connect();
        }
    }
    #endregion

    public void Close()
    {
        if (!IsConnected)
            return;

        m_ClientSocket.Close();
        m_ClientSocket = null;
    }

    private void OnApplicationQuit()
    {
        m_RehabNetState = RehabNetState.Disconnecting;
        Close();
    }

    public void Home()
    {
        m_RehabNetState = RehabNetState.Homing;
        m_SendGamePackage.Control = (int)RehabNetGameControl.Home;
    }

    public void Disconnect()
    {
        m_RehabNetState = RehabNetState.Disconnecting;
        m_SendGamePackage.Control = (int)RehabNetGameControl.Disconnect;
    }


}
