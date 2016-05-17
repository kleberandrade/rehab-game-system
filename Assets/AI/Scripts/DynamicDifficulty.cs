﻿using System.Text;
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
    // Constantes para o cromossomo
    public float m_MinDistante = 0.01f;
    public float m_MaxDistante = 0.5f;
    public float m_MinSpeed = 0.1f;
    public float m_MaxSpeed = 0.5f;
    // Número de tarefas
    public int m_NumberOfTask = 5;
    public int m_GainToNumberOfTask = 4;
    // Tarefas
    private Task[] m_Tasks;
    private int m_IndexOfTask = 0;

    public void Start()
    {
        InitializeRandomTasks();
    }

    public void InitializeRandomTasks()
    {
        m_IndexOfTask = -1;
        m_Tasks = new Task[m_NumberOfTask * m_GainToNumberOfTask];

        for (int i = 0; i < m_Tasks.Length; i++)
            m_Tasks[i] = new Task(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

        Debug.Log("GA: Initialize random population...");
    }

    public Task NextTask()
    {
        if (m_IndexOfTask < m_Tasks.Length - 1)
        {
            m_IndexOfTask++;
            Task task = m_Tasks[m_IndexOfTask];
            Debug.Log(string.Format("GA: Chromosome({0:00}): {1}", m_IndexOfTask + 1, task.ToString()));
            return task;
        }

        Task selectedTask = Selection();
        GeneticOperator(selectedTask);
        m_IndexOfTask = -1;
        return NextTask();
    }

    public Task Selection()
    {
        Debug.Log("GA: Evaluate fitness");
        // Calcula o fitness
        float[] fitness = new float[m_Tasks.Length];
        for (int i = 0; i < m_Tasks.Length; i++)
        {
            fitness[i] = m_Tasks[i].SetFitness(m_Kd, m_Ks, m_Ke, m_Kc);
            Debug.Log(string.Format("GA: [{0}] = {1}", i, m_Tasks[i].ToString()));
        }

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
        if (m_IndexOfTask < 0)
            return;

        m_Tasks[m_IndexOfTask].Error = error;
        m_Tasks[m_IndexOfTask].Time = time;
    }
}