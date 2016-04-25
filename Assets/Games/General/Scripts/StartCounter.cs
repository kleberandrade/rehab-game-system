using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartCounter : MonoBehaviour
{
    [SerializeField] private string[] m_TextToCounter = { "3", "2", "1", "Vai!" };
    [SerializeField] private float m_FadeTime = 0.5f;
    [SerializeField] private AudioClip m_CountingAudioClip;
    [SerializeField] private AudioClip m_FinishingAudioClip;

    private CanvasGroup m_TextGroupToFade;
    private AudioSource m_AudioSource;
    private Text m_Text;
    
    private void Awake()
    {
        m_Text = GetComponent<Text>();
        m_TextGroupToFade = GetComponent<CanvasGroup>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void BeginCounting()
    {
        StartCoroutine(Fade(0));
    }

    private IEnumerator Fade(int index)
    {
        var rate = 1.0f / m_FadeTime;
        var startAlpha = 0;
        var endAlpha = 1;

        m_Text.text = m_TextToCounter[index];
        for (var i = 0; i < 2; i++)
        {
            var progress = 0.0f;

            while (progress < 1.0f)
            {
                m_TextGroupToFade.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }

            m_TextGroupToFade.alpha = endAlpha;

            if (i != 1)
            {
                m_AudioSource.clip = index == m_TextToCounter.Length - 1 ? m_FinishingAudioClip : m_CountingAudioClip;
                m_AudioSource.Play();
            }


            startAlpha = 1;
            endAlpha = 0;
        };

        if (index < m_TextToCounter.Length - 1)
            StartCoroutine(Fade(index + 1));
    }

}
