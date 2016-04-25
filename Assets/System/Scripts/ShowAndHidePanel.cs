using System;
using System.Collections;
using UnityEngine;

public class ShowAndHidePanel : MonoBehaviour
{
    private float m_FadeTime = 0.4f;
    private bool m_IsFading = false;

    public IEnumerator Fade(bool fadeIn, CanvasGroup canvasGroup, RectTransform[] panels)
    {
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

        if (!fadeIn)
        {
            foreach (RectTransform rect in panels)
                rect.gameObject.SetActive(false);
        }

        m_IsFading = false;
    }
}
