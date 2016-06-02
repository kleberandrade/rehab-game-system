﻿using System;
using System.Text;
using UnityEngine;

[Serializable]
public class Task : IComparable<Task>, ICloneable, IJsonData<Task>
{
    public float Distance;
    public float Speed;
    public float Error;
    public float Time;
    public float Fitness;

    public Task(float distance, float speed)
    {
        Distance = distance;
        Speed = speed;
    }

    public void EvaluateFitness(float kd, float ks, float ke, float kc)
    {
        Fitness = Distance * kd + Speed * ks - ke * Error + kc * Time;
    }

    public override string ToString()
    {
        return string.Format(" | {0:0.00000} | {1:0.00000} | {2:0.00000} | {3:0.00000} | {4:0.00000} ", Distance, Speed, Error, Time, Fitness);
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public static Task CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Task>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public void Load(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }

    public int CompareTo(Task other)
    {
        if (Fitness < other.Fitness)
            return -1;
        else if (Fitness > other.Fitness)
            return 1;
        else
            return 0;
    }
}