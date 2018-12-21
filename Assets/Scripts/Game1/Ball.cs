using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public float speed = 10f;
    public Vector3 direction;
    Rigidbody rigidbody;
	// Use this for initialization
	void Start () {
        direction = direction.normalized;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = (collision.contacts[0].normal);
        if (collision.gameObject.CompareTag("Block"))
        {
            collision.gameObject.SendMessage("BlockHit");
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            float offsetX = collision.contacts[0].point.x - collision.transform.position.x;
            direction = new Vector3(direction.x + offsetX * 0.5f, -direction.y, direction.z);
            direction.Normalize();
        }
        else
        {
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
    // Update is called once per frame
    void LateUpdate () {
        //transform.position = transform.position + direction * speed * Time.deltaTime;
        rigidbody.velocity = direction * speed * Time.deltaTime;
    }
}
