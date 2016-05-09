using System;
using UnityEngine;

[Serializable]
public class Metric : IJsonData<Metric>
{
    public int Id;
    public string Name;

    public Metric()
    {
        this.Name = string.Empty;
    }

    public Metric(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public static Metric CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Metric>(jsonString);
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

