using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatientResults : MonoBehaviour {
    public UILabel nameGameLabel;
    public UILabel recordLabel;
    int pk_game;
	// Use this for initialization
	void Start () {
		
	}

    public void SetInformation(int pk_game, string name)
    {
        nameGameLabel.text = name;
        this.pk_game = pk_game;
    }

    public void ButtonPlay()
    {
        PlayerPrefs.SetInt("pk_game", pk_game);
    }

    public void ButtonDetails()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
