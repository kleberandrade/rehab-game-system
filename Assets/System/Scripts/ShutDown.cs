using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShutDown : MonoBehaviour
{
    [SerializeField] private Button m_ShutDownButton;
    private UnityAction m_YesAction;
    private UnityAction m_NoAction;
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();

        m_YesAction = new UnityAction(Quit);
        m_NoAction = new UnityAction(Cancel);

        m_ShutDownButton.onClick.RemoveAllListeners();
        m_ShutDownButton.onClick.AddListener(delegate { ShowMessage(); });
    }

    private void ShowMessage()
    {
        m_AudioSource.Play();

        SystemDialogBox.Instance.Show(
            "Confirmar saída", 
            "Deseja sair do sistema de reabilitação robótica?",
            new string[]{ "Não", "Sim" },
            new UnityAction[]{ m_NoAction, m_YesAction}
        );
    }

    private void Quit()
    {
        m_AudioSource.Play();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Cancel()
    {
        m_AudioSource.Play();
    }
}
