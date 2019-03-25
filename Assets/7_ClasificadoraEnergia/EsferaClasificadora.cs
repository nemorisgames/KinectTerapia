using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsferaClasificadora : MonoBehaviour {
	public Vector2 speed = new Vector2(0,5);
	public bool rojo = true;
	public bool move = true;
	public bool forked = false;
	private MeshRenderer mr;

	void Awake(){
		mr = GetComponent<MeshRenderer>();
	}

	public void SetColor(){
		if(rojo)
			mr.material.color = Color.red;
		else
			mr.material.color = Color.blue;
	}
	
	// Update is called once per frame
	void Update () {
		if(move)
			transform.position = new Vector3(transform.position.x + speed.x * Time.deltaTime, transform.position.y, transform.position.z - speed.y * Time.deltaTime);
		
		if(transform.position.z <= 4f && !forked){
			if(ClasificadoraGM.Instance.openDoor != 0){
				forked = true;
				speed.x = 5 * ClasificadoraGM.Instance.openDoor;
				speed.y = 0;
			}
		}

		if(transform.position.z <= 2f && move){
			move = false;
		}

		if(Mathf.Abs(transform.position.x) >= ClasificadoraGM.Instance.rightExit.position.x || transform.position.y < -3){
			if((transform.position.x > 0 && !rojo) || (transform.position.x < 0 && rojo)){
				if(transform.position.z > 3f && transform.position.z < 4f)
					ClasificadoraGM.Instance.CatchBall(true);
				else
					ClasificadoraGM.Instance.CatchBall(false);
			}
			else
				ClasificadoraGM.Instance.CatchBall(false);
			Destroy(this.gameObject);
		}			
	}

	void OnCollisionEnter(Collision col){
		if(col.collider.tag == "CPU"){
			ClasificadoraGM.Instance.CatchBall(false);
			Destroy(this.gameObject);
		}
		
	}
}
