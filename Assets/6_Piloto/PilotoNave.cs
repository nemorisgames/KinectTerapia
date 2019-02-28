using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotoNave : MonoBehaviour {
	Rigidbody rb;
	MeshRenderer mr;
	public bool invincible = false;
	public float knockBack = 1;

	void Awake () {
		rb = GetComponent<Rigidbody>();
		mr = GetComponent<MeshRenderer>();
	}

	IEnumerator Hit(){
		invincible = true;
		mr.material.color = Color.red;
		yield return new WaitForSeconds(0.5f);
		mr.material.color = Color.white;
		invincible = false;
	}

	void OnCollisionEnter(Collision c){
		if(!invincible && c.collider.tag == "CPU"){
			PilotoGM.Instance.health--;
			PilotoGM.Instance.SetHealth();
			if(PilotoGM.Instance.health > 0)
				StartCoroutine(Hit());
			else{
				GameManager.instance.LevelFailed();
				this.gameObject.SetActive(false);
				Time.timeScale = 0;
			}
				
		}
	}

	public Rigidbody rigidbody(){
		return rb;
	}
}
