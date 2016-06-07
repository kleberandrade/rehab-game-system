using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public RectTransform m_LoginPanel;
    private InputField m_EmailInputValue;
    private InputField m_PasswordInputValue;
    private Toggle m_RememberToggle;
    private Button m_LoginButton;
    private AudioSource m_AudioSource;
    private EventSystem m_EventSystem;
    private string m_RememberUserKey = "RememberUserKey";

    private void Start()
    {
        m_EventSystem = FindObjectOfType<EventSystem>();

        m_AudioSource = GetComponent<AudioSource>();

        m_EmailInputValue = m_LoginPanel.GetChild(0).GetComponent<InputField>();

        m_PasswordInputValue = m_LoginPanel.GetChild(1).GetComponent<InputField>();
        m_PasswordInputValue.inputType = InputField.InputType.Password;

        m_RememberToggle = m_LoginPanel.GetChild(2).GetComponent<Toggle>();

        m_LoginButton = m_LoginPanel.GetChild(3).GetComponent<Button>();
        m_LoginButton.onClick.RemoveAllListeners();
        m_LoginButton.onClick.AddListener(delegate { Login(); });

        m_EmailInputValue.text = GetUserNameIfRemember();
    }
    
    private string GetUserNameIfRemember()
    {
        string email = string.Empty;
        m_RememberToggle.isOn = false;

        if (PlayerPrefs.HasKey(m_RememberUserKey))
        {
            email = PlayerPrefs.GetString(m_RememberUserKey);
            m_EventSystem.firstSelectedGameObject = m_PasswordInputValue.gameObject;
            m_RememberToggle.isOn = true;
        }

        return email;
    }

    private void RememberChanged(string username)
    {
        if (m_RememberToggle.isOn)
        {
            PlayerPrefs.SetString(m_RememberUserKey, username);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.DeleteKey(m_RememberUserKey);
        }
    }

    private void Login()
    {
        m_AudioSource.Play();
        string username = m_EmailInputValue.text;
        string password = m_PasswordInputValue.text;
        RememberChanged(username);
        SessionManager.Instance.Login(username, password);
    }

    private void Cancel()
    {
        m_AudioSource.Play();
    }
}
