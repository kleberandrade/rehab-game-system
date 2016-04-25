using System.Text;

public class Task
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
        StringBuilder builder = new StringBuilder();
        builder.Append(" | ");
        builder.Append(Distance);
        builder.Append(" | ");
        builder.Append(Speed);
        builder.Append(" | ");
        return builder.ToString();
    }
}