using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(ShowAndHidePanel))]
public class SystemDialogBox : Singleton<SystemDialogBox>
{
    private CanvasGroup m_CanvasGroup;
    private RectTransform m_FaderPanel;
    private RectTransform m_MessagePanel;

    private Text m_Title;
    private Text m_Message;
    private Button[] m_Butons = new Button[2];
    private Text[] m_TextButtons = new Text[2];

    private ShowAndHidePanel m_Transition;

    private void Start()
    {
        m_Transition = GetComponent<ShowAndHidePanel>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_CanvasGroup.alpha = 0;
                
        m_FaderPanel = transform.GetChild(0).GetComponent<RectTransform>();
        m_MessagePanel = transform.GetChild(1).GetComponent<RectTransform>();

        m_Title = m_MessagePanel.GetChild(0).GetComponent<Text>();

        m_Message = m_MessagePanel.GetChild(1).GetComponent<Text>();

        m_Butons[0] = m_MessagePanel.GetChild(2).GetComponent<Button>();
        m_Butons[0].onClick.RemoveAllListeners();
        m_Butons[0].onClick.AddListener(delegate { Hide(); });

        m_Butons[1] = m_MessagePanel.GetChild(3).GetComponent<Button>();
        m_Butons[1].onClick.RemoveAllListeners();
        m_Butons[1].onClick.AddListener(delegate { Hide(); });

        m_TextButtons[0] = m_Butons[0].transform.GetChild(0).GetComponent<Text>();
        m_TextButtons[1] = m_Butons[1].transform.GetChild(0).GetComponent<Text>();

        m_FaderPanel.gameObject.SetActive(false);
        m_MessagePanel.gameObject.SetActive(false);
    }

    public void Show(string title, string message, string[] textButtons, UnityAction[] actionEvents)
    {
        m_Title.text = title;
        m_Message.text = message;

        m_Butons[0].gameObject.SetActive(true);
        m_Butons[0].onClick.RemoveAllListeners();
        m_Butons[0].onClick.AddListener(delegate { Hide(); });
        if (actionEvents[0] != null)
            m_Butons[0].onClick.AddListener(actionEvents[0]);
        m_TextButtons[0].text = textButtons[0].ToUpper();

        m_Butons[1].gameObject.SetActive(false);
        if (textButtons.Length == 2 && actionEvents.Length == 2)
        {
            m_Butons[1].gameObject.SetActive(true);
            m_Butons[1].onClick.RemoveAllListeners();
            m_Butons[1].onClick.AddListener(delegate { Hide(); });
            if (actionEvents[1] != null)
                m_Butons[1].onClick.AddListener(actionEvents[1]);
            m_TextButtons[1].text = textButtons[1].ToUpper();
        }

        m_FaderPanel.gameObject.SetActive(true);
        m_MessagePanel.gameObject.SetActive(true);
        StartCoroutine(m_Transition.Fade(true, m_CanvasGroup, new RectTransform[]{ m_FaderPanel, m_MessagePanel}));
    }

    public void Hide()
    {
        StartCoroutine(m_Transition.Fade(false, m_CanvasGroup, new RectTransform[] { m_FaderPanel, m_MessagePanel }));
    }
}
