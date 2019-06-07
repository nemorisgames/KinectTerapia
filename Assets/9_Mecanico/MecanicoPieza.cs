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
        Renderer rend = GetComponent<Renderer>();
        //Set the main Color of the Material to green
        //rend.material.shader = Shader.Find("_Color");
        switch (type)
        {
            case MecanicoGM.PartType.None:
                rend.material.SetColor("_Color", new Color(0.1f, 0.1f, 0.1f));
                //rend.material.SetColor("_EmissionColor", Color.black);
                break;
            case MecanicoGM.PartType.Part1:
                rend.material.SetColor("_Color", Color.red);
                //rend.material.SetColor("_EmissionColor", new Color(1f, 0.3f, 0.3f) * 2f);
                break;
            case MecanicoGM.PartType.Part2:
                rend.material.SetColor("_Color", Color.blue);
                //rend.material.SetColor("_EmissionColor", new Color(0.3f, 0.3f, 1f) * 2f);
                break;
            case MecanicoGM.PartType.Part3:
                rend.material.SetColor("_Color", Color.yellow);
                //rend.material.SetColor("_EmissionColor", new Color(0.3f, 1f, 1f) * 2f);
                break;
            case MecanicoGM.PartType.Part4:
                rend.material.SetColor("_Color", Color.green);
                //rend.material.SetColor("_EmissionColor", new Color(0.3f, 1f, 0.3f) * 2f);
                break;
            case MecanicoGM.PartType.Part5:
                rend.material.SetColor("_Color", Color.cyan);
                //rend.material.SetColor("_EmissionColor", new Color(1f, 1f, 0.3f) * 2f);
                break;
        }
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
