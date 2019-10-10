using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform playerBar;
    public int tries = 3;
    public UILabel triesLabel;
    public float speed = 10f;
    public Vector3 direction;
    Rigidbody rigidbody;
    Vector3 initialDirection;
    Vector3 initialPosition;
    public AudioClip wallBounce, blockBounce;
    // Use this for initialization
    void Start()
    {
        direction = direction.normalized;
        initialDirection = direction;
        initialPosition = transform.position;
        rigidbody = GetComponent<Rigidbody>();
        tries = 3;
        triesLabel.text = "Intentos: "+tries;
    }

    public void resetTries(){
        tries = 3;
        triesLabel.text = "Intentos: "+tries;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = (collision.contacts[0].normal);
        if (collision.gameObject.CompareTag("Block"))
        {
            GameManager.instance.PlayAudio(blockBounce);
            collision.gameObject.SendMessage("BlockHit");
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PlayAudio(wallBounce);
            float offsetX = collision.contacts[0].point.x - collision.transform.position.x;
            direction = new Vector3(direction.x + offsetX * 0.5f, -direction.y, direction.z);
            direction.Normalize();

        }
        else
        {
            GameManager.instance.PlayAudio(wallBounce);
            if (normal == Vector3.right)
            {
                direction = new Vector3(-direction.x, direction.y, direction.z);
            }
            else
            {
                if (normal == Vector3.left)
                {
                    direction = new Vector3(-direction.x, direction.y, direction.z);
                }
                else
                {
                    if (normal == Vector3.down)
                    {
                        direction = new Vector3(direction.x, -direction.y, direction.z);
                    }
                    else
                    {
                        direction = new Vector3(direction.x, -direction.y, direction.z);
                    }
                }
            }
        }
    }

    public void ResetBall()
    {
        transform.position = initialPosition;
        direction = initialDirection;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position = transform.position + direction * speed * Time.deltaTime;
        rigidbody.velocity = direction * speed * Time.deltaTime;
        if (transform.position.y < -2.5f)
        {
            tries--;
            triesLabel.text = "Intentos: "+tries;
            if(tries <= 0){
                GameManager.instance.LevelFailed();
            }
            else{
                transform.position = new Vector3(playerBar.position.x,initialPosition.y,initialPosition.z);
                direction = initialDirection;
            }
            
        }
    }
}
