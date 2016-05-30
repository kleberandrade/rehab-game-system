using UnityEngine;
using System.Collections;

public class PlayerEyes : MonoBehaviour 
{
    [SerializeField] private Texture[] m_Eyes;
    [SerializeField] private float m_RepeatRate = 0.15f;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_Threshold = 0.7f;

    private Renderer m_Renderer;

	private void Start()
	{
        m_Renderer = GetComponent<Renderer>();

        if (m_Eyes.Length >= 2)
		    InvokeRepeating("EyesChanged", 0.0f, m_RepeatRate);
	}

    private void EyesChanged()
	{
       m_Renderer.material.mainTexture = m_Eyes[Random.Range(0.0f, 1.0f) > m_Threshold ? 0 : 1];   
	}
}
