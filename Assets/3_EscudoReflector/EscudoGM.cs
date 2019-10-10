using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscudoGM : MonoBehaviour
{
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
    public Dificultad.Nivel dificultad;
    public int bounceTarget;
    public int bounces;
    public int currentScore;
    public GameObject[] cpuStation;
    public AudioClip shootSound;
    public int tries = 3;
    public UILabel triesLabel;

    // Use this for initialization
    void Awake()
    {
        rand = new System.Random();


    }

    // Update is called once per frame
    void Start()
    {
        cpuNave.SetGM(this);
        player.width = limit + 1;
        player.moveSpeed = playerSpeed;
        Init();
    }

    public void SetCPUStation()
    {
        switch (dificultad)
        {
            case Dificultad.Nivel.facil:
                cpuStation[0].SetActive(true);
                cpuStation[1].SetActive(false);
                cpuStation[2].SetActive(false);
                break;
            case Dificultad.Nivel.medio:
                cpuStation[0].SetActive(false);
                cpuStation[1].SetActive(true);
                cpuStation[2].SetActive(false);
                break;
            case Dificultad.Nivel.dificil:
                cpuStation[0].SetActive(false);
                cpuStation[1].SetActive(false);
                cpuStation[2].SetActive(true);
                break;
        }
    }

    public void Init()
    {
        switch (dificultad)
        {
            case Dificultad.Nivel.facil:
                bounceTarget = 3;
                currentScore = 200;
                ballBaseSpeed = 200;
                break;
            case Dificultad.Nivel.medio:
                bounceTarget = 4;
                currentScore = 500;
                ballBaseSpeed = 250;
                break;
            case Dificultad.Nivel.dificil:
                bounceTarget = 5;
                currentScore = 800;
                ballBaseSpeed = 300;
                break;
        }
        tries = 3;
        triesLabel.text = "Intentos: "+tries;
        InitBall();
    }

    public void InitBall(int bounces = 0)
    {
        this.bounces = bounces;
        Vector3 pos = cpuNave.transform.position;
        pos.y -= 0.7f;
        GameManager.instance.PlayAudio(shootSound);
        currentBall = (GameObject)Instantiate(ball, pos, Quaternion.identity);
        currentBall.GetComponent<EscudoBall>().SetGM(this);
        if (CPUCatchAll)
            cpuNave.target = player.transform;
        cpuNave.chase = true;
    }

    public void SetCPUTarget(bool b)
    {
        //if(b){
        cpuNave.speed = 10;
        cpuNave.target = currentBall.transform;
        /* }
		else{
			cpuNave.speed = 7;
			if(CPUCatchAll)
				cpuNave.target = currentBall.transform;
			else
				cpuNave.target = null;
		}*/
        StartCoroutine(cpuNave.startChase());
    }

    public void PlayerHit(){
        tries--;
        triesLabel.text = "Intentos: "+tries;
        if(tries <= 0){
            GameManager.instance.LevelFailed();
        }
        else{
            InitBall();
        }
    }
}
