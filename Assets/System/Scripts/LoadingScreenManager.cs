using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Loading Visuals")]
    public Image m_LoadingIcon;
    public Image m_LoadingDoneIcon;
    public Text m_LoadingText;
    public Slider m_ProgressBar;
    public Image m_FadeOverlay;

    [Header("Timing Settings")]
    public float m_WaitOnLoadEnd = 0.25f;
    public float m_FadeDuration = 0.25f;

    [Header("Loading Settings")]
    public LoadSceneMode m_LoadSceneMode = LoadSceneMode.Single;
    public ThreadPriority m_LoadThreadPriority;

    [Header("Other")]
    // If loading additive, link to the cameras audio listener, to avoid multiple active audio listeners
    public AudioListener m_AudioListener;

    private AsyncOperation m_Operation;
    private Scene m_CurrentScene;

    public static int m_SceneToLoad = -1;
    // IMPORTANT! This is the build index of your loading scene. You need to change this to match your actual scene index
    private static int m_LoadingSceneIndex = 1;

    public static void LoadScene(int levelNum)
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        m_SceneToLoad = levelNum;
        SceneManager.LoadScene(m_LoadingSceneIndex);
    }

    private void Start()
    {
        if (m_SceneToLoad < 0)
            return;

        m_FadeOverlay.gameObject.SetActive(true); // Making sure it's on so that we can crossfade Alpha
        m_CurrentScene = SceneManager.GetActiveScene();
        StartCoroutine(LoadAsync(m_SceneToLoad));
    }

    private IEnumerator LoadAsync(int levelNum)
    {
        ShowLoadingVisuals();

        yield return null;

        FadeIn();
        StartOperation(levelNum);

        float lastProgress = 0f;

        // operation does not auto-activate scene, so it's stuck at 0.9
        while (DoneLoading() == false)
        {
            yield return null;

            if (Mathf.Approximately(m_Operation.progress, lastProgress) == false)
            {
                m_ProgressBar.value = m_Operation.progress;
                lastProgress = m_Operation.progress;
            }
        }

        if (m_LoadSceneMode == LoadSceneMode.Additive)
            m_AudioListener.enabled = false;

        ShowCompletionVisuals();

        yield return new WaitForSeconds(m_WaitOnLoadEnd);

        FadeOut();

        yield return new WaitForSeconds(m_FadeDuration);

        if (m_LoadSceneMode == LoadSceneMode.Additive)
            SceneManager.UnloadScene(m_CurrentScene.name);
        else
            m_Operation.allowSceneActivation = true;
    }

    private void StartOperation(int levelNum)
    {
        Application.backgroundLoadingPriority = m_LoadThreadPriority;
        m_Operation = SceneManager.LoadSceneAsync(levelNum, m_LoadSceneMode);

        if (m_LoadSceneMode == LoadSceneMode.Single)
            m_Operation.allowSceneActivation = false;
    }

    private bool DoneLoading()
    {
        return (m_LoadSceneMode == LoadSceneMode.Additive && m_Operation.isDone) || (m_LoadSceneMode == LoadSceneMode.Single && m_Operation.progress >= 0.9f);
    }

    private void FadeIn()
    {
        m_FadeOverlay.CrossFadeAlpha(0, m_FadeDuration, true);
    }

    private void FadeOut()
    {
        m_FadeOverlay.CrossFadeAlpha(1, m_FadeDuration, true);
    }

    private void ShowLoadingVisuals()
    {
        m_LoadingIcon.gameObject.SetActive(true);
        m_LoadingDoneIcon.gameObject.SetActive(false);
        m_ProgressBar.value = 0.0f;
        m_LoadingText.text = "CARREGANDO...";
    }

    private void ShowCompletionVisuals()
    {
        m_LoadingIcon.gameObject.SetActive(false);
        m_LoadingDoneIcon.gameObject.SetActive(true);
        m_ProgressBar.value = 1.0f;
        m_LoadingText.text = "CARREGAMENTO PRONTO";
    }

}