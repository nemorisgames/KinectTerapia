using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    public int life = 3;
    public int points = 10;
	// Use this for initialization
	void Start () {
		
	}

    public void BlockHit()
    {
        life--;
        if(life <= 0)
        {
            GameManager.instance.AddToScore(points);
            Destroy(gameObject);
            print(GameObject.FindGameObjectsWithTag("Block").Length);
            if(GameObject.FindGameObjectsWithTag("Block").Length <= 1)
            {
                GameManager.instance.FinishLevel();
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
