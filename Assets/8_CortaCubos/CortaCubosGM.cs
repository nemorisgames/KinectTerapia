using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CortaCubosGM : MonoBehaviour
{
    public static CortaCubosGM Instance;
    public Transform moveRef;
    public bool moveWithMouse = false;
    public float height, width;
    private List<Vector2> points = new List<Vector2>();
    public Dificultad.Nivel dificultad;
    public enum CortaDireccion
    {
        None,
        Horizontal,
        Vertical,
        DiagonalD,
        DiagonalI
    }
    public CortaDireccion direccion = CortaDireccion.None;
    private IEnumerator corteActual;
    public bool canCut = false;
    public GameObject cortaCuboPrefab;
    public bool cubeInArea = false;
    public float spawnTime = 2f;
    public float cubeSpeed = 10f;
    public int cubes = 30;
    public int spawnedCubes = 30;
    public int target = 5;
    public int fallos = 0;
    public int puntaje = 100;
    public UILabel fallosLabel, totalLabel, corteLabel;
    private float s_xy = 0, s_x = 0, s_y = 0, s_x2 = 0, m = 0;
    public float margenX, margenY;
    public Transform volumenMargen;
    public UILabel debugLabel;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        Random.InitState(System.DateTime.Now.Second * System.DateTime.Now.Minute);
    }

    void Start()
    {
        //Init();
    }

    public void Init()
    {
        //volumenMargen.localScale = new Vector3(margenX,margenY,volumenMargen.localScale.z);
        ClearCubes();
        fallos = 0;
        s_xy = 0;
        s_x = 0;
        s_y = 0;
        s_x2 = 0;
        switch (dificultad)
        {
            case Dificultad.Nivel.facil:
                cubeSpeed = 4f;
                spawnTime = 2.5f;
                cubes = 15;
                target = 5;
                puntaje = 150;
                break;
            case Dificultad.Nivel.medio:
                cubeSpeed = 6f;
                spawnTime = 2.25f;
                cubes = 12;
                target = 4;
                puntaje = 200;
                break;
            case Dificultad.Nivel.dificil:
                cubeSpeed = 8f;
                spawnTime = 2f;
                cubes = 10;
                target = 3;
                puntaje = 250;
                break;
        }
        spawnedCubes = cubes;
        UpdateFallos(fallos);
        UpdateRestantes(cubes);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("CPU"), LayerMask.NameToLayer("CPU"));
        StartCoroutine(SpawnCube(0));
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

        /* 
        Debug.DrawLine(new Vector3(-margenX,margenY -3.15f,0),new Vector3(margenX,margenY -3.15f,0),Color.red);
        Debug.DrawLine(new Vector3(margenX,margenY -3.15f,0),new Vector3(margenX,-margenY -3.15f,0),Color.red);
        Debug.DrawLine(new Vector3(margenX,-margenY -3.15f,0),new Vector3(-margenX,-margenY -3.15f,0),Color.red);
        Debug.DrawLine(new Vector3(-margenX,-margenY -3.15f,0),new Vector3(-margenX,margenY -3.15f,0),Color.red);
        */

        debugLabel.text = moveRef.position.ToString();

        if (Mathf.Abs(moveRef.position.y) <= (margenY) && Mathf.Abs(moveRef.position.x) <= (margenX))
        {
            points.Add(moveRef.position);
            s_xy += moveRef.position.x * moveRef.position.y;
            s_x += moveRef.position.x;
            s_y += moveRef.position.y;
            s_x2 += Mathf.Pow(moveRef.position.x,2);
        }
        else
        {
            if (!canCut)
                canCut = true;
            if (points.Count > 1)
            {
                //int last = points.Count - 1;
                //float m = (points[last].y - points[0].y) / (points[last].x - points[0].x);
                /* 
                for(int i = 0; i < points.Count; i++){
                    s_xy += points[i].x * points[i].y;
                    s_x += points[i].x;
                    s_y += points[i].y;
                    s_x2 += Mathf.Pow(points[i].x,2);
                }
                */
                m = (s_xy - ((s_x*s_y)/points.Count) ) / ( s_x2 - (Mathf.Pow(s_x,2)/points.Count));
                TipoMovimiento(m);
                points.Clear();
                s_xy = 0;
                s_x = 0;
                s_y = 0;
                s_x2 = 0;
            }
        }

        if (fallos >= target)
            GameManager.instance.LevelFailed();
    }

    void TipoMovimiento(float m)
    {
        //if (!cubeInArea)
        //    return;
        if (corteActual != null)
            StopCoroutine(corteActual);
        //Debug.Log(m);
        float sign = Mathf.Sign(m);
        if (Mathf.Abs(m) > 2)
        {
            //Debug.Log("Vertical");
            corteActual = SetDireccion(CortaDireccion.Vertical);
            StartCoroutine(corteActual);
        }
        else if (Mathf.Abs(m) < 0.5f)
        {
            //Debug.Log("Horizontal");
            corteActual = SetDireccion(CortaDireccion.Horizontal);
            StartCoroutine(corteActual);
        }
        else
        {
            if (sign >= 0)
            {
                //Debug.Log("Diagonal Derecho");
                corteActual = SetDireccion(CortaDireccion.DiagonalD);
                StartCoroutine(corteActual);
            }
            else
            {
                //Debug.Log("Diagonal Izquierdo");
                corteActual = SetDireccion(CortaDireccion.DiagonalI);
                StartCoroutine(corteActual);
            }

        }
    }

    public void UpdateFallos(int f)
    {
        fallos = f;
        fallosLabel.text = "Fallos: " + fallos;
    }

    public void UpdateRestantes(int r)
    {
        cubes = r;
        totalLabel.text = "Restantes: " + cubes;
    }

    IEnumerator SetDireccion(CortaDireccion dir)
    {
        direccion = dir;
        corteLabel.text = m + " | " +dir.ToString();
        yield return new WaitForSeconds(0.1f);
        direccion = CortaDireccion.None;
        m = 0;
    }

    public void CortarCubo(CortaCubo c)
    {
        if (direccion == CortaDireccion.None || c.broken)
            return;
        float a = c.transform.rotation.eulerAngles.z;
        //Debug.Log(a + " | "+direccion.ToString());
        if (
            (a == 0 && direccion == CortaDireccion.Vertical) ||
            (a < 90 && a > 0 && direccion == CortaDireccion.DiagonalI) ||
            (a == 315 && direccion == CortaDireccion.DiagonalD) ||
            (a == 90 && direccion == CortaDireccion.Horizontal)
        )
        {
            c.Break();
            direccion = CortaDireccion.None;
        }
    }


    public IEnumerator SpawnCube(float f)
    {
        if (spawnedCubes > 0)
        {
            yield return new WaitForSeconds(f);
            CortaCubo c = ((GameObject)Instantiate(cortaCuboPrefab, cortaCuboPrefab.transform.position, cortaCuboPrefab.transform.rotation)).GetComponent<CortaCubo>();
            c.speed = cubeSpeed;
            spawnedCubes--;
        }
    }

    public void ClearCubes()
    {
        List<GameObject> cubos = GameObject.FindGameObjectsWithTag("CPU").ToList();
        foreach (GameObject g in cubos)
            Destroy(g);
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
}
