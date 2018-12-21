using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGrid : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void ButtonPlayGame1()
    {
        PlayerPrefs.SetInt("pk_game", 1);
        SceneManager.LoadScene("Game1");
    }

    public void ButtonPlayGame2()
    {
        PlayerPrefs.SetInt("pk_game", 2);
        SceneManager.LoadScene("Game2");
    }

    public void ButtonPlayGame3()
    {
        PlayerPrefs.SetInt("pk_game", 3);
        SceneManager.LoadScene("Game3");
    }

    public void ButtonPlayGame4()
    {
        PlayerPrefs.SetInt("pk_game", 4);
        SceneManager.LoadScene("Game4");
    }

    public void ButtonPlayGame5()
    {
        PlayerPrefs.SetInt("pk_game", 5);
        SceneManager.LoadScene("Game5");
    }

    public void ButtonPlayGame6()
    {
        PlayerPrefs.SetInt("pk_game", 6);
        SceneManager.LoadScene("Game6");
    }

    public void ButtonPlayGame7()
    {
        PlayerPrefs.SetInt("pk_game", 7);
        SceneManager.LoadScene("Game7");
    }

    public void ButtonPlayGame8()
    {
        PlayerPrefs.SetInt("pk_game", 8);
        SceneManager.LoadScene("Game8");
    }

    public void ButtonPlayGame9()
    {
        PlayerPrefs.SetInt("pk_game", 9);
        SceneManager.LoadScene("Game9");
    }
    // Update is called once per frame
    void Update () {
		
	}
}
