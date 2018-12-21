using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    int currentLevel = 1;
    int currentScore = 0;
    public UILabel scoreLabel;
    public TweenAlpha beginScreen;
    public TweenAlpha nextLevelScreen;
    public TweenAlpha youWinScreen;
    public TweenAlpha youLoseScreen;

    // Use this for initialization
    void Start () {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }
        Time.timeScale = 0f;
    }

    public void AddToScore(int points)
    {
        currentScore += points;
        scoreLabel.text = "" + currentScore;
    }

    public void ButtonPlay()
    {
        Time.timeScale = 1f;
        beginScreen.PlayForward();
    }

    public void ButtonNextLevel()
    {
        Time.timeScale = 1f;
        nextLevelScreen.PlayReverse();
    }

    public void FinishLevel()
    {
        currentLevel++;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
