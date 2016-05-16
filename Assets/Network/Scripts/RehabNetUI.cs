using UnityEngine;
using UnityEngine.UI;

public class RehabNetUI : MonoBehaviour
{
    public Text m_StatusText;
    public Button m_Button;

    public void Start()
    {
        m_Button.onClick.AddListener(delegate { RehabNetManager.Instance.Connection.Home(); });
        InvokeRepeating("UpdateStatus", 0.0f, 1.0f);
    }

    private void UpdateStatus()
    {
        m_Button.interactable = RehabNetManager.Instance.Connection.IsConnected;

        if (RehabNetManager.Instance.Connection.IsConnected)
        {
            m_StatusText.text = "Comunicando com o robô";
        }
        else
        {
            m_StatusText.text = "Estabelecendo conexão com o robô...";
        }
    }
}
