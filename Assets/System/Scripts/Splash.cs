using UnityEngine;
using UnityEngine.CrashLog;

public class Splash : MonoBehaviour
{
    public string m_SceneName;
    public float m_Time = 3.0f;
   
    private void Awake()
    {
        CrashReporting.Init("c31e08ef-2237-4e0e-a5a4-4f085e756c78", "1.0.0", "Rehab System");
    }

    private void Start()
    {
        Invoke("ChangeScene", m_Time);
    }

    private void ChangeScene()
    {
        LoadingScreenManager.LoadScene(2);
    }
}
