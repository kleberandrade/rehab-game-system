using System;
using UnityEngine;

[Serializable]
public class Performance : IJsonData<Performance>
{
    public string Timestamp;
    public string Metric;
    public double Value;

    public Performance(string metric, double value)
    {
        this.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff");
        this.Metric = metric;
        this.Value = value;
    }

    public Performance(string timestamp, string metric, double value)
    {
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

