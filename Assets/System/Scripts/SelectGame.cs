﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectGame : MonoBehaviour
{
    public int m_SceneNumber = 4;
    private Button m_Button;

	private void Start ()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(delegate { Select(); });
	}
	
	private void Select ()
    {
	    if (!RehabNetManager.Instance.Connection.IsConnected)
        {
            SystemDialogBox.Instance.Show(
                "Erro de conexão",
                "Por favor verifique a conexão com o robô",
                new string[] { "Fechar" },
                new UnityAction[] { null });

            return;
        }

        if (!SessionManager.Instance.IsHoming)
        {
            SystemDialogBox.Instance.Show(
                "Erro de callibração",
                "Por favor zere o centro do robô.",
                new string[] { "Fechar", "Zerar" },
                new UnityAction[] { null, new UnityAction(RehabNetManager.Instance.Connection.Home) });

            return;
        }

        ToPlay();
    }

    private void ToPlay()
    {
        SessionManager.Instance.SetGame(new Game("The Catcher"));
        SessionManager.Instance.SetDevice(new Device("MORE-W"));
        SessionManager.Instance.NewPerformance();
        SessionManager.Instance.NewTasks();
        SessionManager.Instance.NewSession();
        LoadingScreenManager.LoadScene(m_SceneNumber);
    }
}