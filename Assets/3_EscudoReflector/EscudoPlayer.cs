using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscudoPlayer : MonoBehaviour {
	[HideInInspector]
	public float moveSpeed = 2f;
	Rigidbody rb;
	[HideInInspector]
	public float speed;
	[HideInInspector]
	public float width = 7f;


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
}
