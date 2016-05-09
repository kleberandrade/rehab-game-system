using System;
using UnityEngine;

[Serializable]
public class Performance : IJsonData<Performance>
{
    public string Timestamp;
    public Metric Metric;
    public double Value;

    public Performance()
    {

    }

    public Performance(Metric metric, double value)
    {
        this.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        this.Metric = metric;
        this.Value = value;
    }

    public Performance(string timestamp, Metric metric, double value)
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

