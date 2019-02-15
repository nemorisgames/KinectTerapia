using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatientInKinesiologist : MonoBehaviour {
    public UILabel namePatient;
    int pk_patient;
    PatientsList patientsList;
	// Use this for initialization
	void Start () {
		
	}

    public void SetInformation(int pk_patient, string name)
    {
        namePatient.text = name;
        this.pk_patient = pk_patient;
        patientsList = transform.root.GetComponent<PatientsList>();
    }

    public void ButtonPlay()
    {
        PlayerPrefs.SetInt("pk_patient", pk_patient);
        SceneManager.LoadScene("Configuration");
    }

    public void ButtonResults()
    {
        patientsList.CheckResults(pk_patient);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
