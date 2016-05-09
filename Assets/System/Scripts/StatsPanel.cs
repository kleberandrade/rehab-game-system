using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ArrowType
{
    Up,
    Down
}

public class StatsPanel : MonoBehaviour
{
    private Text m_Label;
    public string Label
    {
        get { return m_Label.text; }
        set { m_Label.text = value; }
    }

    private Text m_Value;
    public float Value
    {
        get { return float.Parse(m_Value.text); }
        set { m_Value.text = value.ToString(); }
    }

    private Image m_UpArrow;
    private Image m_DownArrow;

    public void SetArrow(ArrowType type)
    {
        if (type == ArrowType.Up)
        {
            m_UpArrow.gameObject.SetActive(true);
            m_DownArrow.gameObject.SetActive(false);
        }
        else
        {
            m_UpArrow.gameObject.SetActive(false);
            m_DownArrow.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        m_Label = transform.GetChild(0).GetComponent<Text>();
        m_Value = transform.GetChild(1).GetComponent<Text>();
        m_UpArrow = transform.GetChild(2).GetChild(0).GetComponent<Image>();
        m_DownArrow = transform.GetChild(2).GetChild(1).GetComponent<Image>();
    }


}
