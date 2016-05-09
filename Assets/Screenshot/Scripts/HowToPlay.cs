using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    public string m_Text;
    public Sprite[] m_Sprite;
    public float m_Time = 4.0f;
    public float m_RepeatRate = 2.0f;
    public int m_CurrentSprite = 0;

    private RectTransform m_Transform;
    private CanvasGroup m_Group;

    private Image m_HowToPlayImage;
    private Text m_HowToPlayText;

    private void Start()
    {
        m_Transform = GetComponent<RectTransform>();
        m_Group = GetComponent<CanvasGroup>();

        m_HowToPlayImage = m_Transform.GetChild(1).GetChild(0).GetComponent<Image>();
        m_HowToPlayText = m_Transform.GetChild(0).GetChild(0).GetComponent<Text>();
        m_HowToPlayText.text = m_Text;

        InvokeRepeating("UpdateFrame", 0.0f, m_Time / m_RepeatRate / (float)m_Sprite.Length);
        Invoke("Hide", m_Time);
    }

    private void UpdateFrame()
    {
        m_HowToPlayImage.sprite = m_Sprite[m_CurrentSprite];
        m_CurrentSprite = ++m_CurrentSprite % m_Sprite.Length;
    }

    private void Hide()
    {
        StartCoroutine("Hidding");
    }

    private IEnumerator Hidding()
    {
        float rate = 1.0f / 0.5f;
        int startAlpha = 1;
        int endAlpha = 0;
        float progress = 0.0f;

        while (progress < 1.0)
        {
            m_Group.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        CancelInvoke("UpdateFrame");
        gameObject.SetActive(false);
    }
}
