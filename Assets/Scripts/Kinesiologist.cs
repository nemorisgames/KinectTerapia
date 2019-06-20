using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kinesiologist : MonoBehaviour
{
    public UILabel nameKinesiologist, numPatients;
    int pk_kinesiologist;
    private TableKinesiologist kine;

    public void SetInformation (TableKinesiologist tk)
    {
        nameKinesiologist.text = tk.name;
        pk_kinesiologist = tk.pk_kinesiologist;
        kine = tk;
        if (numPatients != null)
        {
            TablePatient[] tp = DatabaseManager.instance.GetPatientsInKinesiologist (pk_kinesiologist);
            if (tp != null)
                numPatients.text = (tp.Length).ToString ();
            else
                numPatients.text = "0";
        }

    }

    public void ButtonEdit ()
    {
        KinesiologistList kl = transform.root.GetComponent<KinesiologistList> ();
        if (kl != null)
        {
            kl.EditKine (kine);
        }
    }

    public void ButtonDelete ()
    {
        KinesiologistList kl = transform.root.GetComponent<KinesiologistList> ();
        if (kl != null)
        {
            kl.DeleteKine (kine);
        }
    }

    public void ButtonSelect ()
    {
        KinesiologistList kl = transform.root.GetComponent<KinesiologistList> ();
        if (kl != null)
        {
            kl.KinesiologistLinkPatient (pk_kinesiologist);
        }
    }

    public void ButtonPatients ()
    {
        KinesiologistList kl = transform.root.GetComponent<KinesiologistList> ();
        if (kl != null)
        {
            kl.ShowPatients (true, kine.pk_kinesiologist);
        }
    }
}