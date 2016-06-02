using System;
using UnityEngine;

[Serializable]
public class User : IJsonData<User>, IComparable<User>
{ 
    public string Name;
    public string Username;
    public string Password;
    public string TherapistName;

    public User(string name, string username, string password, string therapistName)
    {
        this.Name = name;
        this.Username = username;
        this.Password = password;
        this.TherapistName = therapistName;
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

    public int CompareTo(User other)
    {
        return Name.CompareTo(other.Name);
    }
}