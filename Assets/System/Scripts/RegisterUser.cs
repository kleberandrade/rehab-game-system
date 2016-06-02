using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ShowAndHidePanel))]
public class RegisterUser : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;
    private RectTransform m_FaderPanel;
    private RectTransform m_UserPanel;

    private InputField m_NameInputValue;
    private InputField m_EmailInputValue;
    private InputField m_PasswordInputValue;

    private Button m_CancelButton;
    private Button m_RegisterButton;

    private AudioSource m_AudioSource;

    private ShowAndHidePanel m_Transition;

    public UsersDropdown m_UserDropdown;

    private void Start()
    {
        m_Transition = GetComponent<ShowAndHidePanel>();
        m_AudioSource = GetComponent<AudioSource>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_CanvasGroup.alpha = 0;

        m_FaderPanel = transform.GetChild(0).GetComponent<RectTransform>();
        m_UserPanel = transform.GetChild(1).GetComponent<RectTransform>();

        m_NameInputValue = m_UserPanel.GetChild(1).GetChild(0).GetComponent<InputField>();
        m_EmailInputValue = m_UserPanel.GetChild(1).GetChild(1).GetComponent<InputField>();
        m_PasswordInputValue = m_UserPanel.GetChild(1).GetChild(2).GetComponent<InputField>();

        m_CancelButton = m_UserPanel.GetChild(2).GetComponent<Button>();
        m_CancelButton.onClick.RemoveAllListeners();
        m_CancelButton.onClick.AddListener(delegate { Cancel(); });

        m_RegisterButton = m_UserPanel.GetChild(3).GetComponent<Button>();
        m_RegisterButton.onClick.RemoveAllListeners();
        m_RegisterButton.onClick.AddListener(delegate { Register(); });

        m_FaderPanel.gameObject.SetActive(false);
        m_UserPanel.gameObject.SetActive(false);
    }

    public void Show()
    {
        Clear();
        m_FaderPanel.gameObject.SetActive(true);
        m_UserPanel.gameObject.SetActive(true);
        StartCoroutine(m_Transition.Fade(true, m_CanvasGroup, new RectTransform[] { m_FaderPanel, m_UserPanel }));
    }

    public void Hide()
    {
        StartCoroutine(m_Transition.Fade(false, m_CanvasGroup, new RectTransform[] { m_FaderPanel, m_UserPanel }));
    }

    private void Register()
    {
        m_AudioSource.Play();

        if (m_NameInputValue.text == string.Empty || m_EmailInputValue.text == string.Empty || m_PasswordInputValue.text == string.Empty)
        {
            SystemDialogBox.Instance.Show(
                "Erro ao criar usuário",
                "Por favor, verifique se existem campos vazios",
                new string[] { "Fechar" },
                new UnityAction[] { null }
            );
            
            return;
        }

        User patient = new User(m_NameInputValue.text,
            m_EmailInputValue.text,
            m_PasswordInputValue.text,
            SessionManager.Instance.GetTherapistName());

        if (SessionManager.Instance.SaveNewUser(patient.SaveToString()))
        {
            m_UserDropdown.SetPatient(patient);
            Hide();
        }
        else
        {
            SystemDialogBox.Instance.Show(
                "Erro ao criar usuário",
                "Por favor verifique a conexão com a internet",
                new string[] { "Fechar" },
                new UnityAction[] { null });
        }
    }

    private void Cancel()
    {
        m_AudioSource.Play();
        Hide();
    }

    private void Clear()
    {
        m_NameInputValue.text = string.Empty;
        m_EmailInputValue.text = string.Empty;
        m_PasswordInputValue.text = string.Empty;
    }
}