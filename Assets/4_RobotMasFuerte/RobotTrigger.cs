using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTrigger : MonoBehaviour {
	public bool triggerAltura = true;
	
	void OnTriggerEnter(Collider c){
		if(c.tag == "EsferaPoder"){
			if(triggerAltura)
				RobotFuerteGM.Instance.LevelWon();
			else
				RobotFuerteGM.Instance.LevelFailed();
		}
	}
}
