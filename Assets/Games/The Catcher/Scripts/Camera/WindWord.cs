using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindWord : MonoBehaviour
{
    public float m_NormalPeriodOscillator = 2.0f;
    public float m_NormalAmplitudeOscillator = 1.0f;
    public float m_PeriodOscillator = 35.0f;
    public float m_AmplitudeOscillator = 3.0f;
    public List<Oscillator> m_Oscillators;

    private float m_TimeToOscilattion = 1.0f;
    private AudioSource m_AudioSource;
    
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        m_NormalAmplitudeOscillator = m_Oscillators[0].Amplitude;
        m_NormalPeriodOscillator = m_Oscillators[0].Period;
        m_TimeToOscilattion = m_AudioSource.clip.length - 0.5f;
    }

    public void Fan()
    {
        StartCoroutine(BlowingStrong());
    }

    private IEnumerator BlowingStrong()
    {
        m_AudioSource.Play();

        for (int i = 0; i < m_Oscillators.Count; i++)
        {
            m_Oscillators[i].Period = m_PeriodOscillator;
            m_Oscillators[i].Amplitude = m_AmplitudeOscillator;
        }

        yield return new WaitForSeconds(m_TimeToOscilattion);

        for (int i = 0; i < m_Oscillators.Count; i++)
        {
            m_Oscillators[i].Period = m_NormalPeriodOscillator;
            m_Oscillators[i].Amplitude = m_NormalAmplitudeOscillator;
        }
    }
}
