using UnityEngine;
using System.Collections;

public enum StatsType
{
    Score = 0,
    Difficulty,
    Skill,
    Time,
    RobotInit
}

public class StatsManager : MonoBehaviour
{
    private StatsPanel[] m_Stats;

    private void Start()
    {
        m_Stats = GetComponentsInChildren<StatsPanel>();
    }

    private void SetStats(string value, ArrowType arrowType, StatsType statsType)
    {
        m_Stats[(int)statsType].Value = value;
        m_Stats[(int)statsType].SetArrow(arrowType);
    }

    public void SetScore(float value, ArrowType type)
    {
        SetStats(value.ToString(), type, StatsType.Score);
    }

    public void SetDifficulty(float value, ArrowType type)
    {
        SetStats(value.ToString(), type, StatsType.Difficulty);
    }

    public void SetSkill(float value, ArrowType type)
    {
        SetStats(value.ToString(), type, StatsType.Skill);
    }

    public void SetTime(float value, ArrowType type)
    {
        SetStats(value.ToString(), type, StatsType.Time);
    }

    public void SetRobotInit(float value, ArrowType type)
    {
        SetStats(value.ToString(), type, StatsType.RobotInit);
    }

    public void SetScore(string value, ArrowType type)
    {
        SetStats(value, type, StatsType.Score);
    }

    public void SetDifficulty(string value, ArrowType type)
    {
        SetStats(value, type, StatsType.Difficulty);
    }

    public void SetSkill(string value, ArrowType type)
    {
        SetStats(value, type, StatsType.Skill);
    }

    public void SetTime(string value, ArrowType type)
    {
        SetStats(value, type, StatsType.Time);
    }

    public void SetRobotInit(string value, ArrowType type)
    {
        SetStats(value, type, StatsType.RobotInit);
    }
}
