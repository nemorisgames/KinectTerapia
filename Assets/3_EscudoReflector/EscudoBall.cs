using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscudoBall : MonoBehaviour
{
    [HideInInspector]
    public float angle = 0;
    [HideInInspector]
    public float baseSpeed = 10f;
    public float sweetSpot = 0.4f;
    public float angleAmp = 2;
    Rigidbody rb;
    [HideInInspector]
    public int limit;
    [HideInInspector]
    public float fixAngle = 0.1f;
    private EscudoGM gm;
    private int score;
    System.Random random;
    public AudioClip breakSound, bounceSound;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        random = new System.Random();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (transform.position.x > limit || transform.position.x < -limit)
        {
            //GameManager.instance.SubstractFromScore(gm.bounces * score);
            gm.InitBall(gm.bounces);
            Destroy(this.gameObject);
        }
    }

    public void SetGM(EscudoGM gm)
    {
        this.gm = gm;
        this.limit = (gm.limit + 3);
        this.baseSpeed = gm.ballBaseSpeed;
        this.score = gm.currentScore;
    }

    void Init()
    {
        rb.AddForce(new Vector3(0, -baseSpeed, 0));
        gm.SetCPUTarget(true);
    }

    void UpdateSpeed(int dir)
    {
        float speed = baseSpeed;
        if (dir > 0)
            speed *= gm.playerBallSpeedMult;
        else
            speed *= gm.CPUBallSpeedMult;
        rb.velocity = Vector3.zero;
        rb.AddForce(angle * baseSpeed, dir * speed, 0);
        //gm.SetCPUTarget(dir > 0 ? true : false);
    }

    void OnCollisionEnter(Collision col)
    {
        Collider c = col.collider;
        float r = 0;
        switch (c.tag)
        {
            case "Player":
                GameManager.instance.PlayAudio(bounceSound);
                EscudoPlayer player = c.GetComponent<EscudoPlayer>();
                float extLeft = (c.bounds.center.x - c.bounds.extents.x / 2);
                float extRight = (c.bounds.center.x + c.bounds.extents.x / 2);

                if (c.bounds.center.x + sweetSpot / 2 > transform.position.x && c.bounds.center.x - sweetSpot / 2 < transform.position.x)
                {
                    angle = 0;
                }
                else
                {
                    if (Mathf.Abs(extLeft - transform.position.x) < Mathf.Abs(extRight - transform.position.x))
                    {
                        angle = angleAmp * Mathf.Abs((extLeft - transform.position.x)) * -1;
                    }
                    else
                    {
                        angle = angleAmp * Mathf.Abs((extRight - transform.position.x));
                    }
                    angle += player.speed;
                }
                if (transform.position.x > 5)
                {
                    angle = Mathf.Clamp(angle, float.MinValue, 0);
                }
                if (transform.position.x < -5)
                {
                    angle = Mathf.Clamp(angle, 0, float.MaxValue);
                }
                gm.cpuNave.ballAngle = angle;
                UpdateSpeed(1);
                break;
            case "CPU":
                GameManager.instance.PlayAudio(breakSound);
                //puntaje
                GameManager.instance.AddToScore(score);
                //num rebotes
                gm.bounces++;
                if (gm.bounces >= gm.bounceTarget)
                {
                    switch (gm.dificultad)
                    {
                        case Dificultad.Nivel.facil:
                            gm.dificultad = Dificultad.Nivel.medio;
                            break;
                        case Dificultad.Nivel.medio:
                            gm.dificultad = Dificultad.Nivel.dificil;
                            break;
                        default:
                            break;
                    }
                    gm.SetCPUStation();
                    GameManager.instance.FinishLevel();
                    Destroy(this.gameObject);
                }
                else
                {
                    float cpuPos = gm.cpuNave.transform.position.x;
                    float playerPos = gm.player.transform.position.x;
                    float target = 0;

                    float rangeLeft = -(limit - 1) - playerPos;
                    float rangeRight = (limit - 1) - playerPos;

                    float probLeft = Mathf.Abs((rangeLeft * 100) / (2 * limit));
                    r = random.Next(0, 99);
                    //Debug.Log(probLeft +" < "+r);
                    if (r <= probLeft)
                    {
                        int aux = Mathf.CeilToInt(playerPos) + Mathf.FloorToInt(rangeLeft / 2f);
                        r = random.Next(-(limit - 1), aux);
                        target = r - cpuPos;
                    }
                    else
                    {
                        int aux = Mathf.FloorToInt(playerPos) + Mathf.CeilToInt(rangeRight / 2f);
                        r = random.Next(aux, (limit - 1));
                        target = r - cpuPos;
                    }

                    //Debug.Log(target);
                    angle = target * fixAngle;
                    UpdateSpeed(-1);
                }
                break;
            case "Block":
                Destroy(c.gameObject);
                break;
            case "Wall":
                angle *= -1;
                break;
            case "StationPlayer":
                //gm.InitBall();
                gm.PlayerHit();
                Destroy(this.gameObject);
                break;
            case "StationCPU:":
                UpdateSpeed(-1);
                break;
        }
    }

    void OnDestroy()
    {
        GameManager.instance.PlayAudio(breakSound);
    }
}
