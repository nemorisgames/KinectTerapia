using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBlinking : MonoBehaviour
{
    MeshRenderer meshRenderer;
    float timeChange = 0.5f;
    
    void Start () {
        //Select the mode and the correspondant renderer
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    IEnumerator changeStarForce()
    {
        //Make the material blink for 0.03 seconds changing the alpha value
        meshRenderer.material.color = new Color(meshRenderer.material.color.r, meshRenderer.material.color.g, meshRenderer.material.color.b, 0.7f);
        yield return new WaitForSeconds(0.03f);
        meshRenderer.material.color = new Color(meshRenderer.material.color.r, meshRenderer.material.color.g, meshRenderer.material.color.b, 1f);
    }

    // Update is called once per frame
    void Update () {
		if(Time.time > timeChange)
        {
            //Randomize the time for the blink
            timeChange = Time.time + Random.Range(0.1f, 2f);
            StartCoroutine(changeStarForce());
        }
	}
}
