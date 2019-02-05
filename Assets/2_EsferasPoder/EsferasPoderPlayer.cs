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
	private Vector3 startPos;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody>();
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		/* speed = moveSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal");
		if(speed > 0 && transform.position.x >= width || speed < 0 && transform.position.x <= -width)
			return;
		transform.Translate(speed,0,0);*/
		if(Time.timeScale == 0f) return;
		Vector3 handPosition = Input.mousePosition - new Vector3(Screen.width / 2f, 0f, 0f);
        float horizontalViewportPosition = Camera.main.ScreenToViewportPoint(handPosition).x;
        float horizontalPosition = horizontalViewportPosition * width;
        transform.position = new Vector3(horizontalPosition, startPos.y, 0f);
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
