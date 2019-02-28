using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrapaAlien : MonoBehaviour {
	Rigidbody rb;
	public float target;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision col){
		if(col.collider.tag != "Untagged" ){
			AtrapaMarcianosGM.Instance.detenido = true;
			AtrapaMarcianosGM.Instance.BlockPosition(true);
		}
	}

	/* void OnTriggerEnter(Collider c){
		if(c.tag == "CPU"){
			AtrapaMarcianosGM.Instance.detenido = true;
			rb.useGravity = false;
			//c.transform.parent = this.transform;
		}
	}*/
}
