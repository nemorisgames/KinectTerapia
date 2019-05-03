using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotoNave : MonoBehaviour
{
    Rigidbody rb;
    public MeshRenderer mr;
    public bool invincible = false;
    public float knockBack = 1;
    public int direction = 0;
    public float rotSpeed = 50;
    private float lastPos;
    private float rotation;
    public GameObject particles;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator Hit()
    {
        invincible = true;
        Instantiate(particles, transform.position, particles.transform.rotation, transform.parent);
        mr.material.color = Color.red;
        yield return new WaitForSeconds(1f);
        mr.material.color = Color.white;
        invincible = false;
    }

    void OnCollisionEnter(Collision c)
    {
        if (!invincible && c.collider.tag == "CPU")
        {
            PilotoGM.Instance.health--;
            PilotoGM.Instance.SetHealth();
            if (PilotoGM.Instance.health > 0)
                StartCoroutine(Hit());
            else
            {
                Instantiate(particles, transform.position, transform.rotation, transform.parent);
                GameManager.instance.LevelFailed();
                this.gameObject.SetActive(false);
                Time.timeScale = 0;
            }
        }
    }

    void Update()
    {
        float moveDelta = transform.position.y - lastPos;
        if (moveDelta != 0)
            direction = (int)Mathf.Sign(moveDelta);
        else
            direction = 0;

        if (direction != 0)
        {
            rotation = rotation + (rotSpeed * 2 * Time.deltaTime) * direction;
            rotation = Mathf.Clamp(rotation, -45, 45);
        }
        else
        {
            if (rotation < 0)
            {
                rotation = rotation + (rotSpeed * Time.deltaTime);
                rotation = Mathf.Clamp(rotation, -45, 0);
            }
            else
            {
                rotation = rotation + -(rotSpeed * Time.deltaTime);
                rotation = Mathf.Clamp(rotation, 0, 45);
            }
        }

        transform.localEulerAngles = new Vector3(rotation, 0, 0);

        lastPos = transform.position.y;
    }
}
