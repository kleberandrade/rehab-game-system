using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectGame : MonoBehaviour
{
    public string m_SceneName;
    private Button m_Button;

	private void Start ()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(delegate { Select(); });
	}
	
	private void Select ()
    {/*
	    if (!RehabNetManager.Instance.Connection.IsConnected)
        {
            SystemDialogBox.Instance.Show(
                "Erro de conexão",
                "Por favor verifique a conexão com o robô",
                new string[] { "Fechar" },
                new UnityAction[] { null });
        }
        else if (SessionManager.Instance.IsHoming)
        {
            SystemDialogBox.Instance.Show(
                "Erro de callibração",
                "Por favor zere o centro do robô.",
                new string[] { "Cancelar", "Zerar" },
                new UnityAction[] { null, new UnityAction(RehabNetManager.Instance.Connection.Home) });
        }
        else
        {
            
            SystemDialogBox.Instance.Show(
                 "Erro de callibração",
                 "Por favor zere o centro do robô.",
                 new string[] { "Cancelar", "Zerar", "Jogar" },
                 new UnityAction[] { null, new UnityAction(RehabNetManager.Instance.Connection.Home), new UnityAction(ToPlay) });
        }
        */

        ToPlay();
    }

    private void ToPlay()
    {
        SessionManager.Instance.SetGame(new Game("The Catcher"));
        SessionManager.Instance.SetDevice(new Device("MORE-W"));
        SessionManager.Instance.NewPerformance();
        SessionManager.Instance.NewTasks();
        SceneManager.LoadScene(m_SceneName);
    }
}