using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SessionManager : Singleton<SessionManager>
{
    #region [ Variables ]
    private List<User> m_Patients = new List<User>();
    private Session m_Session = new Session();
    private bool m_IsHoming = false;
    #endregion

    #region [ Login ]
    public void Login(string username, string password)
    {
        bool success = false;
        using (StreamReader reader = new StreamReader(Application.dataPath + "/user.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                User user = User.CreateFromJSON(line);
                if (username.Equals(user.Username) && password.Equals(user.Password))
                {
                    m_Session.Therapist = user;
                    success = true;
                    break;
                }
            }
        }

        if (success)
        {
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
        try
        {
            using (StreamWriter writer = new StreamWriter(Application.dataPath + "/user.txt", true))
            {
                writer.WriteLine(data);
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
        foreach (User user in patients)
            AddPatient(user);
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
        m_Session.Patient = patient;
    }

    public bool IsHoming
    {
        get { return m_IsHoming; }
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

    public void NewPerformance()
    {
        m_Session.Performances = new List<Performance>();
    }

    public void SaveSession()
    {
        string filename = string.Format("{0}/session_{1}.txt", Application.dataPath, m_Session.Timestamp);

        using (FileStream file = File.Create(filename))
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine(m_Session.SaveToString());
            }
        }
    }
}

