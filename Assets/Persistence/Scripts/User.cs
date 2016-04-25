using System;
using UnityEngine;

[Serializable]
public class User : IJsonData<User>
{
    public int Id;
    public string Name;
    public string Username;
    public string Email;
    public string Password;
    public string Type;
    public int TherapistId;

    public User()
    {
        this.Name = string.Empty;
        this.Username = string.Empty;
        this.Email = string.Empty;
        this.Password = string.Empty;
        this.Type = string.Empty;
    }

    public User(int id, string name, string username, string email, string password, string type, int therapistId)
    {
        this.Id = id;
        this.Name = name;
        this.Username = username;
        this.Email = email;
        this.Password = password;
        this.Type = type;
        this.TherapistId = therapistId;
    }

    public static User CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<User>(jsonString);
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