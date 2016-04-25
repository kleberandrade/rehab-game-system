using System;
using UnityEngine;

[Serializable]
public class Game : IJsonData<Game>
{
    public int Id;
    public string Name;
    public string Description;

    public Game()
    {
        this.Name = string.Empty;
        this.Description = string.Empty;
    }

    public Game(int id, string name, string description)
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;
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

