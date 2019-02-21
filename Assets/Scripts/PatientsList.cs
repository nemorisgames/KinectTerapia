using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientsList : MonoBehaviour {
    public GameObject patientInKinesiologist;
    public GameObject patientResult;
    public Transform scrollPatientsPanel;
    public Transform scrollResultsPanel;
    ArrayList patientsInKinesiologist;
	// Use this for initialization
	void Start () {
        patientsInKinesiologist = new ArrayList();
        TablePatient[] tablePatients = DatabaseManager.instance.GetPatientsInKinesiologist();
        int cont = 0;
        foreach(TablePatient t in tablePatients)
        {
            GameObject g = (GameObject)Instantiate(patientInKinesiologist);
            g.transform.parent = scrollPatientsPanel;
            g.transform.localPosition = new Vector3(0f, cont * -50f, 0f);
            g.transform.localScale = Vector3.one;
            PatientInKinesiologist p = g.GetComponent<PatientInKinesiologist>();
            p.SetInformation(t.pk_patient, t.name);
            patientsInKinesiologist.Add(p);
            cont++;
        }
    }
    
    public void CheckResults(int pk_patient)
    {
        scrollPatientsPanel.gameObject.SetActive(false);
        scrollResultsPanel.gameObject.SetActive(true);

        PlayerPrefs.SetInt("pk_patient", pk_patient);
        ArrayList PatientResults = new ArrayList();
        List<TablePatientsResult> tableResults = DatabaseManager.instance.GetResultsOnPatient(pk_patient);
        int cont = 0;
        foreach (TablePatientsResult t in tableResults)
        {
            GameObject g = (GameObject)Instantiate(patientResult);
            g.transform.parent = scrollResultsPanel;
            g.transform.localPosition = new Vector3(0f, cont * -50f, 0f);
            g.transform.localScale = Vector3.one;
            PatientResults p = g.GetComponent<PatientResults>();
            p.SetInformation(t.pk_session, );
            print("game " + t.game);
            print("score " + t.score);
            //patientsInKinesiologist.Add(p);
            cont++;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
