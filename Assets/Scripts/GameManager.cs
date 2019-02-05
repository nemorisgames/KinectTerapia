using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    [HideInInspector]
    public int currentLevel = 1;
    [HideInInspector]
    public int currentScore = 0;
    public UILabel scoreLabel;
    public UILabel levelLabel;
    public UILabel finalScoreLabel;
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
        Time.timeScale = 0f;
        currentLevel++;
        if(currentLevel > 3)
        {
            finalScoreLabel.text = "Puntaje: " + currentScore;
            youWinScreen.PlayForward();
        }
        else
        {
            nextLevelScreen.PlayForward();
            levelLabel.text = "Nivel " + currentLevel;
        }
    }

    public void LevelFailed(){
        youLoseScreen.PlayForward();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        SceneManager.LoadScene("GameGrid");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
