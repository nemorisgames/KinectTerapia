using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MecanicoSlot : MonoBehaviour {
	public MecanicoGM.PartType type;
	public MecanicoPieza pieza;

	// Use this for initialization
	void Start () {
        Renderer rend = GetComponent<Renderer>();
        //Set the main Color of the Material to green
        //rend.material.shader = Shader.Find("_Color");
        //switch (type)
        //{
        //    case MecanicoGM.PartType.None:
        //        rend.material.SetColor("_Color", Color.white);
        //        rend.material.SetColor("_EmissionColor", Color.black);
        //        break;
        //    case MecanicoGM.PartType.Part1:
        //        rend.material.SetColor("_Color", Color.red);
        //        rend.material.SetColor("_EmissionColor", Color.red * 2f);
        //        break;
        //    case MecanicoGM.PartType.Part2:
        //        rend.material.SetColor("_Color", Color.blue);
        //        rend.material.SetColor("_EmissionColor", Color.blue * 2f);
        //        break;
        //    case MecanicoGM.PartType.Part3:
        //        rend.material.SetColor("_Color", Color.yellow);
        //        rend.material.SetColor("_EmissionColor", Color.yellow * 2f);
        //        break;
        //    case MecanicoGM.PartType.Part4:
        //        rend.material.SetColor("_Color", Color.green);
        //        rend.material.SetColor("_EmissionColor", Color.green * 2f);
        //        break;
        //    case MecanicoGM.PartType.Part5:
        //        rend.material.SetColor("_Color", Color.cyan);
        //        rend.material.SetColor("_EmissionColor", Color.cyan * 2f);
        //        break;
        //}

        switch (type)
        {
            case MecanicoGM.PartType.None:
                rend.material.SetColor("_Color", new Color(0.1f, 0.1f, 0.1f));
                rend.material.SetColor("_EmissionColor", Color.black);
                break;
            case MecanicoGM.PartType.Part1:
                rend.material.SetColor("_Color", new Color(1f, 0.3f, 0.3f, 0.3f));
                rend.material.SetColor("_EmissionColor", new Color(1f, 0.1f, 0.1f) * 2f);
                break;
            case MecanicoGM.PartType.Part2:
                rend.material.SetColor("_Color", new Color(0.1f, 0.1f, 1f, 0.3f));
                rend.material.SetColor("_EmissionColor", new Color(0.1f, 0.1f, 1f) * 2f);
                break;
            case MecanicoGM.PartType.Part3:
                rend.material.SetColor("_Color", new Color(1f, 1f, 0.3f, 0.3f));
                rend.material.SetColor("_EmissionColor", new Color(1f, 1f, 0.1f) * 2f);
                break;
            case MecanicoGM.PartType.Part4:
                rend.material.SetColor("_Color", new Color(0.3f, 1f, 0.3f, 0.3f));
                rend.material.SetColor("_EmissionColor", new Color(0.1f, 1f, 0.1f) * 2f);
                break;
            case MecanicoGM.PartType.Part5:
                rend.material.SetColor("_Color", new Color(0.3f, 1f, 1f, 0.3f));
                rend.material.SetColor("_EmissionColor", new Color(0.1f, 1f, 1f) * 2f);
                break;
        }
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
