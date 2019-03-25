using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MecanicoPieza : MonoBehaviour {
	Rigidbody rb;
	public MecanicoGM.PartType type;
	[HideInInspector]
	public Vector3 initPos;
	public bool trapped = false;
	public bool finished = false;

	void Awake(){
		rb = GetComponent<Rigidbody>();
	}
	void Start () {
		rb.AddTorque(Random.Range(-2f,2f),Random.Range(-2f,2f),Random.Range(-2f,2f),ForceMode.Force);
		//type = MecanicoGM.Instance.RandomPartType();
	}

	void LateUpdate(){
		if(!trapped)
			Reset();
	}
	
	public void Reset(){
		transform.position = initPos;
	}

	public void StopRotation(){
		rb.angularVelocity = Vector3.zero;
	}

	
}
