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
    }

    // Update is called once per frame
    void Update () {
		
	}
}
