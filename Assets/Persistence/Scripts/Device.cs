using System;
using UnityEngine;

[Serializable]
public class Device : IJsonData<Device>
{
    public string Name;
    
    public Device(string name)
    {
        this.Name = name;
    }

    public static Device CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Device>(jsonString);
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
