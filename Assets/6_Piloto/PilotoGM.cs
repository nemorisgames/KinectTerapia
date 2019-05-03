using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotoGM : MonoBehaviour
{
    public static PilotoGM Instance;
    public Dificultad.Nivel dificultad;
    public int health = 5;
    public int height, width;
    public float screenSpeed = 10;
    public float screenLength = 30;
    public Vector2 range = Vector2.zero;
    public int score = 100;
    public Transform espacioNave, nave;
    private PilotoNave pilotoNave;
    [HideInInspector]
    public Vector3 lastPos;
    public GameObject[] vidas;
    public GameObject[] etapa;

    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        pilotoNave = nave.GetComponent<PilotoNave>();
    }

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
            return;
        Vector3 handPosition = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        Vector3 viewport = Camera.main.ScreenToViewportPoint(handPosition);
        float vPos = viewport.y * height;
        float hPos = viewport.x * width;
        //if(nave.localPosition.x != hPos || nave.localPosition.y != vPos)
        //	pilotoNave.rigidbody().AddForce(hPos - nave.localPosition.x,vPos - nave.localPosition.y,0);

        nave.localPosition = new Vector3(hPos, vPos, 0);

        Vector3 screenPos = espacioNave.transform.position;
        espacioNave.position = new Vector3(screenPos.x + Time.deltaTime * screenSpeed, screenPos.y, screenPos.z);

        lastPos = nave.localPosition;
    }

    void ActivarEtapa(int i)
    {
        i = Mathf.Clamp(i, 0, 2);
        if (etapa.Length < 2)
            return;
        foreach (GameObject g in etapa)
            g.SetActive(false);
        etapa[i].SetActive(true);
        PilotoObstaculo[] obstaculos = etapa[i].GetComponentsInChildren<PilotoObstaculo>();
        foreach (PilotoObstaculo obs in obstaculos)
            obs.InicializarPilares(range);
    }

    public void Init()
    {
        switch (dificultad)
        {
            case Dificultad.Nivel.facil:
                health = 5;
                screenSpeed = 6;
                score = 150;
                range = new Vector2(8, 9);
                break;
            case Dificultad.Nivel.medio:
                health = 4;
                screenSpeed = 8;
                score = 250;
                range = new Vector2(7.8f, 8.5f);
                break;
            case Dificultad.Nivel.dificil:
                health = 3;
                screenSpeed = 10;
                score = 400;
                range = new Vector2(7.5f, 8.2f);
                break;
        }
        ActivarEtapa((int)dificultad);
        SetHealth();
        espacioNave.position = new Vector3(-8, 0, 0);
    }

    public void NextLevel()
    {
        switch (dificultad)
        {
            case Dificultad.Nivel.facil:
                dificultad = Dificultad.Nivel.medio;
                break;
            case Dificultad.Nivel.medio:
                dificultad = Dificultad.Nivel.dificil;
                break;
        }
        Init();
    }

    public void SetHealth()
    {
        foreach (GameObject o in vidas)
            o.SetActive(false);
        for (int i = 0; i < health; i++)
            vidas[i].SetActive(true);
    }
}
