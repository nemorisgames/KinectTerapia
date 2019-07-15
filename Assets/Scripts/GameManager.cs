using System.Collections;
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
    public UILabel descriptionLabel;
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
    public AudioClip instructionLeft, instructionRight;
    public AudioClip[] winLevelSound, loseLevelSound, winGameSound;
    private bool gameStarted = false;
    private AstraManager astraManager;

    void Awake ()
    {
        config = GameObject.FindObjectOfType<PositionTransformator> ();
        astraManager = GameObject.FindObjectOfType<AstraManager> ();
        audioSource = GetComponent<AudioSource> ();
        Time.timeScale = 1f;
    }
    // Use this for initialization
    void Start ()
    {
        Random.InitState (System.DateTime.Now.Second * System.DateTime.Now.Minute);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != null)
            {
                Destroy (gameObject);
            }
        }
        Time.timeScale = 0f;
        if (loseScore == null)
            loseScore = youLoseScreen.transform.Find ("Subtitle").GetComponent<UILabel> ();

        if (config != null && astraManager != null)
        {
            // = PlayerPrefs.GetInt("handSelected"),
            if (!astraManager.lockMovX)
            {
                config.horLimits.x = PlayerPrefs.GetFloat ("limitHorMin");
                config.horLimits.y = PlayerPrefs.GetFloat ("limitHorMax");
            }
            if (!astraManager.lockMovY)
            {
                config.verLimits.x = PlayerPrefs.GetFloat ("limitVerMin");
                config.verLimits.y = PlayerPrefs.GetFloat ("limitVerMax");
            }

            if (!astraManager.lockMovZ)
            {
                config.depthLimits.x = PlayerPrefs.GetFloat ("limitDepthMin");
                config.depthLimits.y = PlayerPrefs.GetFloat ("limitDepthMax");
            }
        }

        int handSelected = PlayerPrefs.GetInt ("handSelected", 1);
        descriptionLabel.text = descriptionLabel.text.Replace ("@", (handSelected == 1 ?
            (SceneManager.GetActiveScene ().name.Trim () != "Game8" ? "derecho" : "derecha") :
            (SceneManager.GetActiveScene ().name.Trim () != "Game8" ? "izquierdo" : "izquierda")));
        GameObject g = GameObject.Find("UI Root");
        if (g == null) g = GameObject.Find("UI Root + GameManager");
        g.transform.Find("Camera").GetComponent<AudioSource>().Stop();
        PlayAudio ((handSelected == 1 ? instructionRight : instructionLeft), 3f);
    }

    public void AddToScore (int points)
    {
        currentScore += points;
        scoreLabel.text = "" + currentScore;
    }

    public void SubstractFromScore (int points)
    {
        currentScore -= points;
        scoreLabel.text = "" + currentScore;
    }

    public void SetScore (int points)
    {
        currentScore = points;
        scoreLabel.text = "" + currentScore;
    }

    public void ButtonPlay ()
    {
        GameObject g = GameObject.Find("UI Root");
        if (g == null) g = GameObject.Find("UI Root + GameManager");
        g.transform.Find("Camera").GetComponent<AudioSource>().Play();
        Time.timeScale = 1f;
        beginScreen.PlayForward ();
        gameStarted = true;
    }

    public void ButtonNextLevel ()
    {
        Time.timeScale = 1f;
        nextLevelScreen.PlayReverse ();
    }

    AudioClip GetRandomAudio (AudioClip[] audios)
    {
        return audios[Random.Range (0, audios.Length - 1)];
    }

    public void FinishLevel ()
    {
        Time.timeScale = 0f;
        currentLevel++;
        if (currentLevel > 3)
        {
            finalScoreLabel.text = "Puntaje: " + currentScore;
            youWinScreen.PlayForward ();
            SaveSession ();
            PlayAudio (GetRandomAudio (winGameSound));

        }
        else
        {
            nextLevelScreen.PlayForward ();
            levelLabel.text = "Nivel " + currentLevel;
            PlayAudio (GetRandomAudio (winLevelSound), 3f);
        }

    }

    public void LevelFailed ()
    {
        PlayAudio (GetRandomAudio (loseLevelSound), 3f);
        Time.timeScale = 0f;
        youLoseScreen.PlayForward ();
        if (loseScore != null)
            loseScore.text = "Puntaje: \n" + currentScore;
        SaveSession ();

    }

    void SaveSession ()
    {
        string s = "";
        float vMax = 0, vMean = 0;
        if (HandPosition.Instance != null)
        {
            s = HandPosition.Instance.SavePoints ();
            vMean = HandPosition.Instance.MeanVel ();
            vMax = HandPosition.Instance.MaxVel ();
        }
        DatabaseManager.instance.SaveSession (currentScore, vMean, vMax, s);
    }

    public void PlayAgain ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    public void Exit ()
    {
        SceneManager.LoadScene ("GameGrid");
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = (Time.timeScale <= 0f)?1f:0f;
        }
    }

    public void PlayAudio (AudioClip clip, float f = 0.5f)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot (clip, f);
    }
}