using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionMessage : MonoBehaviour {
    UILabel label;
	// Use this for initialization
	void Start () {
        label = GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
        label.text = CurrentUserTracker.CurrentUser != 0 ? ("Paciente Detectado") : ("Paciente No Detectado");
        label.color = CurrentUserTracker.CurrentUser != 0 ? (Color.white) : (Color.red);
    }
}
