using System;
using UnityEngine;

[Serializable]
public class Game : IJsonData<Game>
{
    public string Name;

    public Game(string name)
    {
        this.Name = name;
    }

    public static Game CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Game>(jsonString);
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

