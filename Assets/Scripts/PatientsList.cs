using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientsList : MonoBehaviour {
    public GameObject patientInKinesiologist;
    public Transform scrollPanel;
    ArrayList patientsInKinesiologist;
	// Use this for initialization
	void Start () {
        patientsInKinesiologist = new ArrayList();
        TablePatient[] tablePatients = DatabaseManager.instance.GetPatientsInKinesiologist();
        int cont = 0;
        foreach(TablePatient t in tablePatients)
        {
            GameObject g = (GameObject)Instantiate(patientInKinesiologist);
            g.transform.parent = scrollPanel;
            g.transform.localPosition = new Vector3(0f, cont * -50f, 0f);
            g.transform.localScale = Vector3.one;
            PatientInKinesiologist p = g.GetComponent<PatientInKinesiologist>();
            p.SetInformation(t.pk_patient, t.name);
            patientsInKinesiologist.Add(p);
            cont++;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
