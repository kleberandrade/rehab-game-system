using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Basket : MonoBehaviour
{
    [SerializeField] private AudioClip m_CollectedAudioClip;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Nut"))
        {
            m_AudioSource.Play();
            other.GetComponent<Nut>().Captured();
        }
    }
}
