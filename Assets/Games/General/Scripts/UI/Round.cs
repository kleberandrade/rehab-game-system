using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Round : MonoBehaviour
{
    public Image m_Bar;
    public CanvasGroup m_RoundGroupToFade;
    public CanvasGroup m_TextGroupToFade;
    public AudioSource m_ScoreUpAudio;
    public string[] m_MessagesToRoundStart = {"3", "2", "1", "GO!"};

    private float m_FadeTime = 0.5f;
    
    private void Awake()
    {
        m_ScoreUpAudio = GetComponent<AudioSource>();
    }

    public void SetRoundhUI()
    {
        
    }

    private IEnumerator Fade()
    {
        var rate = 1.0f / m_FadeTime;
        var startAlpha = 1;
        var endAlpha = 0;

        for (var i = 0; i < 2; i++)
        {
            if (i == 1)
                m_ScoreUpAudio.Play();

            var progress = 0.0f;

            while (progress < 1.0f)
            {
                m_TextGroupToFade.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }

            SetRoundhUI();
            startAlpha = 0;
            endAlpha = 1;
        }
    }
}
