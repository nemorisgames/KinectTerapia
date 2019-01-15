using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscudoBall : MonoBehaviour {
	[HideInInspector]
	public float angle = 0;
	[HideInInspector]
	public float baseSpeed = 15f;
	public float sweetSpot = 0.4f;
	public float angleAmp = 2;
	Rigidbody rb;
	[HideInInspector]
	public int limit;
	[HideInInspector]
	public float fixAngle = 0.1f;
	private EscudoGM gm;
	System.Random random;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody>();
		random = new System.Random();
	}

	void Start(){
		Init();
	}

	void Update(){
		if(transform.position.x > limit || transform.position.x < -limit){
			gm.InitBall();
			Destroy(this.gameObject);
		}
	}
	
	public void SetGM(EscudoGM gm){
		this.gm = gm;
		this.limit = (gm.limit + 3);
		this.baseSpeed = gm.ballBaseSpeed;
	}

	void Init(){
		rb.AddForce(new Vector3(0,-baseSpeed,0));
	}

	void UpdateSpeed(int dir){
		float speed = baseSpeed;
		if(dir > 0)
			speed *= gm.playerBallSpeedMult;
		else
			speed *= gm.CPUBallSpeedMult;
		rb.velocity = Vector3.zero;
		rb.AddForce(angle*baseSpeed,dir*speed,0);
		gm.SetCPUTarget(dir > 0 ? true : false);
	}

	void OnCollisionEnter(Collision col){
		Collider c = col.collider;
		float r = 0;
		switch(c.tag){
			case "Player":
				EscudoPlayer player = c.GetComponent<EscudoPlayer>();
				float extLeft = (c.bounds.center.x - c.bounds.extents.x / 2);
				float extRight = (c.bounds.center.x + c.bounds.extents.x / 2);
				
				if(c.bounds.center.x + sweetSpot/2 > transform.position.x && c.bounds.center.x - sweetSpot/2 < transform.position.x){
					angle = 0;
				}
				else{
					if(Mathf.Abs(extLeft - transform.position.x) < Mathf.Abs(extRight - transform.position.x)){
						angle = angleAmp*Mathf.Abs((extLeft-transform.position.x))*-1;
					}
					else{
						angle = angleAmp*Mathf.Abs((extRight-transform.position.x));
					}
					angle += player.speed;
				}
				gm.cpuNave.ballAngle = angle;
				UpdateSpeed(1);
				break;
			case "CPU":
				float cpuPos = gm.cpuNave.transform.position.x;
				float playerPos = gm.player.transform.position.x;
				float target = 0;

				if(cpuPos <= 3f && cpuPos >= -3f && playerPos <= 3f && playerPos >= -3f){
					r = random.Next(0,99);
					if(r < 45){
						r = random.Next(0,1);
						if(r == 0)
							target = random.Next(9,11);
						else
							target = -(random.Next(9,11));
					}
					else{
						target = -playerPos * (random.Next(0,10)/2);
					}

				}
				//si cpu esta cerca de una esquina
				else if(cpuPos > 6 || cpuPos < -6){
					//esquinas
					float sign = 1;
					if(cpuPos > 6)
						sign = -1;
					if((cpuPos > 6 && playerPos >= 6f) || (cpuPos < -6 && playerPos <= -6f)){
						r = random.Next(0,99);
						if(r < 35)
							target = sign * random.Next(0,3);
						else
							target = sign * random.Next(13,18);
					}
					else{
						r = random.Next(0,99);
						if(r < 50)
							target = Mathf.Abs(cpuPos) * sign;
						else
							target = random.Next(5,15) * sign;
					}
				}
				else{
					target = random.Next(-7,7);
				}
				Debug.Log(target);
				angle = target * fixAngle;
				UpdateSpeed(-1);
			break;
			case "Block":
			Destroy(c.gameObject);
			break;
			case "Wall":
			angle *= -1;
			break;
			case "StationPlayer":
			gm.InitBall();
			Destroy(this.gameObject);
			break;
			case "StationCPU:":
			Destroy(this.gameObject);
			break;
		}
	}
}
