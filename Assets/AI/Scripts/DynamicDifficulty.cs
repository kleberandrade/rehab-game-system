using System.Collections.Generic;
using UnityEngine;

public class DynamicDifficulty : MonoBehaviour
{
    #region [ Variables ]
    public float m_Kd = 0.3483f;
    public float m_Ks = 0.4679f;
    public float m_Ke = 0.6692f;
    public float m_Kc = 0.5319f;
    public float m_MutationRate = 0.25f;
    public int m_NumberOfInitialTasks = 20;
    public int m_NumberOfTasks = 5;
    private List<Task> m_Tasks = new List<Task>();
    private int m_IndexOfTask = 0;

    private float m_Difficulty = 0.0f;
    #endregion

    public void InitializeRandomTasks()
    {
        Debug.Log("GA: Initialize random population...");
        for (int i = 0; i < m_NumberOfInitialTasks; i++)
            m_Tasks.Add(new Task(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
    }

    public Task GetTask()
    {
        Task task = m_Tasks[m_IndexOfTask];
        Debug.Log(string.Format("GA: Get chromosome({0:00}): {1}", m_IndexOfTask, task.ToString()));
        return task;
    }

    private void NextPopulation()
    {
        // Avalia a população
        Debug.Log("GA: Evaluate fitness");
        for (int i = 0; i < m_Tasks.Count; i++)
        {
            m_Tasks[i].EvaluateFitness(m_Kd, m_Ks, m_Ke, m_Kc);
            Debug.Log(string.Format("GA: [{0}] = {1}", i, m_Tasks[i].ToString()));
            SessionManager.Instance.AddTasks(m_Tasks[i]);
        }

        // Seleciona o melhor alvo
        m_Tasks.Sort();
        Debug.Log(string.Format("GA: Selection chromosome {0}", m_Tasks[0].ToString()));
        Task selectedTask = (Task)m_Tasks[m_Tasks.Count - 1].Clone();

        // Criar a nova população
        m_Tasks.Clear();
        m_Tasks.Add(selectedTask);

        // Aplica o Crossover e Mutação
        Debug.Log("GA: Crossover and mutation...");
        
        for (int i = 1; i < m_NumberOfTasks; i++)
        {
            Task task = (Task)selectedTask.Clone();
            task.Distance = Mathf.Clamp(task.Distance + Random.Range(-m_MutationRate, m_MutationRate), 0.0f, 1.0f);
            task.Speed = Mathf.Clamp(task.Speed + Random.Range(-m_MutationRate, m_MutationRate), 0.0f, 1.0f);
            task.Error = 0.0f;
            task.Time = 0.0f;
            task.Fitness = 0.0f;
            Debug.Log(string.Format("GA: generate new chromosome({0:00}): {0}", i, task.ToString()));
            m_Tasks.Add(task);
        }

        m_IndexOfTask = 0;

        Debug.Log("GA: New population...");
    }

    public void EvaluationFitness(float error, float time)
    {
        m_Tasks[m_IndexOfTask].Error = error;
        m_Tasks[m_IndexOfTask].Time = time;
        m_Difficulty = (m_Tasks[m_IndexOfTask].Distance + m_Tasks[m_IndexOfTask].Speed) / 2.0f;

        m_IndexOfTask++;

        if (m_IndexOfTask > m_Tasks.Count - 1)
            NextPopulation();
    }

    public float Difficulty(int numberOfTargets)
    {
        return 100.0f * m_Difficulty / numberOfTargets ;
    }
}
