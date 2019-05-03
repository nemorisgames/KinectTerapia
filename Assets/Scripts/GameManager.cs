﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    public UILabel loseScore;
    private PositionTransformator config;
    private AudioSource audioSource;
    public AudioClip winSound, loseSound;

    void Awake()
    {
        config = GameObject.FindObjectOfType<PositionTransformator>();
        audioSource = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start()
    {
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
        if (loseScore == null)
            loseScore = youLoseScreen.transform.Find("Subtitle").GetComponent<UILabel>();

        if (config != null)
        {
            // = PlayerPrefs.GetInt("handSelected"),
            config.horLimits.x = PlayerPrefs.GetFloat("limitHorMin");
            config.horLimits.y = PlayerPrefs.GetFloat("limitHorMax");
            config.verLimits.x = PlayerPrefs.GetFloat("limitVerMin");
            config.verLimits.y = PlayerPrefs.GetFloat("limitVerMax");
            config.depthLimits.x = PlayerPrefs.GetFloat("limitDepthMin");
            config.depthLimits.y = PlayerPrefs.GetFloat("limitDepthMax");
        }
    }

    public void AddToScore(int points)
    {
        currentScore += points;
        scoreLabel.text = "" + currentScore;
    }

    public void SubstractFromScore(int points)
    {
        currentScore -= points;
        scoreLabel.text = "" + currentScore;
    }

    public void SetScore(int points)
    {
        currentScore = points;
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
        PlayAudio(winSound);
        Time.timeScale = 0f;
        currentLevel++;
        if (currentLevel > 3)
        {
            finalScoreLabel.text = "Puntaje: " + currentScore;
            youWinScreen.PlayForward();
            SaveHeatmap();
        }
        else
        {
            nextLevelScreen.PlayForward();
            levelLabel.text = "Nivel " + currentLevel;
        }

    }

    public void LevelFailed()
    {
        PlayAudio(loseSound);
        Time.timeScale = 0f;
        youLoseScreen.PlayForward();
        if (loseScore != null)
            loseScore.text = "Puntaje: \n" + currentScore;
        SaveHeatmap();

    }

    void SaveHeatmap()
    {
        if (HandPosition.Instance == null)
            return;
        HandPosition.Instance.SavePoints();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        DatabaseManager.instance.SaveSession(SceneManager.GetActiveScene().name, currentScore, 1, 1);
        SceneManager.LoadScene("GameGrid");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelFailed();
        }

    }

    public void PlayAudio(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}
