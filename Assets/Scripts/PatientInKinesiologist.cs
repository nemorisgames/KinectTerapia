using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatientInKinesiologist : MonoBehaviour
{
    public UILabel namePatient, nameKine;
    int pk_patient;
    int fk_kinesiologist;
    PatientsList patientsList;
    private TablePatient patient;

    public void SetInformation (TablePatient tp)
    {
        patient = tp;
        namePatient.text = tp.name;
        pk_patient = tp.pk_patient;
        fk_kinesiologist = tp.fk_kinesiologist;
        TableKinesiologist kine = DatabaseManager.instance.GetKinesiologist (fk_kinesiologist);
        if (kine != null)
            nameKine.text = kine.name;
        patientsList = transform.root.GetComponent<PatientsList> ();
    }

    public void ButtonPlay ()
    {
        PlayerPrefs.SetInt ("pk_patient", pk_patient);
        SceneManager.LoadScene ("Configuration");
    }

    public void ButtonResults ()
    {
        patientsList.CheckResults (pk_patient);
    }

    public void ButtonKine ()
    {
        KinesiologistList kl = transform.root.GetComponent<KinesiologistList> ();
        if (kl != null)
        {
            kl.PatientSelectKine (patient);
        }
    }

    public void ButtonDelete ()
    {
        KinesiologistList kl = transform.root.GetComponent<KinesiologistList> ();
        if (kl != null)
        {
            kl.DeletePatient (patient);
        }
    }

    public void ButtonEdit ()
    {
        KinesiologistList kl = transform.root.GetComponent<KinesiologistList> ();
        if (kl != null)
        {
            kl.EditPatient (patient);
        }
    }
}