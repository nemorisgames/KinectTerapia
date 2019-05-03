using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine.Utility;

public class RobotFuerteGM : MonoBehaviour
{
    public static RobotFuerteGM Instance;
    public Transform brazo;
    public Transform pivoteBrazo;
    public BoxCollider hitbox;
    public GameObject esferaPrefab;
    public Cinemachine.CinemachineVirtualCamera baseCam, moveCam;
    public Dificultad.Nivel dificultad;
    private Vector3 lastMousePos;
    private Vector3 mousePos;
    private List<Vector3> trackedMovement;
    public float armPos;
    public float ajusteY = 0.75f;
    public float ajusteFuerza = 0.75f;
    public float fuerza;
    private Vector3 rotation = new Vector3(0, -30, 0);
    public RobotSphere target;
    public Transform[] triggerAltura;
    public float height = 1;
    float baseHeight = 1.5f;
    public float heightMult = 3;
    public AudioClip hitSound;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
    void Start()
    {
        trackedMovement = new List<Vector3>();
        moveCam.Priority = -11;
        triggerAltura[0].position = new Vector3(triggerAltura[0].position.x, baseHeight + 5 * heightMult, triggerAltura[0].position.z);
        triggerAltura[1].position = new Vector3(triggerAltura[1].position.x, baseHeight + 10 * heightMult, triggerAltura[1].position.z);
        triggerAltura[2].position = new Vector3(triggerAltura[2].position.x, baseHeight + 15 * heightMult, triggerAltura[2].position.z);
    }

    public void Init()
    {
        brazo.gameObject.SetActive(true);
        hitbox.enabled = true;
        target = ((GameObject)Instantiate(esferaPrefab, esferaPrefab.transform.position, esferaPrefab.transform.rotation)).GetComponent<RobotSphere>();
        moveCam.Follow = target.transform;
        moveCam.Priority = 11;
        switch (dificultad)
        {
            case Dificultad.Nivel.facil:
                target.Setup(triggerAltura[0].position.y);
                ajusteFuerza = 5f;
                break;
            case Dificultad.Nivel.medio:
                target.Setup(triggerAltura[1].position.y);
                ajusteFuerza = 5.8f;
                break;
            case Dificultad.Nivel.dificil:
                target.Setup(triggerAltura[2].position.y);
                ajusteFuerza = 6.8f;
                break;
        }
    }

    public void Restart()
    {
        if (target != null)
            Destroy(target.gameObject);
        moveCam.transform.position = baseCam.transform.position;
        moveCam.Priority = -11;
        if (dificultad == Dificultad.Nivel.facil)
            dificultad = Dificultad.Nivel.medio;
        else if (dificultad == Dificultad.Nivel.medio)
            dificultad = Dificultad.Nivel.dificil;
        Init();
    }

    public void Hit(Rigidbody rb)
    {
        GameManager.instance.PlayAudio(hitSound);
        StopMoving();
        fuerza = Mathf.Clamp(fuerza, 0, 4);
        Debug.Log(fuerza);
        rb.AddForce(new Vector3(0, fuerza * ajusteFuerza, 0), ForceMode.Impulse);
        rb.useGravity = true;
        rb.GetComponent<SphereCollider>().enabled = false;
        hitbox.enabled = false;
    }

    public bool moving = false;
    float[] moveDist = new float[2];

    void Update()
    {
        if (Time.timeScale == 0)
            return;
        mousePos = Input.mousePosition;
        /* if(lastMousePos != null && Vector3.Distance(mousePos,lastMousePos) > 0.1f){
			trackedMovement.Add(mousePos);
		}
		else if(!stopping){
			StartCoroutine(stopDelay());
		}*/
        if (!moving && Vector3.Distance(mousePos, lastMousePos) > 0.05f)
        {
            moving = true;
            moveDist[0] = armPos;
        }
        if (moving && Vector3.Distance(mousePos, lastMousePos) < 0.05f)
        {
            StartCoroutine(stopDelay());
        }


        armPos = (mousePos.y / Screen.height) - ajusteY;
        /*if(armPos <= 1){
			rotation.x = (armPos * 90) / -1;
			brazo.eulerAngles = rotation;
		}*/
        Vector3 handPosition = Input.mousePosition - new Vector3(Screen.height / 2f, 0f, 0f);
        float horizontalViewportPosition = Camera.main.ScreenToViewportPoint(handPosition).y;
        float horizontalPosition = horizontalViewportPosition * height - 1;
        brazo.transform.position = new Vector3(0, horizontalPosition, brazo.position.z);
        if (moveCam.transform.position.y > 2 && brazo.gameObject.activeSelf)
            brazo.gameObject.SetActive(false);


        lastMousePos = mousePos;
        //fuerza = trackedMovement.Count;
        pivoteBrazo.LookAt(brazo, Vector3.up);

        if (target != null && !target.launched && brazo.position.y >= target.transform.position.y)
        {
            target.HitSphere();
        }

    }

    IEnumerator stopDelay()
    {
        yield return new WaitForSeconds(0.1f);
        if (moving && Vector3.Distance(mousePos, lastMousePos) < 0.05f)
            StopMoving();
    }

    public void StopMoving()
    {
        moving = false;
        moveDist[1] = armPos;
        fuerza = (moveDist[1] - moveDist[0]) * 10;
    }

    public void LevelWon()
    {
        moveCam.Follow = null;
        GameManager.instance.FinishLevel();
    }

    public void LevelFailed()
    {
        moveCam.Follow = null;
        GameManager.instance.LevelFailed();
    }

}
