using UnityEngine;
using System.Collections;

public class Alien : MonoBehaviour {
    public Animator alien;
    private IEnumerator coroutine;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            alien.SetBool("idle", true);
            alien.SetBool("walking", false);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            alien.SetBool("walking", true);
            alien.SetBool("idle", false);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            alien.SetBool("walking", false);
            alien.SetBool("running", true);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            alien.SetBool("running", false);
            alien.SetBool("jumping", true);
            alien.SetBool("idle", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            alien.SetBool("jumping", false);
            alien.SetBool("attack", true);
            alien.SetBool("idle", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            alien.SetBool("attack", false);
            alien.SetBool("hited", true);
            alien.SetBool("idle", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            alien.SetBool("hited", false);
            alien.SetBool("death", true);
            alien.SetBool("idle", false);
        }
    }
    IEnumerator idle()
    {
        yield return new WaitForSeconds(0.1f);
        alien.SetBool("attack", false);
        alien.SetBool("hited", false);
        alien.SetBool("idle", true);
        alien.SetBool("jumping", false);
        alien.SetBool("attack", false);
    }
}
