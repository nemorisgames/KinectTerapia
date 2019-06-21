using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KinesiologistList : MonoBehaviour
{
    public GameObject Kinesiologist, KineSelect;
    public GameObject Patient;
    private TablePatient currentPatient;
    private TableKinesiologist currentKine;
    public Transform scrollKinesiologistsPanel;
    public Transform scrollPatientsPanel;
    public GameObject editKine, editPatient, kineButtons, patientButtons;
    public UILabel editKineTitle, editPatientTitle;
    public UIInput editKineName, editKineUser, editKinePass;
    public UIInput editPatientName;
    public UILabel labelRellenarCampos;
    public UILabel verificarDeleteKine, verificarDeletePatient;
    ArrayList kinesiologistsList;
    ArrayList patientsList;
    bool isEditing = false;

    void Start ()
    {
        kinesiologistsList = new ArrayList ();
        patientsList = new ArrayList ();

        verificarDeletePatient.transform.parent.gameObject.SetActive (false);
        verificarDeleteKine.transform.parent.gameObject.SetActive (false);

        ShowKinesiologists ();
    }

    public void KinesiologistLinkPatient (int pk_kinesiologist)
    {
        TablePatient patientLink = new TablePatient ();
        patientLink.fk_kinesiologist = pk_kinesiologist;
        patientLink.pk_patient = currentPatient.pk_patient;
        patientLink.name = currentPatient.name;
        DatabaseManager.instance.SavePatient (patientLink, true);
        ShowPatients (true);
        currentPatient = null;
    }

    public void PatientSelectKine (TablePatient patient)
    {
        currentPatient = patient;
        ShowKinesiologists (false);
    }

    public void ButtonShowPatients ()
    {
        ShowPatients (true);
    }

    public void ButtonShowKine ()
    {
        ShowKinesiologists (true);
    }

    public void DeletePatient (TablePatient tp)
    {
        currentPatient = tp;
        verificarDeletePatient.text = "Eliminar Paciente " + tp.name + "?";
        verificarDeletePatient.transform.parent.gameObject.SetActive (true);
    }

    public void DeleteKine (TableKinesiologist tk)
    {
        currentKine = tk;
        verificarDeleteKine.text = "Eliminar Kinesiólogo " + tk.name + "?";
        verificarDeleteKine.transform.parent.gameObject.SetActive (true);
    }

    public void ConfirmDeletePatient ()
    {
        DatabaseManager.instance.DeletePatient (currentPatient);
        ShowPatients (true);
        CloseDeletePatient ();
    }

    public void ConfirmDeleteKine ()
    {
        DatabaseManager.instance.DeleteKine (currentKine);
        ShowKinesiologists (true);
        CloseDeleteKine ();
    }

    public void CloseDeletePatient ()
    {
        currentPatient = null;
        verificarDeletePatient.transform.parent.gameObject.SetActive (false);
    }

    public void CloseDeleteKine ()
    {
        currentKine = null;
        verificarDeleteKine.transform.parent.gameObject.SetActive (false);
    }

    public void ButtonAddPatient ()
    {
        EditPatient ();
    }

    public void EditPatient (TablePatient tp = null)
    {
        editPatientName.value = "";
        isEditing = false;
        if (tp != null)
        {
            isEditing = true;
            currentPatient = tp;
            editPatientTitle.text = "Editar Paciente";
            editPatientName.value = tp.name;
        }
        else
        {
            currentPatient = new TablePatient ();
            editPatientTitle.text = "Agregar Paciente";
        }
        ShowEditPatient ();
    }

    public void EditKine (TableKinesiologist tk = null)
    {
        editKineName.value = "";
        editKinePass.value = "";
        editKineUser.value = "";
        isEditing = false;
        if (tk != null)
        {
            isEditing = true;
            currentKine = tk;
            editKineTitle.text = "Editar Kinesiólogo";
            editKineName.value = tk.name;
            editKinePass.value = tk.password;
            editKineUser.value = tk.username;
        }
        else
        {
            currentKine = new TableKinesiologist ();
            editKineTitle.text = "Agregar Kinesiólogo";
        }
        ShowEditKine ();
    }

    public void SavePatient ()
    {
        if (editPatientName.value.Trim () == "")
        {
            labelRellenarCampos.gameObject.SetActive (true);
        }
        else
        {
            currentPatient.name = editPatientName.value;
            DatabaseManager.instance.SavePatient (currentPatient, isEditing);
            ShowPatients (true);
            currentPatient = null;
        }
    }

    public void SaveKine ()
    {
        if (editKineName.value.Trim () == "" || editKinePass.value.Trim () == "" || editKineUser.value.Trim () == "")
        {
            labelRellenarCampos.gameObject.SetActive (true);
        }
        else
        {
            currentKine.name = editKineName.value;
            currentKine.password = editKinePass.value;
            currentKine.username = editKineUser.value;
            DatabaseManager.instance.SaveKine (currentKine, isEditing);
            ShowKinesiologists (true);
            currentKine = null;
        }
    }

    public void ShowEditPatient ()
    {
        scrollKinesiologistsPanel.transform.parent.gameObject.SetActive (false);
        scrollPatientsPanel.transform.parent.gameObject.SetActive (false);
        labelRellenarCampos.gameObject.SetActive (false);
        editKine.SetActive (false);
        editPatient.SetActive (true);
    }

    public void ShowEditKine ()
    {
        scrollKinesiologistsPanel.transform.parent.gameObject.SetActive (false);
        scrollPatientsPanel.transform.parent.gameObject.SetActive (false);
        labelRellenarCampos.gameObject.SetActive (false);
        editKine.SetActive (true);
        editPatient.SetActive (false);
    }

    public void ShowPatients (bool b = true, int pk_kine = -1)
    {
        //Debug.Log (b);
        scrollKinesiologistsPanel.transform.parent.gameObject.SetActive (false);
        scrollPatientsPanel.transform.parent.gameObject.SetActive (true);
        labelRellenarCampos.gameObject.SetActive (false);
        editKine.SetActive (false);
        editPatient.SetActive (false);
        verificarDeleteKine.transform.parent.gameObject.SetActive (false);
        verificarDeletePatient.transform.parent.gameObject.SetActive (false);

        kineButtons.SetActive (false);
        patientButtons.SetActive (b);

        foreach (Transform t in scrollPatientsPanel)
        {
            Destroy (t.gameObject);
        }

        TablePatient[] tablePatients;

        if (pk_kine != -1 && pk_kine >= 0)
            tablePatients = DatabaseManager.instance.GetPatientsInKinesiologist (pk_kine);
        else
            tablePatients = DatabaseManager.instance.GetPatients ();

        if (tablePatients == null)
            return;

        for (int i = 0; i < tablePatients.Length; i++)
        {
            GameObject g = (GameObject) Instantiate (Patient);
            g.transform.parent = scrollPatientsPanel;
            g.transform.localPosition = new Vector3 (0f, -40 + i * -80f, 0f);
            g.transform.localScale = Vector3.one;
            PatientInKinesiologist p = g.GetComponent<PatientInKinesiologist> ();
            p.SetInformation (tablePatients[i]);
            patientsList.Add (p);
        }
    }

    public void ShowKinesiologists (bool b = true)
    {
        //Debug.Log (b);
        scrollKinesiologistsPanel.transform.parent.gameObject.SetActive (true);
        scrollPatientsPanel.transform.parent.gameObject.SetActive (false);
        labelRellenarCampos.gameObject.SetActive (false);
        editKine.SetActive (false);
        editPatient.SetActive (false);
        verificarDeleteKine.transform.parent.gameObject.SetActive (false);
        verificarDeletePatient.transform.parent.gameObject.SetActive (false);

        kineButtons.SetActive (b);
        patientButtons.SetActive (false);

        foreach (Transform t in scrollKinesiologistsPanel)
        {
            Destroy (t.gameObject);
        }

        TableKinesiologist[] tableKinesiologists = DatabaseManager.instance.GetKinesiologists ();
        int cont = 0;
        foreach (TableKinesiologist t in tableKinesiologists)
        {
            GameObject g = (GameObject) Instantiate (b ? Kinesiologist : KineSelect);
            g.transform.parent = scrollKinesiologistsPanel;
            g.transform.localPosition = new Vector3 (0f, cont * -50f, 0f);
            g.transform.localScale = Vector3.one;
            Kinesiologist p = g.GetComponent<Kinesiologist> ();
            p.SetInformation (t);
            kinesiologistsList.Add (p);
            cont++;
        }
    }
    /*
    public void CheckResults(int pk_patient)
    {
        scrollKinesiologistsPanel.transform.parent.gameObject.SetActive(false);
        scrollResultsPanel.transform.parent.gameObject.SetActive(true);
        scrollDetailsPanel.transform.parent.gameObject.SetActive(false);

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
            p.SetInformation(t.pk_game, t.game, t.score);
            print("game " + t.game);
            print("score " + t.score);
            patientsResults.Add(p);
            cont++;
        }
    }*/

    /*public void CheckDetails(int pk_patient, int pk_game)
    {
        scrollPatientsPanel.transform.parent.gameObject.SetActive(false);
        scrollResultsPanel.transform.parent.gameObject.SetActive(false);
        scrollDetailsPanel.transform.parent.gameObject.SetActive(true);

        List<TablePatientsDetails> tableDetails = DatabaseManager.instance.GetDetailsOnPatient(pk_patient, pk_game);
        int cont = 0;
        foreach (TablePatientsDetails t in tableDetails)
        {
            GameObject g = (GameObject)Instantiate(patientDetails);
            g.transform.parent = scrollDetailsPanel;
            g.transform.localPosition = new Vector3(0f, cont * -50f, 0f);
            g.transform.localScale = Vector3.one;
            PatientDetails p = g.GetComponent<PatientDetails>();
            string hand = "Ambas";
            if (t.hand == -1) hand = "Izquierda";
            else hand = "Derecha";
            p.SetInformation(t.pk_session, t.dateSession, t.score, hand, t.speedMin, t.speedMax);
            print("pk_session " + t.pk_session);
            print("dateSession " + t.dateSession);
            patientsDetails.Add(p);
            cont++;
        }
    }*/

    public void BackToLogin ()
    {
        SceneManager.LoadScene ("Login");
    }

    /*
    public void BackToPatients()
    {
        scrollPatientsPanel.transform.parent.gameObject.SetActive(true);
        scrollResultsPanel.transform.parent.gameObject.SetActive(false);
        scrollDetailsPanel.transform.parent.gameObject.SetActive(false);

        foreach(PatientResults p in patientsResults)
        {
            Destroy(p.gameObject);
        }
        patientsResults.Clear();
    }*/

    /*
    public void BackToResults()
    {
        scrollPatientsPanel.transform.parent.gameObject.SetActive(false);
        scrollResultsPanel.transform.parent.gameObject.SetActive(true);
        scrollDetailsPanel.transform.parent.gameObject.SetActive(false);

        foreach (PatientDetails p in patientsDetails)
        {
            Destroy(p.gameObject);
        }
        patientsDetails.Clear();
    }*/
}