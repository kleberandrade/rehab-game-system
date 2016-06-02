using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Session : IJsonData<Session>
{
    public string Timestamp;
    public User Patient;
    public User Therapist;
    public Game Game;
    public Device Device;
    public List<Task> Tasks;
    public List<Performance> Performances;    

    public Session()
    {
        this.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
        this.Tasks = new List<Task>();
        this.Performances = new List<Performance>();
    }

    public Session(string timestamp, User patient, User therapist, Game game, Device device)
    {
        this.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
        this.Patient = patient;
        this.Therapist = therapist;
        this.Game = game;
        this.Device = device;
        this.Tasks = new List<Task>();
        this.Performances = new List<Performance>();
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