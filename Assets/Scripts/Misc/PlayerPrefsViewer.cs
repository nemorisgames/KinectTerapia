using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsViewer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnGUI()
    {
        GUILayout.Label("pk_kinesiologist " + PlayerPrefs.GetInt("pk_kinesiologist"));
        GUILayout.Label("pk_patient " + PlayerPrefs.GetInt("pk_patient"));
        GUILayout.Label("pk_game " + PlayerPrefs.GetInt("pk_game"));
        GUILayout.Label("handSelected " + PlayerPrefs.GetInt("handSelected"));

        GUILayout.Label("limitHorMin " + PlayerPrefs.GetFloat("limitHorMin"));
        GUILayout.Label("limitHorMax " + PlayerPrefs.GetFloat("limitHorMax"));
        GUILayout.Label("limitVerMin " + PlayerPrefs.GetFloat("limitVerMin"));
        GUILayout.Label("limitVerMax " + PlayerPrefs.GetFloat("limitVerMax"));
        GUILayout.Label("limitDepthMin " + PlayerPrefs.GetFloat("limitDepthMin"));
        GUILayout.Label("limitDepthMax " + PlayerPrefs.GetFloat("limitDepthMax"));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
