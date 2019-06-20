using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatientsList : MonoBehaviour
{
    public GameObject patientInKinesiologist;
    public GameObject patientResult;
    public GameObject patientDetails;
    public Transform scrollPatientsPanel;
    public Transform scrollResultsPanel;
    public Transform scrollDetailsPanel;
    ArrayList patientsInKinesiologist;
    ArrayList patientsResults;
    ArrayList patientsDetails;
    // Use this for initialization
    void Start ()
    {
        patientsInKinesiologist = new ArrayList ();
        patientsResults = new ArrayList ();
        patientsDetails = new ArrayList ();
        TablePatient[] tablePatients = DatabaseManager.instance.GetPatientsInKinesiologist ();
        int cont = 0;
        foreach (TablePatient t in tablePatients)
        {
            GameObject g = (GameObject) Instantiate (patientInKinesiologist);
            g.transform.parent = scrollPatientsPanel;
            g.transform.localPosition = new Vector3 (0f, cont * -50f, 0f);
            g.transform.localScale = Vector3.one;
            PatientInKinesiologist p = g.GetComponent<PatientInKinesiologist> ();
            p.SetInformation (t);
            patientsInKinesiologist.Add (p);
            cont++;
        }
    }

    public void CheckResults (int pk_patient)
    {
        scrollPatientsPanel.transform.parent.gameObject.SetActive (false);
        scrollResultsPanel.transform.parent.gameObject.SetActive (true);
        scrollDetailsPanel.transform.parent.gameObject.SetActive (false);

        PlayerPrefs.SetInt ("pk_patient", pk_patient);
        ArrayList PatientResults = new ArrayList ();
        List<TablePatientsResult> tableResults = DatabaseManager.instance.GetResultsOnPatient (pk_patient);
        int cont = 0;
        foreach (TablePatientsResult t in tableResults)
        {
            GameObject g = (GameObject) Instantiate (patientResult);
            g.transform.parent = scrollResultsPanel;
            g.transform.localPosition = new Vector3 (0f, cont * -50f, 0f);
            g.transform.localScale = Vector3.one;
            PatientResults p = g.GetComponent<PatientResults> ();
            p.SetInformation (t.pk_game, t.game, t.score);
            print ("game " + t.game);
            print ("score " + t.score);
            patientsResults.Add (p);
            cont++;
        }
    }

    public void CheckDetails (int pk_patient, int pk_game)
    {
        scrollPatientsPanel.transform.parent.gameObject.SetActive (false);
        scrollResultsPanel.transform.parent.gameObject.SetActive (false);
        scrollDetailsPanel.transform.parent.gameObject.SetActive (true);

        List<TablePatientsDetails> tableDetails = DatabaseManager.instance.GetDetailsOnPatient (pk_patient, pk_game);
        int cont = 0;
        foreach (TablePatientsDetails t in tableDetails)
        {
            GameObject g = (GameObject) Instantiate (patientDetails);
            g.transform.parent = scrollDetailsPanel;
            g.transform.localPosition = new Vector3 (0f, cont * -50f, 0f);
            g.transform.localScale = Vector3.one;
            PatientDetails p = g.GetComponent<PatientDetails> ();
            string hand = "Ambas";
            if (t.hand == -1) hand = "Izquierda";
            else hand = "Derecha";
            p.SetInformation (t.pk_session, t.dateSession, t.score, hand, t.speedMin, t.speedMax);
            print ("pk_session " + t.pk_session);
            print ("dateSession " + t.dateSession);
            patientsDetails.Add (p);
            cont++;
        }
    }

    public void BackToLogin ()
    {
        SceneManager.LoadScene ("Login");
    }

    public void BackToPatients ()
    {
        scrollPatientsPanel.transform.parent.gameObject.SetActive (true);
        scrollResultsPanel.transform.parent.gameObject.SetActive (false);
        scrollDetailsPanel.transform.parent.gameObject.SetActive (false);

        foreach (PatientResults p in patientsResults)
        {
            Destroy (p.gameObject);
        }
        patientsResults.Clear ();
    }

    public void BackToResults ()
    {
        scrollPatientsPanel.transform.parent.gameObject.SetActive (false);
        scrollResultsPanel.transform.parent.gameObject.SetActive (true);
        scrollDetailsPanel.transform.parent.gameObject.SetActive (false);

        foreach (PatientDetails p in patientsDetails)
        {
            Destroy (p.gameObject);
        }
        patientsDetails.Clear ();
    }

    // Update is called once per frame
    void Update ()
    {

    }
}