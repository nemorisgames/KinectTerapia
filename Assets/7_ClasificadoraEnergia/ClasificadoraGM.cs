using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClasificadoraGM : MonoBehaviour
{
    public static ClasificadoraGM Instance;
    public bool moveWithMouse = true;
    public Transform moveRef, palanca;
    public float height, width;
    Vector3 palancaPos, palancaPosInicial, moveRefLast;
    public Transform leftExit, rightExit;
    public Transform leftDoor, rightDoor;
    public float palancaSign;
    bool moving = false;
    public Dificultad.Nivel dificultad;
    public int atrapadas = 0;
    public int target = 10;
    public int total = 30;
    public int openDoor = 0;
    private float ballTime = 2f;
    public GameObject ballPrefab;
    public UILabel fallosLabel, restantesLabel;
    private List<EsferaClasificadora> currentBalls = new List<EsferaClasificadora>();
    private IEnumerator nextBall;
    private int fallos;
    private int score;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        palancaPosInicial = palanca.position;
    }

    void Start()
    {
        Random.InitState(System.DateTime.Now.Minute * System.DateTime.Now.Second);
        Init();
    }

    public void Init()
    {
        switch (dificultad)
        {
            case Dificultad.Nivel.facil:
                target = 10;
                total = 15;
                ballTime = 2f;
                score = 100;
                break;
            case Dificultad.Nivel.medio:
                target = 6;
                total = 20;
                ballTime = 1.75f;
                score = 200;
                break;
            case Dificultad.Nivel.dificil:
                target = 3;
                total = 25;
                ballTime = 1.5f;
                score = 300;
                break;
        }
        fallos = 0;
        fallosLabel.text = "Fallos: 0";
        restantesLabel.text = "Restantes: " + total;
        InitBall();
    }

    void InitBall()
    {
        GameObject go = (GameObject)Instantiate(ballPrefab, ballPrefab.transform.position, ballPrefab.transform.rotation);
        EsferaClasificadora ec = go.GetComponent<EsferaClasificadora>();
        ec.rojo = (Random.Range(1f, 100f) > 50f ? true : false);
        ec.SetColor();
        currentBalls.Add(ec);
        if (total > 0)
        {
            nextBall = NextBall();
            StartCoroutine(nextBall);
        }
    }

    IEnumerator NextBall()
    {
        yield return new WaitForSeconds(ballTime);
        InitBall();
    }

    public void CatchBall(bool b)
    {
        if (Time.timeScale == 0)
            return;
        total--;
        restantesLabel.text = "Restantes: " + total;
        if (b)
            GameManager.instance.AddToScore(score);
        else
            fallos++;
        fallosLabel.text = "Fallos: " + fallos;
        if (total <= 0)
        {
            GameManager.instance.FinishLevel();
            StopCoroutine(nextBall);

            foreach (EsferaClasificadora e in currentBalls)
            {
                if (e != null && e.gameObject != null)
                    Destroy(e.gameObject);
            }

            switch (dificultad)
            {
                case Dificultad.Nivel.facil:
                    dificultad = Dificultad.Nivel.medio;
                    break;
                case Dificultad.Nivel.medio:
                    dificultad = Dificultad.Nivel.dificil;
                    break;
                default:
                    break;
            }
        }
        if (fallos >= target)
        {
            foreach (EsferaClasificadora e in currentBalls)
            {
                if (e != null && e.gameObject != null)
                    Destroy(e.gameObject);
            }

            GameManager.instance.LevelFailed();
            StopCoroutine(nextBall);
        }
    }


    void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (moveWithMouse)
        {
            Vector3 handPosition = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
            Vector3 viewport = Camera.main.ScreenToViewportPoint(handPosition);
            float vPos = viewport.y * height;
            float hPos = viewport.x * width;
            moveRef.position = new Vector3(hPos, vPos, 0);
        }

        if (moveRef.position.z <= 2f)
            palancaSign = Mathf.Sign(moveRef.position.x);

        float palancaX = 0;
        float palancaZ = 0;
        if (Mathf.Abs(moveRef.position.x) > 0.25f)
        {
            palancaX = Mathf.Clamp(moveRef.position.z - 1, 0, 2.5f) * palancaSign;
            palancaZ = Mathf.Clamp(moveRef.position.z - 1, 0, 2.5f);
        }
        palanca.position = new Vector3(palancaX, 0, palancaZ);

        moveRefLast = moveRef.position;

        if (currentBalls != null && currentBalls.Count > 0)
        {

        }

        if (moveRef.position.z >= 2.5f && openDoor == 0)
        {
            OpenDoor(palanca.position.x);
        }
        else if (moveRef.position.z <= 2f && openDoor != 0)
        {
            OpenDoor();
        }
    }

    void OpenDoor(float f = 0)
    {
        if (f != 0)
        {
            f = Mathf.Sign(f);
            if (f > 0)
                rightDoor.localPosition = new Vector3(rightDoor.localPosition.x, 3, rightDoor.localPosition.z);
            else
                leftDoor.localPosition = new Vector3(leftDoor.localPosition.x, 3, leftDoor.localPosition.z);
            openDoor = (int)f;
        }
        else
        {
            leftDoor.localPosition = new Vector3(leftDoor.localPosition.x, 0, leftDoor.localPosition.z);
            rightDoor.localPosition = new Vector3(rightDoor.localPosition.x, 0, rightDoor.localPosition.z);
            openDoor = 0;
        }
    }
}
