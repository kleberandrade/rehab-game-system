using UnityEngine;
using UnityEngine.UI;

public class RehabNetUI : MonoBehaviour
{
    public Text m_StatusText;
    public Button m_Button;
    public InputField m_TargetNumberInputField;
    public Text m_RobotPosition;
    public Toggle m_RobotDirectionToggle;

    public void Start()
    {
        m_Button.onClick.AddListener(delegate { RehabNetManager.Instance.Connection.Home(); });
        InvokeRepeating("UpdateStatus", 0.0f, 1.0f);
    }

    private void SaveNumberOfGoal()
    {
        PlayerPrefs.SetInt("NumberOfGoal", int.Parse(m_TargetNumberInputField.text));
    }

    private void OnDestroy()
    {
        SaveNumberOfGoal();
    }

    private void Update()
    {
        if (RehabNetManager.Instance.Connection.IsConnected)
            m_RobotPosition.text = string.Format("{0:0.000}", RehabNetManager.Instance.Connection.RobotPackage.Position);
        else
            m_RobotPosition.text = "-";
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
