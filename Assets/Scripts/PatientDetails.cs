using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatientDetails : MonoBehaviour
{
    public UILabel dateLabel;
    public UILabel scoreLabel;
    public UILabel handLabel;
    public UILabel vMinLabel;
    public UILabel vMaxLabel;
    public UILabel vMeanLabel;
    int pk_session;
    string heatmap;
    PatientsList patientsList;

    public void SetInformation (int pk_session, string date, int score, string hand, float vMin, float vMax)
    {
        this.pk_session = pk_session;
        this.dateLabel.text = "" + date;
        this.scoreLabel.text = "" + score;
        this.handLabel.text = "" + hand;
        this.vMinLabel.text = "" + vMin;
        this.vMaxLabel.text = "" + vMax;
        this.vMeanLabel.text = "" + ((vMin + vMax) / 2f);
        patientsList = transform.root.GetComponent<PatientsList> ();
    }

    public void SetInformation (TablePatientsDetails details, string hand)
    {
        patientsList = transform.root.GetComponent<PatientsList> ();
        pk_session = details.pk_session;
        dateLabel.text = details.dateSession;
        scoreLabel.text = details.score.ToString ();
        handLabel.text = hand;
        float vMax = (Mathf.Sqrt (details.speedMax));
        vMaxLabel.text = ((Mathf.Round (vMax * 100f)) / 100f).ToString ();
        float vMean = Mathf.Sqrt (Mathf.Abs (details.speedMin));
        vMeanLabel.text = ((Mathf.Round (vMean * 100f)) / 100f).ToString ();
        heatmap = details.heatmap;
    }

    public void ButtonHeatmap ()
    {
        patientsList.CheckHeatmap (heatmap);
    }
}