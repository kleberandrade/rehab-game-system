using System.Text;
using UnityEngine;

public class DynamicDifficulty : MonoBehaviour
{
    // Constantes para calculo do fitness
    public float m_Kd = 0.3483f;
    public float m_Ks = 0.4679f;
    public float m_Ke = 0.6692f;
    public float m_Kc = 0.5319f;
    // Constantes para mutação
    public float m_Md = 0.05f;
    public float m_Ms = 0.05f;
    // Número de tarefas
    public int m_NumberOfTask = 5;

    private Task[] m_TasksToCalibration;
    private Task[] m_Tasks;
    private int m_IndexOfTask = 0;
    private int m_IndexOfCalibration = 0;

    private float m_MinDistante;
    private float m_MaxDistante;
    private float m_MinSpeed;
    private float m_MaxSpeed;

    public DynamicDifficulty()
    {
        InitializeCalibrationTasks();
        InitializeRandomTasks();
    }

    public void InitializeCalibrationTasks()
    {
        m_IndexOfCalibration = 0;
        m_TasksToCalibration = new Task[4];
        // Primeiro nível de testes
        m_TasksToCalibration[0] = new Task(0.0f, 0.1f);
        m_TasksToCalibration[1] = new Task(0.5f, 0.1f);
        m_TasksToCalibration[2] = new Task(1.0f, 0.1f);
        m_TasksToCalibration[3] = new Task(0.5f, 0.1f);
    }

    public void InitializeRandomTasks()
    {
        m_IndexOfTask = 0;
        m_Tasks = new Task[m_NumberOfTask];

        for (int i = 0; i < m_NumberOfTask; i++)
            m_Tasks[i] = new Task(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

        Debug.Log("GA: Initialize random population...");
    }

    public Task NextTask()
    {
        if (m_IndexOfCalibration < m_TasksToCalibration.Length)
        {
            Task task = m_TasksToCalibration[m_IndexOfCalibration];
            m_IndexOfCalibration++;
            return task;
        }

        if (m_IndexOfTask < m_Tasks.Length)
        {
            Task task = m_Tasks[m_IndexOfTask];
            m_IndexOfTask++;

            Debug.Log("GA: Chromosome: " + task.ToString());
            return task;
        }

        Task selectedTask = Selection();
        GeneticOperator(selectedTask);
        m_IndexOfTask = 0;
        return NextTask();
    }

    public Task Selection()
    {
        // Calcula o fitness
        float[] fitness = new float[m_Tasks.Length];
        for (int i = 0; i < m_Tasks.Length; i++)
            fitness[i] = m_Tasks[0].Fitness(m_Kd, m_Ks, m_Ke, m_Kc);

        Debug.Log("GA: Evaluate fitness");

        // Procura o Maior Fitness
        int index = 0;
        float bestFitness = fitness[0];
        for (int i = 1; i < m_Tasks.Length; i++)
        {
            if (bestFitness < fitness[i])
            {
                index = i;
                bestFitness = fitness[i];
            }
        }

        Debug.Log("GA: Selection chromosome[" + index + "]: " + m_Tasks[index].ToString());

        return m_Tasks[index];
    }

    public void GeneticOperator(Task selectedTasks)
    {
        // Crossover e Mutação
        for (int i = 0; i < m_Tasks.Length; i++)
        {
            m_Tasks[i] = selectedTasks;
            m_Tasks[i].Distance += Mathf.Clamp(Random.Range(-m_Md, m_Md) * m_Tasks[i].Distance, 0.0f, 1.0f);
            m_Tasks[i].Speed += Mathf.Clamp(Random.Range(-m_Ms, m_Ms) * m_Tasks[i].Speed, 0.0f, 1.0f);
            m_Tasks[i].Error = 0.0f;
            m_Tasks[i].Time = 0.0f;
        }

        Debug.Log("GA: Crossover and mutation...");
    }

    public void Evaluation(float error, float time)
    {
        m_Tasks[m_IndexOfTask].Error = error;
        m_Tasks[m_IndexOfTask].Time = time;
    }
}