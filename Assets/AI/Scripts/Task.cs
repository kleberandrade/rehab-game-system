using System;
using System.Text;

public class Task : ICloneable
{
    public float Distance { get; set; }
    public float Speed { get; set; }
    public float Error { get; set; }
    public float Time { get; set; }

    public Task(float distance, float speed)
    {
        Distance = distance;
        Speed = speed;
    }

    public float Fitness(float kd, float ks, float ke, float kc)
    {
        return Distance * kd + Speed * ks + ke * Error + kc * Time;
    }

    public override string ToString()
    {
        return string.Format(" | {0:0.00000} | {1:0.00000} | ", Distance, Speed);
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}