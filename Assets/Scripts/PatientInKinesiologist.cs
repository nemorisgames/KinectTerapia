using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatientInKinesiologist : MonoBehaviour {
    public UILabel namePatient;
    int pk_patient;
	// Use this for initialization
	void Start () {
		
	}

    public void SetInformation(int pk_patient, string name)
    {
        namePatient.text = name;
        this.pk_patient = pk_patient;
    }

    public void ButtonPlay()
    {
        PlayerPrefs.SetInt("pk_patient", pk_patient);
        SceneManager.LoadScene("GameGrid");

    }

    public void ButtonResults()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
