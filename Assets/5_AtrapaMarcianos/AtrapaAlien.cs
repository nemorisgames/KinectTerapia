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

	void OnTriggerEnter(Collider c){
		if(c.tag == "CPU" && !AtrapaMarcianosGM.Instance.detenido){
			AtrapaMarcianosGM.Instance.detenido = true;
			rb.useGravity = false;
			rb.velocity = Vector3.zero;
			transform.position = new Vector3(transform.position.x,-1,transform.position.z);
			c.GetComponent<TweenPosition>().Finish();
			c.transform.parent = this.transform;
			c.transform.position = new Vector3(c.transform.position.x,-2.5f,c.transform.position.z);
			c.transform.localPosition = new Vector3(0,c.transform.localPosition.y,0);
			AtrapaMarcianosGM.Instance.BlockPosition(true);
			AtrapaMarcianosGM.Instance.Atrapado(target);
		}
	}
}
