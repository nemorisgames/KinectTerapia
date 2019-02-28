using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrapaMarcianosGM : MonoBehaviour {
	public static AtrapaMarcianosGM Instance;
	public GameObject hand;
	public GameObject manoRef;
	public GameObject alienPrefab;
	public Dificultad.Nivel dificultad;
	public int atrapados = 0;
	public int maxMoving = 2;
	public int moving = 0;
	public int range = 3;
	public Vector2 moveSpeed;
	public float launchForce;
	public float enemyPosZ = 8.5f;
	Rigidbody rb;
	public bool launched = false;
	public bool detenido = false;
	private Vector3 handPos;
	private int width = 8;
	private int height = 6;
	private float [] posiciones;
	public List<TweenPosition> aliens = new List<TweenPosition>();
	public bool [] aliensMoving = new bool[]{};
	float blocked = 100;
	private AtrapaAlien mano;

	// Use this for initialization
	void Awake () {
		if(Instance == null)
			Instance = this;
		rb = hand.GetComponent<Rigidbody>();
		mano = hand.GetComponent<AtrapaAlien>();
		handPos = hand.transform.position;
		Random.InitState(System.DateTime.Now.Minute * System.DateTime.Now.Second);
	}

	void Start(){
		Init();
		RestartHand();
	}

	void Init(){
		atrapados = 0;
		switch(dificultad){
			case Dificultad.Nivel.facil:
				range = 3;
				posiciones = new float[]{-5,0,5};
			break;
			case Dificultad.Nivel.medio:
				range = 4;
				posiciones = new float[]{-6,-2,2,6};
			break;
			case Dificultad.Nivel.dificil:
				range = 5;
				posiciones = new float[]{-8,-4,0,4,8};
			break;
		}
		maxMoving = range - 2;
		aliens.Clear();
		aliensMoving = new bool[posiciones.Length];
		for(int i = 0; i < aliensMoving.Length; i++)
			aliensMoving[i] = false;
		foreach(float f in posiciones){
			GameObject g = (GameObject)Instantiate(alienPrefab,new Vector3(f,alienPrefab.transform.position.y,enemyPosZ),alienPrefab.transform.rotation);
			aliens.Add(g.GetComponent<TweenPosition>());
		}
		foreach(TweenPosition alien in aliens){
			alien.to.x = alien.transform.position.x;
			alien.from.x = alien.transform.position.x;
		}
	}

	void RestartHand(){
		launched = false;
		detenido = false;
		rb.useGravity = false;
		rb.velocity = Vector3.zero;
		hand.transform.position = handPos;
		BlockPosition(false);
	}

	void NextLevel(){
		switch(dificultad){
			case Dificultad.Nivel.facil:
				dificultad = Dificultad.Nivel.medio;
			break;
			case Dificultad.Nivel.medio:
				dificultad = Dificultad.Nivel.dificil;
			break;
		}
	}

	IEnumerator MoveAlien(int index, float time){
		moving++;
		Debug.Log("Moving: "+index);
		aliensMoving[index] = true;
		yield return new WaitForSeconds(Random.Range(0,2));
		aliens[index].PlayForward();
		yield return new WaitForSeconds(time);
		aliens[index].PlayReverse();
		aliensMoving[index] = false;
		moving--;
	}
	
	// Update is called once per frame
	Vector3 lastPos;
	void Update () {
		if(Time.timeScale == 0)
			return;
		if(moving < maxMoving){
			int index = Random.Range(0,aliensMoving.Length);
			while(aliensMoving[index] || aliens[index].transform.position.x == blocked)
				index = Random.Range(0,aliensMoving.Length);
			Debug.Log(index);
			StartCoroutine(MoveAlien(index,5));
		}
		Vector3 handPosition = Input.mousePosition - new Vector3(Screen.width/2, Screen.height/2, 0f);
        Vector3 viewport = Camera.main.ScreenToViewportPoint(handPosition);
        float vPos = viewport.y * height;
		float hPos = viewport.x * width;
		manoRef.transform.position = new Vector3(hPos,vPos,0);

		if(!launched){
			if(manoRef.transform.position.y > 2f){
				launched = true;
				rb.velocity = Vector3.zero;
				float launchX = LaunchX(range);
				rb.AddForce(new Vector3(launchX,moveSpeed.y,moveSpeed.x)*launchForce,ForceMode.Impulse);
				rb.useGravity = true;
			}
		}
		else{
			if(detenido){
				if(manoRef.transform.position.y < lastPos.y){
					hand.transform.Translate(new Vector3(0,0,Time.deltaTime*-moveSpeed.y * 3f));
				}
				if(hand.transform.position.z <= -3){
					RestartHand();
				}
			}
		}

		lastPos = manoRef.transform.position;
	}

	float LaunchX(float range){
		if(range == 3){
			if(manoRef.transform.position.x < -1.5f){
				mano.target = posiciones[0];
				return -5;
			}
			else if(manoRef.transform.position.x > 1.5f){
				mano.target = posiciones[2];
				return 5;
			}
			else{
				mano.target = posiciones[1];
			}
		}
		else if(range == 4){
			if(manoRef.transform.position.x < -3f){
				mano.target = posiciones[0];
				return -6f;
			}
			else if(manoRef.transform.position.x < 0){
				mano.target = posiciones[1];
				return -2f;
			}
			else if(manoRef.transform.position.x >= 0 && manoRef.transform.position.x <= 3f){
				mano.target = posiciones[2];
				return 2f;
			}
				
			else{
				mano.target = posiciones[3];
				return 6f;
			}
				

		}
		else if(range == 5){
			if(manoRef.transform.position.x < -3f){
				mano.target = posiciones[0];
				return -8f;
			}
			else if(manoRef.transform.position.x < -1f){
				mano.target = posiciones[1];
				return -4f;
			}
			else if(manoRef.transform.position.x > 1f && manoRef.transform.position.x <= 3f){
				mano.target = posiciones[3];
				return 4f;
			}
			else if(manoRef.transform.position.x > 3f){
				mano.target = posiciones[4];
				return 8f;
			}
			else
			{
				mano.target = posiciones[2];
			}
				
		}
		return 0;
	}

	public void BlockPosition(bool b){
		if(b)
			blocked = mano.target;
		else
			blocked = 100;
	}
}
