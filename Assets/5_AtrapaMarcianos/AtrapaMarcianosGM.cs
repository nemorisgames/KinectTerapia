using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrapaMarcianosGM : MonoBehaviour
{
    public static AtrapaMarcianosGM Instance;
    public GameObject hand;
    public GameObject manoRef;
    public GameObject alienPrefab;
    public Dificultad.Nivel dificultad;
    public int atrapados = 0;
    public int maxMoving = 2;
    public int moving = 0;
    public int range = 3;
    public Vector2 moveSpeed;
    public float launchForce;
    public float enemyPosZ = 8.5f;
    Rigidbody rb;
    public bool launched = false;
    public bool detenido = false;
    public bool canLaunch = true;
    private Vector3 handPos;
    private int width = 8;
    private int height = 6;
    private float[] posiciones;
    public List<TweenPosition> aliens = new List<TweenPosition>();
    public bool[] aliensMoving = new bool[] { };
    private float blocked = 100;
    private AtrapaAlien mano;
    private int score;
    private float stageTime;
    private int atrapadosTarget;
    public float time;
    private float alienSpeed;
    private float alienTime;
    public UILabel timeLabel;
    public UILabel atrapadosLabel;
    public float savedTime;
    IEnumerator trappedAlien;
    public AudioClip trapSound, catchSound, alienSound;

    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        rb = hand.GetComponent<Rigidbody>();
        mano = hand.GetComponent<AtrapaAlien>();
        handPos = hand.transform.position;
        Random.InitState(System.DateTime.Now.Minute * System.DateTime.Now.Second);
    }

    void Start()
    {
        Init();
        //RestartHand();
    }

    void SetAtrapados(int a)
    {
        atrapados = a;
        atrapadosLabel.text = "Atrapados: " + atrapados + "/" + atrapadosTarget;
    }

    void Init()
    {
        time = 0;
        switch (dificultad)
        {
            case Dificultad.Nivel.facil:
                range = 3;
                atrapadosTarget = 3;
                posiciones = new float[] { -5, 0, 5 };
                score = 500;
                stageTime = 5;
                alienTime = 5;
                alienSpeed = 3;
                break;
            case Dificultad.Nivel.medio:
                range = 4;
                atrapadosTarget = 5;
                posiciones = new float[] { -6, -2, 2, 6 };
                score = 700;
                stageTime = 4;
                alienTime = 4f;
                alienSpeed = 2.5f;
                break;
            case Dificultad.Nivel.dificil:
                range = 5;
                atrapadosTarget = 10;
                posiciones = new float[] { -8, -4, 0, 4, 8 };
                score = 800;
                stageTime = 3;
                alienTime = 3.5f;
                alienSpeed = 2f;
                break;
        }
        SetAtrapados(0);
        SetTimeLabel(stageTime * 60);
        maxMoving = range - 1;
        foreach (TweenPosition t in aliens)
            Destroy(t.gameObject);
        aliens.Clear();
        aliensMoving = new bool[posiciones.Length];
        for (int i = 0; i < aliensMoving.Length; i++)
            aliensMoving[i] = false;
        foreach (float f in posiciones)
        {
            GameObject g = (GameObject)Instantiate(alienPrefab, new Vector3(f, -8.5f, enemyPosZ), alienPrefab.transform.rotation);
            EventDelegate.Add(g.GetComponent<TweenPosition>().onFinished, () => GameManager.instance.PlayAudio(alienSound));
            aliens.Add(g.GetComponent<TweenPosition>());
        }
        foreach (TweenPosition alien in aliens)
        {
            alien.to.x = alien.transform.position.x;
            alien.from.x = alien.transform.position.x;
        }
    }

    void RestartHand()
    {
        if (trappedAlien != null)
            StopCoroutine(trappedAlien);
        launched = false;
        detenido = false;
        PositionTransformator.Instance.ForcePosition(true);
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        hand.transform.position = handPos;
        mano.CampoFuerzaOn(false);
        BlockPosition(false);
        GameManager.instance.PlayAudio(trapSound);
        foreach (Transform t in hand.transform)
        {
            if (t.tag == "CPU")
            {
                SetAtrapados(atrapados + 1);
                GameManager.instance.AddToScore(score);
                Destroy(t.gameObject);
            }
        }
        if (atrapados >= atrapadosTarget)
        {
            savedTime = Time.time;
            GameManager.instance.FinishLevel();
        }
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

    IEnumerator MoveAlien(int index, float time)
    {
        moving++;
        //Debug.Log("Moving: "+index);
        aliensMoving[index] = true;
        yield return new WaitForSeconds(Random.Range(0, 2));
        aliens[index].PlayForward();
        yield return new WaitForSeconds(time);
        aliens[index].PlayReverse();
        aliensMoving[index] = false;
        moving--;
    }

    IEnumerator TrappedAlien()
    {
        yield return new WaitForSeconds(alienSpeed);
        foreach (Transform t in hand.transform)
            Destroy(t.gameObject);
    }

    // Update is called once per frame
    public Vector3 lastPos;
    void Update()
    {
        if (Time.timeScale == 0)
            return;
        if (moving < maxMoving)
        {
            int index = Random.Range(0, aliensMoving.Length);
            while (aliensMoving[index] || aliens[index].transform.position.x == blocked)
                index = Random.Range(0, aliensMoving.Length);
            //Debug.Log(index);
            StartCoroutine(MoveAlien(index, alienTime));
        }
        /* 
        Vector3 handPosition = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        Vector3 viewport = Camera.main.ScreenToViewportPoint(handPosition);
        float vPos = viewport.y * height;
        float hPos = viewport.x * width;
        //manoRef.transform.position = new Vector3(hPos, vPos, 0);
        */
        if (!launched)
        {
            if (!canLaunch && manoRef.transform.position.y < -1f)
                canLaunch = true;
            if (manoRef.transform.position.y > 0 && canLaunch)
            {
                Debug.Log("launched");
                PositionTransformator.Instance.ForcePosition(false);
                launched = true;
                rb.velocity = Vector3.zero;
                float launchX = LaunchX(range);
                GameManager.instance.PlayAudio(catchSound);
                rb.AddForce(new Vector3(launchX, moveSpeed.y, moveSpeed.x) * launchForce, ForceMode.Impulse);
                rb.useGravity = true;
                canLaunch = false;
            }
        }
        else
        {
            if (detenido)
            {
                if (manoRef.transform.position.y < lastPos.y)
                {
                    hand.transform.Translate(new Vector3(0, 0, Time.deltaTime * -moveSpeed.y * 3f));
                }
                if (hand.transform.position.z <= -3)
                {
                    RestartHand();
                }
            }
        }

        lastPos = manoRef.transform.position;
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0)
            return;
        time = stageTime * 60 - (Time.time - savedTime);
        SetTimeLabel(time);
    }

    void SetTimeLabel(float f)
    {
        timeLabel.text = Mathf.FloorToInt(f / 60) + ":" + (Mathf.FloorToInt(f % 60) < 10 ? "0" : "") + (Mathf.FloorToInt(f % 60));
    }

    float LaunchX(float range)
    {
        if (range == 3)
        {
            if (manoRef.transform.position.x < -3f)
            {
                mano.target = posiciones[0];
                return -5;
            }
            else if (manoRef.transform.position.x > 3f)
            {
                mano.target = posiciones[2];
                return 5;
            }
            else
            {
                mano.target = posiciones[1];
            }
        }
        else if (range == 4)
        {
            if (manoRef.transform.position.x < -6f)
            {
                mano.target = posiciones[0];
                return -6f;
            }
            else if (manoRef.transform.position.x < 0)
            {
                mano.target = posiciones[1];
                return -2f;
            }
            else if (manoRef.transform.position.x >= 0 && manoRef.transform.position.x <= 6f)
            {
                mano.target = posiciones[2];
                return 2f;
            }

            else
            {
                mano.target = posiciones[3];
                return 6f;
            }


        }
        else if (range == 5)
        {
            if (manoRef.transform.position.x < -7f)
            {
                mano.target = posiciones[0];
                return -8f;
            }
            else if (manoRef.transform.position.x < -3f)
            {
                mano.target = posiciones[1];
                return -4f;
            }
            else if (manoRef.transform.position.x > 3f && manoRef.transform.position.x <= 7f)
            {
                mano.target = posiciones[3];
                return 4f;
            }
            else if (manoRef.transform.position.x > 7f)
            {
                mano.target = posiciones[4];
                return 8f;
            }
            else
            {
                mano.target = posiciones[2];
            }

        }
        return 0;
    }

    public void BlockPosition(bool b)
    {
        if (b)
            blocked = mano.target;
        else
            blocked = 100;
    }

    public void Atrapado(float f)
    {
        int position = 0;
        for (int i = 0; i < posiciones.Length; i++)
        {
            if (f == posiciones[i])
            {
                position = i;
                break;
            }
        }
        GameObject g = (GameObject)Instantiate(alienPrefab, new Vector3(f, -8.5f, enemyPosZ), alienPrefab.transform.rotation);
        aliens[position] = g.GetComponent<TweenPosition>();
        aliens[position].to.x = f;
        aliens[position].from.x = f;
        trappedAlien = TrappedAlien();
        StartCoroutine(trappedAlien);
    }

    delegate void AlienAudio();
}
