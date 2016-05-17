using System;
using System.Text;

public class Task : ICloneable
{
    public float Distance { get; set; }
    public float Speed { get; set; }
    public float Error { get; set; }
    public float Time { get; set; }
    public float Fitness { get; set; }

    public Task(float distance, float speed)
    {
        Distance = distance;
        Speed = speed;
    }

    public float SetFitness(float kd, float ks, float ke, float kc)
    {
        Fitness = Distance * kd + Speed * ks - ke * Error + kc * Time;
        return Fitness;
    }

    public override string ToString()
    {
        return string.Format(" | {0:0.00000} | {1:0.00000} | {2:0.00000} | {3:0.00000} | {4:0.00000} ", Distance, Speed, Error, Time, Fitness);
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}