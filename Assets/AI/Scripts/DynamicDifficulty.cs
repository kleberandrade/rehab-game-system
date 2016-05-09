using System.Text;
using UnityEngine;

public class DynamicDifficulty : MonoBehaviour
{
    // Constantes para calculo do fitness
    [Range(0.0f,1.0f)]
    public float m_Kd = 0.3483f;
    [Range(0.0f, 1.0f)]
    public float m_Ks = 0.4679f;
    [Range(0.0f, 1.0f)]
    public float m_Ke = 0.6692f;
    [Range(0.0f, 1.0f)]
    public float m_Kc = 0.5319f;
    // Constantes para mutação
    [Range(0.0f, 1.0f)]
    public float m_Md = 0.1f;
    [Range(0.0f, 1.0f)]
    public float m_Ms = 0.1f;
    // Número de tarefas
    public int m_NumberOfTask = 5;
    public int m_GainToNumberOfTask = 4;

    private Task[] m_Tasks;
    private int m_IndexOfTask = 0;

    private float m_MinDistante;
    private float m_MaxDistante;
    private float m_MinSpeed;
    private float m_MaxSpeed;

    public void Start()
    {
        InitializeRandomTasks();
    }

    public void InitializeRandomTasks()
    {
        m_IndexOfTask = 0;
        m_Tasks = new Task[m_NumberOfTask * m_GainToNumberOfTask];

        for (int i = 0; i < m_Tasks.Length; i++)
            m_Tasks[i] = new Task(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

        Debug.Log("GA: Initialize random population...");
    }

    public Task NextTask()
    {
        if (m_IndexOfTask < m_Tasks.Length)
        {
            Task task = m_Tasks[m_IndexOfTask];
            Debug.Log(string.Format("GA: Chromosome({0:00}): {1}", m_IndexOfTask + 1, task.ToString()));
            m_IndexOfTask++;

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
            fitness[i] = m_Tasks[i].Fitness(m_Kd, m_Ks, m_Ke, m_Kc);

        Debug.Log("GA: Evaluate fitness");

        // Procura o Maior Fitness
        int indexOfBestFitness = 0;
        float bestFitness = fitness[0];
        for (int i = 1; i < m_Tasks.Length; i++)
        {
            if (bestFitness < fitness[i])
            {
                indexOfBestFitness = i;
                bestFitness = fitness[i];
            }
        }

        Debug.Log("GA: Selection chromosome[" + indexOfBestFitness + "]: " + m_Tasks[indexOfBestFitness].ToString());
        return (Task)m_Tasks[indexOfBestFitness].Clone();
    }

    public void GeneticOperator(Task selectedTasks)
    {
        m_Tasks = new Task[m_NumberOfTask];
        m_Tasks[0] = selectedTasks;

        // Crossover e Mutação
        for (int i = 1; i < m_Tasks.Length; i++)
        {
            m_Tasks[i] = (Task)selectedTasks.Clone();
            m_Tasks[i].Distance = Mathf.Clamp(m_Tasks[i].Distance + Random.Range(-m_Md, m_Md), 0.0f, 1.0f);
            m_Tasks[i].Speed = Mathf.Clamp(m_Tasks[i].Speed + Random.Range(-m_Ms, m_Ms), 0.0f, 1.0f);
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