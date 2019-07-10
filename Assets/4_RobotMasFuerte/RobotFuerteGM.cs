using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.UI;

public class RobotFuerteGM : MonoBehaviour
{
    public static RobotFuerteGM Instance;
    public Transform brazo;
    public Transform pivoteBrazo;
    public GameObject esferaPrefab;
    public Cinemachine.CinemachineVirtualCamera baseCam, moveCam;
    public Dificultad.Nivel dificultad;
    public Vector3 lastMousePos;
    private Vector3 mousePos;
    public List<Vector3> trackedMovement;
    public float ajusteFuerza = 0.75f;
    public float fuerza;
    public RobotSphere target;
    public Transform[] triggerAltura;
    float baseHeight = 1.5f;
    public float heightMult = 3;
    public AudioClip hitSound;
    private bool gameStarted = false;
    void Awake ()
    {
        if (Instance == null)
            Instance = this;

    }
    void Start ()
    {
        trackedMovement = new List<Vector3> ();
        moveCam.Priority = -11;
        triggerAltura[0].position = new Vector3 (triggerAltura[0].position.x, baseHeight + 5 * heightMult, triggerAltura[0].position.z);
        triggerAltura[1].position = new Vector3 (triggerAltura[1].position.x, baseHeight + 10 * heightMult, triggerAltura[1].position.z);
        triggerAltura[2].position = new Vector3 (triggerAltura[2].position.x, baseHeight + 15 * heightMult, triggerAltura[2].position.z);
    }

    public void Init ()
    {
        gameStarted = false;
        target = null;
    }

    public void Restart ()
    {
        if (target != null)
            Destroy (target.gameObject);
        moveCam.transform.position = baseCam.transform.position;
        moveCam.Priority = -11;
        if (dificultad == Dificultad.Nivel.facil)
            dificultad = Dificultad.Nivel.medio;
        else if (dificultad == Dificultad.Nivel.medio)
            dificultad = Dificultad.Nivel.dificil;
        Init ();
    }

    public void Hit (Rigidbody rb)
    {
        GameManager.instance.PlayAudio (hitSound);
        StopMoving ();
        fuerza = Mathf.Clamp (fuerza, 0, 4f);
        //Debug.Log(fuerza);
        rb.AddForce (new Vector3 (0, fuerza * ajusteFuerza, 0), ForceMode.Impulse);
        rb.useGravity = true;
        rb.GetComponent<SphereCollider> ().enabled = false;
    }

    public bool moving = false;
    IEnumerator movingCoroutine;

    IEnumerator StartMoving ()
    {
        moving = true;
        trackedMovement.Clear ();
        trackedMovement.Add (lastMousePos);
        int count = trackedMovement.Count;
        yield return new WaitForSeconds (0.1f);
        while (Vector3.Distance (lastMousePos, trackedMovement[Mathf.Clamp (count - 1, 0, int.MaxValue)]) > 0.1f)
        {
            trackedMovement.Add (lastMousePos);
            count = trackedMovement.Count;
            yield return new WaitForSeconds (0.1f);
        }
        StopMoving ();
    }

    void Update ()
    {
        if (Time.timeScale == 0)
            return;
        mousePos = brazo.position;

        if (!moving && lastMousePos != null && Vector3.Distance (mousePos, lastMousePos) > 0.1f)
        {
            movingCoroutine = StartMoving ();
            StartCoroutine (movingCoroutine);
        }
        lastMousePos = mousePos;
        pivoteBrazo.LookAt (brazo, Vector3.up);

        if (target != null && !target.launched && brazo.position.y >= target.transform.position.y)
        {
            target.HitSphere ();
        }

        if (!gameStarted && brazo.position.y <= -1.5f)
        {
            StartCoroutine (startBall ());
        }
    }

    IEnumerator startBall ()
    {
        gameStarted = true;
        yield return new WaitForSeconds (1f);
        target = ((GameObject) Instantiate (esferaPrefab, esferaPrefab.transform.position, esferaPrefab.transform.rotation)).GetComponent<RobotSphere> ();
        moveCam.Follow = target.transform;
        moveCam.Priority = 11;
        switch (dificultad)
        {
            case Dificultad.Nivel.facil:
                target.Setup (triggerAltura[0].position.y);
                ajusteFuerza = 5f;
                break;
            case Dificultad.Nivel.medio:
                target.Setup (triggerAltura[1].position.y);
                ajusteFuerza = 6.2f;
                break;
            case Dificultad.Nivel.dificil:
                target.Setup (triggerAltura[2].position.y);
                ajusteFuerza = 7.2f;
                break;
        }
    }

    IEnumerator stopDelay ()
    {
        yield return new WaitForSeconds (0.1f);
        if (moving && Vector3.Distance (mousePos, lastMousePos) < 0.05f)
            StopMoving ();
    }

    public void StopMoving ()
    {
        if (movingCoroutine != null)
        {
            StopCoroutine (movingCoroutine);
        }
        moving = false;
        fuerza = (trackedMovement[Mathf.Clamp (trackedMovement.Count - 1, 0, int.MaxValue)].y - trackedMovement[0].y) * 2.5f;
    }

    public void LevelWon ()
    {
        moveCam.Follow = null;
        GameManager.instance.FinishLevel ();
    }

    public void LevelFailed ()
    {
        moveCam.Follow = null;
        GameManager.instance.LevelFailed ();
    }

}