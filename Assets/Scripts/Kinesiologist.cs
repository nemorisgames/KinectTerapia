using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kinesiologist : MonoBehaviour {
    public UILabel nameKinesiologist;
    int pk_patient;
    //PatientsList patientsList;
	// Use this for initialization
	void Start () {
		
	}

    public void SetInformation(int pk_patient, string name)
    {
        nameKinesiologist.text = name;
        this.pk_patient = pk_patient;
        //patientsList = transform.root.GetComponent<PatientsList>();
    }

    public void ButtonEdit()
    {
        PlayerPrefs.SetInt("pk_patient", pk_patient);
        SceneManager.LoadScene("Configuration");
    }

    public void ButtonDelete()
    {
        //patientsList.CheckResults(pk_patient);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
