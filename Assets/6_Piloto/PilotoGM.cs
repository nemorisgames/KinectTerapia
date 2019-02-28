using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotoGM : MonoBehaviour {
	public static PilotoGM Instance;
	public Dificultad.Nivel dificultad;
	public int health = 5;
	public int height, width;
	public float screenSpeed = 10;
	public Transform espacioNave, nave;
	private PilotoNave pilotoNave;
	[HideInInspector]
	public Vector3 lastPos;
	public GameObject [] vidas;

	// Use this for initialization
	void Awake () {
		if(Instance == null)
			Instance = this;
		pilotoNave = nave.GetComponent<PilotoNave>();
	}

	void Start(){
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeScale == 0)
			return;
		Vector3 handPosition = Input.mousePosition - new Vector3(Screen.width/2, Screen.height/2, 0f);
        Vector3 viewport = Camera.main.ScreenToViewportPoint(handPosition);
        float vPos = viewport.y * height;
		float hPos = viewport.x * width;
		//if(nave.localPosition.x != hPos || nave.localPosition.y != vPos)
		//	pilotoNave.rigidbody().AddForce(hPos - nave.localPosition.x,vPos - nave.localPosition.y,0);

		if(!pilotoNave.invincible)
			nave.localPosition = new Vector3(hPos,vPos,0);

		Vector3 screenPos = espacioNave.transform.position;
		espacioNave.position = new Vector3(screenPos.x + Time.deltaTime * screenSpeed,screenPos.y,screenPos.z);

		lastPos = nave.localPosition;
	}

	public void Init(){
		switch(dificultad){
			case Dificultad.Nivel.facil:
			health = 5;
			screenSpeed = 2;
			break;
			case Dificultad.Nivel.medio:
			health = 4;
			screenSpeed = 4;
			break;
			case Dificultad.Nivel.dificil:
			health = 3;
			screenSpeed = 6;
			break;
		}
		SetHealth();
		espacioNave.position = new Vector3(0,0,0);
	}

	public void NextLevel(){
		switch(dificultad){
			case Dificultad.Nivel.facil:
			dificultad = Dificultad.Nivel.medio;
			break;
			case Dificultad.Nivel.medio:
			dificultad = Dificultad.Nivel.dificil;
			break;
		}
		Init();
	}

	public void SetHealth(){
		foreach(GameObject o in vidas)
			o.SetActive(false);
		for(int i = 0; i < health; i++)
			vidas[i].SetActive(true);
	}
}
