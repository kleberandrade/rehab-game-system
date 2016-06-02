using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.CrashLog;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SessionManager : Singleton<SessionManager>
{
    #region [ Variables ]
    private List<User> m_Patients = new List<User>();
    private Session m_Session = new Session();
    private bool m_IsHoming = false;
    #endregion

    private void Start()
    {
        m_IsHoming = false;
    }

    #region [ Login ]
    public void Login(string username, string password)
    {
        bool success = false;
        TextAsset userFile = Resources.Load<TextAsset>("user");
        string[] linesFromfile = userFile.text.Split("\n"[0]);

        for (int i = 0; i < linesFromfile.Length; i++)
        {
            string line = linesFromfile[i];
            User user = User.CreateFromJSON(line);
            if (username.Equals(user.Username) && password.Equals(user.Password))
            {
                m_Session.Therapist = user;
                success = true;
                break;
            }
        }

        if (success)
        {
            CrashReporting.Init("c31e08ef-2237-4e0e-a5a4-4f085e756c78", "1.0.0", m_Session.Therapist.Name);
            SceneManager.LoadScene("Hub");
        }
        else
        {
            SystemDialogBox.Instance.Show(
                "Falha no Login",
                "Verifique seu nome de usuário e/ou senha",
                new string[] { "Fechar" },
                new UnityAction[] { null });
        }
    }
    #endregion

    #region [ Create/Save New User ]
    public bool SaveNewUser(string data)
    {
        string fileName = string.Empty;

        try
        {
            fileName = Application.dataPath + "/Resources/user.txt";
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.Write("\n" + data);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
#endregion

    public string GetTherapistName()
    {
        return m_Session.Therapist.Username;
    }

    public void AddPatients(List<User> patients)
    {
        foreach (User patient in patients)
            AddPatient(patient);
    }

    public void AddPatient(User patient)
    {
        m_Patients.Add(patient);
    }

    public List<User> GetPatients()
    {
        return m_Patients;
    }

    public User GetPatientsByIndex(int index)
    {
        return m_Patients[index];
    }

    public void PatientSelected(User patient)
    {
        IsHoming = false;
        m_Session.Patient = patient;
    }

    public bool IsHoming
    {
        get { return m_IsHoming; }
        set { m_IsHoming = value; }
    }

    public void SetGame(Game game)
    {
        m_Session.Game = game;
    }

    public void SetDevice(Device device)
    {
        m_Session.Device = device;
    }

    public void AddPerformance(string metric, double value)
    {
        m_Session.Performances.Add(new Performance(metric, value));
    }

    public void AddTasks(Task task)
    {
        m_Session.Tasks.Add((Task)task.Clone());
    }

    public void NewPerformance()
    {
        m_Session.Performances = new List<Performance>();
    }

    public void NewTasks()
    {
        m_Session.Tasks = new List<Task>();
    }

    public void NewSession()
    {
        m_Session.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
        m_Session.Performances.Clear();
        m_Session.Tasks.Clear();
    }

    public void SaveSession()
    {
        if (!Directory.Exists("C:/RehabSystem"))
            Directory.CreateDirectory("C:/RehabSystem");

        if (!Directory.Exists("C:/RehabSystem/Save"))
            Directory.CreateDirectory("C:/RehabSystem/Save");

        string filename = string.Format("C:/RehabSystem/Save/session_{0}.txt", m_Session.Timestamp);
        using (StreamWriter writer = File.CreateText(filename))
            writer.WriteLine(m_Session.SaveToString());
    }
}

