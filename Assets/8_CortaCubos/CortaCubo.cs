using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CortaCubo : MonoBehaviour
{
    private Rigidbody left, right;
    private BoxCollider leftCol, rightCol;
    public float breakForce = 10f;
    public bool broken = false;
    public float startPos = 30;
    public float speed = 2f;
    public bool canBreak = true;

    // Use this for initialization
    void Awake()
    {
        left = transform.Find("L").GetComponent<Rigidbody>();
        right = transform.Find("R").GetComponent<Rigidbody>();
        leftCol = left.GetComponent<BoxCollider>();
        rightCol = left.GetComponent<BoxCollider>();

    }

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, startPos);
        float r = Random.Range(0f, 99f);
        if (r < 25f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (r < 50f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 45);
        }
        else if (r < 75f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 315);
        }
        StartCoroutine(CortaCubosGM.Instance.SpawnCube(CortaCubosGM.Instance.spawnTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (!broken)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
        }

        if (transform.position.z < -9 && canBreak)
            canBreak = false;

        if (transform.position.z <= 2f && canBreak && !broken)
        {
            CortaCubosGM.Instance.cubeInArea = true;
            CortaCubosGM.Instance.CortarCubo(this);
        }

        if (transform.position.z < -11 && !broken)
        {
            StartCoroutine(DeletThis(0));
            broken = true;
            CortaCubosGM.Instance.UpdateFallos(CortaCubosGM.Instance.fallos + 1);
        }
    }

    public void Break()
    {
        if (broken)
            return;
        CortaCubosGM.Instance.cubeInArea = false;
        broken = true;
        canBreak = false;
        left.AddRelativeForce(-breakForce, 0, -breakForce / 2, ForceMode.Impulse);
        right.AddRelativeForce(breakForce, 0, -breakForce / 2, ForceMode.Impulse);
        left.useGravity = true;
        right.useGravity = true;
        GameManager.instance.AddToScore(CortaCubosGM.Instance.puntaje);
        StartCoroutine(DeletThis(1.5f));
    }

    IEnumerator DeletThis(float f)
    {
        CortaCubosGM.Instance.cubeInArea = false;
        CortaCubosGM.Instance.UpdateRestantes(CortaCubosGM.Instance.cubes - 1);
        yield return new WaitForSeconds(f);
        if (CortaCubosGM.Instance.cubes <= 0)
            GameManager.instance.FinishLevel();
        Destroy(this.gameObject);
    }
}
