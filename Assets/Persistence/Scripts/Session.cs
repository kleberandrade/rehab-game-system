using System;
using UnityEngine;

[Serializable]
public class Session : IJsonData<Session>
{
    public int Id;
    public string Timestamp;
    public User Patient;
    public User Therapist;
    public Game Game;
    public Device Device;

    public Session()
    {
        this.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        this.Patient = new User();
        this.Therapist = new User();
        this.Game = new Game();
        this.Device = new Device();
    }

    public Session(int id, string timestamp, User patient, User therapist, Game game, Device device)
    {
        this.Id = id;
        this.Timestamp = timestamp;
        this.Patient = patient;
        this.Therapist = therapist;
        this.Game = game;
        this.Device = device;
    }

    public static Session CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Session>(jsonString);
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