using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotoObstaculo : MonoBehaviour
{
    public Transform pilarT, pilarB;

    // Use this for initialization
    void Start()
    {
        Random.InitState(gameObject.GetInstanceID());
    }

    public void InicializarPilares(Vector2 rango)
    {
        if (pilarT != null && pilarB != null)
        {
            pilarT.localPosition = new Vector3(pilarT.localPosition.x, Random.Range(rango.x, rango.y), pilarT.localPosition.z);
            pilarB.localPosition = new Vector3(pilarB.localPosition.x, -Random.Range(rango.x, rango.y), pilarB.localPosition.z);
        }
        transform.position = new Vector3(transform.position.x, Random.Range(-4, 4), transform.position.z);
    }
}
