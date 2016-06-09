using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsersDropdown : MonoBehaviour
{
    public Dropdown m_Dropdown;
    public Sprite m_ImageDefault;

    private void Start ()
    {
        m_Dropdown.onValueChanged.AddListener(OnPatientChanged);
        GetPatients();
        OnPatientChanged(0);
    }

    public void GetPatients()
    {
        List<User> patients = new List<User>();
        TextAsset userFile = Resources.Load<TextAsset>("user");
        string[] linesFromfile = userFile.text.Split("\n"[0]);
        string therapistName = SessionManager.Instance.GetTherapistName();

        for (int i = 0; i < linesFromfile.Length; i++)
        {
            string line = linesFromfile[i];
            User user = User.CreateFromJSON(line);        
            if (therapistName.Equals(user.TherapistName))
                patients.Add(user);
        }

        patients.Sort();

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
        SessionManager.Instance.PatientSelected(patientSelected);
    }
}
