using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatientDetails : MonoBehaviour {
    public UILabel dateLabel;
    public UILabel scoreLabel;
    public UILabel handLabel;
    public UILabel vMinLabel;
    public UILabel vMaxLabel;
    public UILabel vMeanLabel;
    int pk_session;
    PatientsList patientsList;
    // Use this for initialization
    void Start () {
		
	}

    public void SetInformation(int pk_session, string date, int score, string hand, float vMin, float vMax)
    {
        this.pk_session = pk_session;
        this.dateLabel.text = "" + date;
        this.scoreLabel.text = "" + score;
        this.handLabel.text = "" + hand;
        this.vMinLabel.text = "" + vMin;
        this.vMaxLabel.text = "" + vMax;
        this.vMeanLabel.text = "" + ((vMin + vMax) / 2f);
        patientsList = transform.root.GetComponent<PatientsList>();
    }

    public void ButtonHeatmap()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
