using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    public int life = 3;
    public int points = 10;
    Color[] colorLevels;
    // Use this for initialization
    void Start () {
        colorLevels = new Color[3] { Color.yellow, Color.green, Color.blue };
        GetComponent<Renderer>().material.SetColor("_EmissionColor", colorLevels[life - 1]);
	}

    public void BlockHit()
    {
        life--;
        GameManager.instance.AddToScore(points);
        if (life <= 0)
        {
            Destroy(gameObject);
            print(GameObject.FindGameObjectsWithTag("Block").Length);
            if(GameObject.FindGameObjectsWithTag("Block").Length <= 1)
            {
                GameManager.instance.FinishLevel();
            }
        }
        else
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", colorLevels[life - 1]);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
