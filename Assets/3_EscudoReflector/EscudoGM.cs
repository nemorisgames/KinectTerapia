using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscudoGM : MonoBehaviour {
	public int limit = 14;
	public EscudoPlayer player;
	public EscudoCPU cpuNave;
	public GameObject ball;
	System.Random rand;
	GameObject currentBall;
	public float ballBaseSpeed = 300f;
	public float playerSpeed = 15f;
	public float playerBallSpeedMult = 2f;
	public bool CPUCatchAll = false;
	public float CPUBaseSpeed = 10f;
	public float CPUBallSpeedMult = 1f;

	// Use this for initialization
	void Awake () {
		rand = new System.Random();
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Init(){
		cpuNave.SetGM(this);
		player.width = limit + 1;
		player.moveSpeed = playerSpeed;
		InitBall();
	}

	public void InitBall(){
		Vector3 pos = cpuNave.transform.position;
		pos.y -= 0.7f;
		currentBall = (GameObject)Instantiate(ball,pos,Quaternion.identity);
		currentBall.GetComponent<EscudoBall>().SetGM(this);
		if(CPUCatchAll)
			cpuNave.target = player.transform;
		cpuNave.chase = true;
	}

	public void SetCPUTarget(bool b){
		if(b){
			cpuNave.speed = 10;
			cpuNave.target = currentBall.transform;
		}
		else{
			cpuNave.speed = 7;
			if(CPUCatchAll)
				cpuNave.target = currentBall.transform;
			else
				cpuNave.target = null;
		}
		StartCoroutine(cpuNave.startChase());
	}
}
