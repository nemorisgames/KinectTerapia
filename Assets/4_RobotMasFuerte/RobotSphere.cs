﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSphere : MonoBehaviour {

	private void OnTriggerEnter(Collider c)
	{
		if(c.tag == "Player"){
			Debug.Log("hit");
		}
	}
}
