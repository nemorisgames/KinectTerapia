﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSphere : MonoBehaviour {
	Rigidbody rb;
	public bool launched = false;
	public float height = 0;
	public float currentHeight = 0;
	private float lastHeight = 0;

	void Awake(){
		rb = GetComponent<Rigidbody>();
	}

	void OnTriggerEnter(Collider c)
	{
		if(launched)
			return;
		if(c.tag == "Player"){
			HitSphere();
		}
	}

	void Update(){
		if(launched){
			if(transform.position.y >= height){
				launched = false;
				GameManager.instance.SetScore((int)(height - 1.5f) * 200);
				RobotFuerteGM.Instance.LevelWon();
			}
			else{
				if(transform.position.y > lastHeight)
				{
					currentHeight = Mathf.Clamp(transform.position.y - 1.5f,0,height - 1.5f);
					GameManager.instance.SetScore((int)(currentHeight * 100));
				}
			}
			lastHeight = transform.position.y;

			if(transform.position.y <= 0){
				RobotFuerteGM.Instance.LevelFailed();
			}
		}
	}

	public void Setup(float h){
		height = h;
	}

	public void HitSphere(){
		launched = true;
		RobotFuerteGM.Instance.Hit(rb);
	}
}
