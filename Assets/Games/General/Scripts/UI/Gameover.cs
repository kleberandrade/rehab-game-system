using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(FadeInOut))]
public class Gameover : MonoBehaviour
{
    private FadeInOut m_Fade;
    private Button m_ButtonClose;
    private AudioSource m_AudioSource;

    private void Awake ()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Fade = GetComponent<FadeInOut>();
	}

    private void Start()
    {
        m_ButtonClose = transform.GetChild(2).GetComponent<Button>();
        m_ButtonClose.onClick.AddListener(delegate { Close(); });
    }

    public void Show()
    {
        m_Fade.Fade(true, 1.0f, 0.0f);
    }
    
    public void Close()
    {
        m_AudioSource.Play();
        SceneManager.LoadScene("Hub");
    }
}
