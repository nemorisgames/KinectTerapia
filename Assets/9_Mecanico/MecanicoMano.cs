using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecanicoMano : MonoBehaviour {
	public Transform part;
	MecanicoPieza pieza;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(part != null)
			part.transform.position = transform.position;
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "CPU" && part == null){
			pieza = c.GetComponent<MecanicoPieza>();
			if(!pieza.finished){
				part = c.transform;
				pieza.trapped = true;
			}
		}
	}
}
