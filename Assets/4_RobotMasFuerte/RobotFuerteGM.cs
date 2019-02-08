using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFuerteGM : MonoBehaviour {
	public Transform brazo;
	private Vector3 lastMousePos;
	private Vector3 mousePos;
	private List<Vector3> trackedMovement;
	public float armPos;
	public float ajusteY = 0.75f;
	private Vector3 rotation = new Vector3(0,-30,0);

	// Use this for initialization
	void Start () {
		trackedMovement = new List<Vector3>();
		//brazo.Rotate(90,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		mousePos = Input.mousePosition;
		if(lastMousePos != null && Vector3.Distance(mousePos,lastMousePos) > 0.5f){
			trackedMovement.Add(mousePos);
		}
		else{
			//if(trackedMovement.Count > 0)
				//Debug.Log(trackedMovement.Count);
			trackedMovement.Clear();
		}
		lastMousePos = mousePos;
		armPos = (mousePos.y / Screen.height) - ajusteY;
		//Debug.Log(armPos); 
		if(armPos <= 1){
			rotation.x = (armPos * 90) / -1;
			brazo.eulerAngles = rotation;
		}

	}
}
