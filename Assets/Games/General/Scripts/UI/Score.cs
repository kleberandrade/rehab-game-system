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
        m_Slider = GetComponent<Slider>();
        m_Text = GetComponentInChildren<Text>();
        m_Fade = GetComponentInChildren<FadeInOut>();
    }

    public void SetNumberOfTargets(int numberOfTargets)
    {
        Debug.Log("Score: Set number of targets = " + numberOfTargets);
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
}