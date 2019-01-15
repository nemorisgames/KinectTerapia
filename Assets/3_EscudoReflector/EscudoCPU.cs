using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscudoCPU : MonoBehaviour {
	public bool chase = false;
	public Transform target;
	[HideInInspector]
	public float speed = 8;
	public float rangoError = 0.25f;
	private EscudoGM gm;
	[HideInInspector]
	public float ballAngle = 1;
	public float chaseDelay = 0.5f;
	[HideInInspector]
	public float currentSpeed = 0;
	int limit;
	int shortLimit;
	int moveDir = 1;

	public void SetGM(EscudoGM gm){
		this.gm = gm;
		this.limit = gm.limit;
		shortLimit = limit - 4;
		this.speed = gm.CPUBaseSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		if(gm.CPUCatchAll && target != null){
			pos = transform.position;
			float targetX = target.position.x;
			if(ballAngle == 0)
				ballAngle = 1;
			currentSpeed = Time.deltaTime * speed * Mathf.Abs(ballAngle);
			if(targetX < pos.x)
				currentSpeed *= -1;
			currentSpeed = Mathf.Clamp(currentSpeed,-limit,limit);
			if(currentSpeed > 0 && pos.x >= limit || currentSpeed < 0 && pos.x <= -limit)
				currentSpeed = 0;
			else if(pos.x <= target.position.x + rangoError && pos.x >= target.position.x - rangoError)
				currentSpeed = 0;
			else
				transform.Translate(currentSpeed,0,0);
		}
		else{
			if(target != null && chase){
				pos = transform.position;
				if(pos.x <= target.position.x + rangoError && pos.x >= target.position.x - rangoError)
					return;
				float targetX = target.position.x;
				if(ballAngle == 0)
					ballAngle = 1;
				currentSpeed = Time.deltaTime * speed;
				if(targetX < pos.x)
					currentSpeed *= -1;
				currentSpeed = Mathf.Clamp(currentSpeed,-limit,limit);
				if(currentSpeed > 0 && pos.x >= limit || currentSpeed < 0 && pos.x <= -limit)
					return;
				else
					transform.Translate(currentSpeed,0,0);
				if(Mathf.Abs(currentSpeed) < 0.05)
					StartCoroutine(startChase());
			}
			else{
				float playerX = gm.player.transform.position.x;
				if(moveDir > 0 && pos.x >= playerX + 5 || moveDir < 0 && pos.x <= playerX - 5)
					moveDir *= -1;
				transform.Translate((Time.deltaTime * (speed) * moveDir),0,0);
			}
		}
	}

	public IEnumerator startChase(){
		chase = false;
		yield return new WaitForSeconds(chaseDelay);
		chase = true;
	}
}
