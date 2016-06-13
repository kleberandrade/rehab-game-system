using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Slider m_Slider;
    private Text m_Text;
    private FadeInOut m_Fade;
    private int m_CurrentTarget;
    private int m_Point;
    private int m_NumberOfTargets;
    private RectTransform m_RectTransform;

    public int Point
    {
        get { return m_Point; }
    }

    public int Targets
    {
        get { return m_NumberOfTargets; }
    }

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Slider = GetComponent<Slider>();
        m_Text = GetComponentInChildren<Text>();
        m_Fade = GetComponentInChildren<FadeInOut>();
    }

    public void SetNumberOfTargets(int numberOfTargets)
    {
        m_NumberOfTargets = numberOfTargets;
        m_Slider.minValue = 0;
        m_Slider.maxValue = m_NumberOfTargets;
        m_Point = 0;
        m_CurrentTarget = 0;

        SetHealthUI();
    }

    public void NextPoint()
    {
        if (m_Point >= m_CurrentTarget)
            return;

        m_Point++;
        StartCoroutine(m_Fade.PulseInverse(0.2f));

        SetHealthUI();
    }

    public void NextTarget()
    {
        m_CurrentTarget++;
        SetHealthUI();
    }

    private void SetHealthUI()
    {
        m_Slider.value = Mathf.Min(m_CurrentTarget, m_NumberOfTargets);
        m_Text.text = m_Point.ToString();
    }

    public Vector3 WorldPoint(Vector3 targetPosition)
    {
        float distance = Mathf.Abs(Camera.main.transform.position.z - targetPosition.z);
        Vector3 position = m_RectTransform.position + new Vector3(1.0f, -20.0f, 0.0f);
        position.z = distance;
        return Camera.main.ScreenToWorldPoint(position);
    }
}