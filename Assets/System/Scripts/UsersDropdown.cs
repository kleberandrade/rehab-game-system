using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UsersDropdown : MonoBehaviour
{
    [SerializeField] private Dropdown m_Dropdown;
    [SerializeField] private Sprite m_ImageDefault;

    private void Start ()
    {
        m_Dropdown.onValueChanged.AddListener(OnPatientChanged);
        GetPatients();
        OnPatientChanged(0);
    }

    public void GetPatients()
    {
        List<User> patients = new List<User>();
        using (StreamReader reader = new StreamReader(Application.dataPath + "/user.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                User user = User.CreateFromJSON(line);
                if (SessionManager.Instance.GetTherapistName().Equals(user.TherapistName))
                    patients.Add(user);
            }
        }

        if (patients.Count > 0)
            SetPatients(patients);
        else
            m_Dropdown.options.Clear();

        SessionManager.Instance.AddPatients(patients);
    }

    public void SetPatients(List<User> patients)
    {
        m_Dropdown.options.Clear();
        foreach (User user in patients)
            m_Dropdown.options.Add(new Dropdown.OptionData(user.Name, m_ImageDefault));
        m_Dropdown.RefreshShownValue();
    }

    public void SetPatient(User patient)
    {
        SessionManager.Instance.AddPatient(patient);
        m_Dropdown.options.Add(new Dropdown.OptionData(patient.Name, m_ImageDefault));
    }

    public void OnPatientChanged(int value)
    {
        User patientSelected = SessionManager.Instance.GetPatientsByIndex(value);
        Debug.Log("Selected " + patientSelected.SaveToString());
        SessionManager.Instance.PatientSelected(patientSelected);
    }
}
