using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine.Utility;

public class RobotFuerteGM : MonoBehaviour {
	public static RobotFuerteGM Instance;
	public Transform brazo;
	public BoxCollider hitbox;
	public GameObject esferaPrefab;
	public Cinemachine.CinemachineVirtualCamera baseCam, moveCam;
	public Dificultad.Nivel dificultad;
	private Vector3 lastMousePos;
	private Vector3 mousePos;
	private List<Vector3> trackedMovement;
	public float armPos;
	public float ajusteY = 0.75f;
	public float ajusteFuerza = 0.75f;
	public float fuerza;
	private Vector3 rotation = new Vector3(0,-30,0);
	private RobotSphere target;
	public Transform triggerAltura;
	public Text triggerAlturaText;
	float height = 1;
	float baseHeight = 1.5f;

	void Awake(){
		if(Instance == null)
			Instance = this;
		
	}
	void Start () {
		trackedMovement = new List<Vector3>();
		moveCam.Priority = -11;
	}

	public void Init(){
		brazo.gameObject.SetActive(true);
		hitbox.enabled = true;
		target = ((GameObject)Instantiate(esferaPrefab,esferaPrefab.transform.position,esferaPrefab.transform.rotation)).GetComponent<RobotSphere>();
		moveCam.Follow = target.transform;
		moveCam.Priority = 11;
		Vector3 t = triggerAltura.position;
		switch(dificultad){
			case Dificultad.Nivel.facil:
				triggerAltura.position = new Vector3(t.x,baseHeight + 5,t.z);
				triggerAlturaText.text = "5m";
				target.Setup(5 + baseHeight);
			break;
			case Dificultad.Nivel.medio:
				triggerAltura.position = new Vector3(t.x,baseHeight + 10,t.z);
				triggerAlturaText.text = "10m";
				target.Setup(10 + baseHeight);
			break;
			case Dificultad.Nivel.dificil:
				triggerAltura.position = new Vector3(t.x,baseHeight + 15,t.z);
				triggerAlturaText.text = "15m";
				target.Setup(15 + baseHeight);
			break;
		}
	}

	public void Restart(){
		if(target != null)
			Destroy(target.gameObject);
		moveCam.transform.position = baseCam.transform.position;
		moveCam.Priority = -11;
		if(dificultad == Dificultad.Nivel.facil)
			dificultad = Dificultad.Nivel.medio;
		else if(dificultad == Dificultad.Nivel.medio)
			dificultad = Dificultad.Nivel.dificil;
		Init();
	}

	public void Hit(Rigidbody rb){
		StopMoving();
		Debug.Log(fuerza);
		rb.AddForce(new Vector3(0,fuerza * ajusteFuerza,0),ForceMode.Impulse);
		rb.useGravity = true;
		rb.GetComponent<SphereCollider>().enabled = false;
		hitbox.enabled = false;
	}

	public bool moving = false;
	float [] moveDist = new float[2];
	
	void Update () {
		if(Time.timeScale == 0)
			return;
		mousePos = Input.mousePosition;
		/* if(lastMousePos != null && Vector3.Distance(mousePos,lastMousePos) > 0.1f){
			trackedMovement.Add(mousePos);
		}
		else if(!stopping){
			StartCoroutine(stopDelay());
		}*/
		if(!moving && Vector3.Distance(mousePos,lastMousePos) > 0.05f){
			moving = true;
			moveDist[0] = armPos;
		}
		if(moving && Vector3.Distance(mousePos,lastMousePos) < 0.05f){
			StartCoroutine(stopDelay());
		}

		
		armPos = (mousePos.y / Screen.height) - ajusteY;
		/*if(armPos <= 1){
			rotation.x = (armPos * 90) / -1;
			brazo.eulerAngles = rotation;
		}*/
		Vector3 handPosition = Input.mousePosition - new Vector3(Screen.height / 2f, 0f, 0f);
        float horizontalViewportPosition = Camera.main.ScreenToViewportPoint(handPosition).y;
        float horizontalPosition = horizontalViewportPosition * (height*0.75f) - height/2;
		brazo.transform.position = new Vector3(0,horizontalPosition,0);
		if(moveCam.transform.position.y > 2 && brazo.gameObject.activeSelf)
			brazo.gameObject.SetActive(false);


		lastMousePos = mousePos;
		//fuerza = trackedMovement.Count;
		
	}

	IEnumerator stopDelay(){
		yield return new WaitForSeconds(0.1f);
		if(moving && Vector3.Distance(mousePos,lastMousePos) < 0.05f)
			StopMoving();
	}

	public void StopMoving(){
		moving = false;
		moveDist[1] = armPos;
		fuerza = (moveDist[1] - moveDist[0])*10;
	}

	public void LevelWon(){
		moveCam.Follow = null;
		GameManager.instance.FinishLevel();
	}

	public void LevelFailed(){
		moveCam.Follow = null;
		GameManager.instance.LevelFailed();
	}

}
