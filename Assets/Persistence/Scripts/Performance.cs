using System;
using UnityEngine;

[Serializable]
public class Performance : IJsonData<Performance>
{
    public int SessionId;
    public string Timestamp;
    public Metric Metric;
    public double Value;

    public Performance()
    {

    }

    public Performance(int sessionId, string timestamp, Metric metric, double value)
    {
        this.SessionId = sessionId;
        this.Timestamp = timestamp;
        this.Metric = metric;
        this.Value = value;
    }

    public static Performance CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Performance>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public void Load(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }
}

