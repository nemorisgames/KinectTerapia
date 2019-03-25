using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MecanicoSlot : MonoBehaviour {
	public MecanicoGM.PartType type;
	public MecanicoPieza pieza;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(pieza != null)
			return;
		if(other.tag == "CPU"){
			MecanicoPieza p = other.GetComponent<MecanicoPieza>();
			if(p.type == type){
				//Debug.Log("correcto");
				p.initPos = transform.position;
				p.finished = true;
				p.transform.rotation = transform.rotation;
				p.StopRotation();
				other.GetComponent<BoxCollider>().isTrigger = true;
				pieza = p;
				MecanicoGM.Instance.CorrectPart();
			}
			else
			{
				//Debug.Log("incorrecto");
			}
			p.trapped = false;
			MecanicoGM.Instance.mano.part = null;
		}
	}
}
