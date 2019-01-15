using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsferasPoderPlayer : MonoBehaviour {
	public float moveSpeed = 2f;
	private EsferasPoderGM gm;
	Rigidbody rb;
	[HideInInspector]
	public float speed;
	//[HideInInspector]
	public float width = 5;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		speed = moveSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal");
		if(speed > 0 && transform.position.x >= width || speed < 0 && transform.position.x <= -width)
			return;
		transform.Translate(speed,0,0);
	}

	void OnCollisionEnter(Collision col){
		Collider c = col.collider;
		if(c.tag == "EsferaPoder"){
			Destroy(c.gameObject);
			gm.GetEsfera();
		}
	}

	public void setGm(EsferasPoderGM gm){
		this.gm = gm;
	}
}
