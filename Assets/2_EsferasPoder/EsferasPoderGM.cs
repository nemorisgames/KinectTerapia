using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsferasPoderGM : MonoBehaviour {
	public GameObject esferaPrefab;
	public EsferasPoderPlayer player;
	public Transform generador;
	List<MeshRenderer> generadorColor;
	List<GameObject> esferas;
	public GameObject carga;
	public int width = 7;
	public Dificultad.Nivel dificultad;
	public int spheresCaught = 0;
	public int sphereCount = 20;
	private int sphereTarget = 5;
	private float sphereTime = 3;
	System.Random random;
	bool gameOver = false;
	IEnumerator next;

	void Awake(){
		player = GameObject.FindObjectOfType<EsferasPoderPlayer>().GetComponent<EsferasPoderPlayer>();
		if(player != null){
			player.setGm(this);
			player.width = width;
		}
			
	}

	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Init () {
		generadorColor = new List<MeshRenderer>();
		esferas = new List<GameObject>();
		spheresCaught = 0;
		gameOver = false;
		random = new System.Random();
		switch(dificultad){
			case Dificultad.Nivel.facil:
				sphereTarget = 5;
				sphereTime = 2;
			break;
			case Dificultad.Nivel.medio:
				sphereTarget = 10;
				sphereTime = 1.5f;
			break;
			case Dificultad.Nivel.dificil:
				sphereTarget = 15;
				sphereTime = 1;
			break;
		}
		next = nextSphere();
		StartCoroutine(next);
	}

	IEnumerator nextSphere(){
		if(gameOver)
			yield break;
		Vector3 pos = new Vector3 (random.Next(-width,width),transform.position.y,transform.position.z);
		GameObject go = (GameObject)Instantiate(esferaPrefab,pos,Quaternion.identity,transform);
		esferas.Add(go);
		yield return new WaitForSeconds(sphereTime);
		sphereCount--;
		if(sphereCount > 0){
			next = nextSphere();
			StartCoroutine(next);
		}
	}

	public void GetEsfera(){
		if(player == null)
			return;
		GameObject go = (GameObject)Instantiate(carga,Vector3.zero,Quaternion.identity,generador);
		go.transform.localPosition = new Vector3(0,spheresCaught,0);
		generadorColor.Add(go.GetComponent<MeshRenderer>());
		spheresCaught++;
		SetColor(spheresCaught);
		if(spheresCaught >= sphereTarget){
			foreach(GameObject g in esferas)
				Destroy(g);
			Debug.Log("end game");
			StopCoroutine(next);
			gameOver = true;
		}
	}

	void SetColor(float current){
		if(current < (sphereTarget/2f)){
			foreach(MeshRenderer mr in generadorColor)
				//if(mr.material.color != Color.red)
					mr.material.color = Color.red;
		}
		else if(current >= (sphereTarget/2f) && current < sphereTarget){
			foreach(MeshRenderer mr in generadorColor)
				//if(mr.material.color != Color.yellow)
					mr.material.color = Color.yellow;
		}
		else if(current >= sphereTarget){
			foreach(MeshRenderer mr in generadorColor)
				//if(mr.material.color != Color.green)
					mr.material.color = Color.green;
		}
	}
}
