using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;
    private StatsManager m_StatsManager;
    private Button m_ButtonClose;
    private AudioSource m_AudioSource;

    private float m_FadeTime = 0.4f;
    private bool m_IsFading = false;

    private void Start ()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_StatsManager = GetComponentInChildren<StatsManager>();
	}

    public void Show()
    {
        StartCoroutine(Fade(true, m_CanvasGroup, 0.0f));
    }

    public void Show (float delay)
    {
        StartCoroutine(Fade(true, m_CanvasGroup, delay));
	}

    public IEnumerator Fade(bool fadeIn, CanvasGroup canvasGroup, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (m_IsFading)
            yield break;

        m_IsFading = true;
        float rate = 1.0f / m_FadeTime;
        int startAlpha = 1 - Convert.ToInt32(fadeIn);
        int endAlpha = Convert.ToInt32(fadeIn);
        float progress = 0.0f;

        while (progress < 1.0)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        m_IsFading = false;

        m_IsFading = false;
    }

    public void Close()
    {
        //m_AudioSource.Play();
        SessionManager.Instance.SaveSession();
        SceneManager.LoadScene("Hub");
    }
}
