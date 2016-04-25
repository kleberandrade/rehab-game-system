using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Slider m_Slider;
    public Text m_Text;
    public CanvasGroup m_GroupToFadeAnimation;

    private int m_CurrentGoal;
    private int m_CurrentScore;
    private int m_MaxGoal = 300;
    private float m_FadeTime = 0.3f;
    private AudioSource m_ScoreUpAudio;
    private RectTransform m_RectTransform;
    private Camera m_Camera;

    private void Awake()
    {
        m_ScoreUpAudio = GetComponent<AudioSource>();
        m_RectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        m_Camera = Camera.main;
    }

    public void Reset(int maxGoal)
    {
        m_FadeTime = m_ScoreUpAudio.clip.length;
        m_MaxGoal = maxGoal;
        m_Slider.minValue = 0;
        m_Slider.maxValue = m_MaxGoal;
        m_CurrentScore = 0;
        m_CurrentGoal = 0;
        SetHealthUI();
    }

    public void Up()
    {
        if (m_CurrentScore >= m_CurrentGoal)
            return;

        m_CurrentScore++;
        StartCoroutine("Fade");
    }

    public bool HasNext()
    {
        return m_CurrentGoal < m_MaxGoal;
    }

    public void Next()
    {
        if (!HasNext())
            return;

        m_CurrentGoal++;
        SetHealthUI();
    }

    private void SetHealthUI()
    {
        m_Slider.value = Mathf.Min(m_CurrentGoal, m_MaxGoal);
        m_Text.text = m_CurrentScore.ToString();
    }

    private IEnumerator Fade()
    {
        var rate = 1.0f/m_FadeTime;
        var startAlpha = 1;
        var endAlpha = 0;

        for (var i = 0; i < 2; i++)
        {
            if (i == 1)
                m_ScoreUpAudio.Play();

            var progress = 0.0f;

            while (progress < 1.0f)
            {
                m_GroupToFadeAnimation.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
                progress += rate*Time.deltaTime;
                yield return null;
            }

            SetHealthUI();
            startAlpha = 0;
            endAlpha = 1;
        }
    }

    public Vector3 WorldPoint(Vector3 targetPosition)
    {
        float distance = Mathf.Abs(m_Camera.transform.position.z - targetPosition.z);
        Vector3 position = m_RectTransform.position;
        position.z = distance;
        return Camera.main.ScreenToWorldPoint(position);
    }
}