using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour {
    //Time that takes in activate the shooting star after generation
    [Range(0f, 30.0f)]
    public float spawnTime = 4f;
    float currentSpawnTime;

    //This speed value to move the shooting star
    [Range(0.01f, 1.0f)]
    public float speed = 1f;
    float currentSpeed;

    //If not activated, the shooting star does not move
    bool activated = false;

    //Define if the spawnTime and the speed should be randomized or not at generation
    public bool randomize = true;

    ScrollDirection direction = ScrollDirection.LeftToRight;
    void Start ()
    {
        if(SpaceManager.instance != null)
        {
            direction = SpaceManager.instance.scrollDirection;
        }
        if (!randomize)
        {
            currentSpawnTime = spawnTime;
            currentSpeed = speed;
        }
        Generate();
	}
	
    public void Generate()
    {
        //Deactivate the shoting star
        Activate(false);
        //Randomize spawn time and speed
        if (randomize)
        {
            currentSpawnTime = Random.Range(0.3f, spawnTime);
            currentSpeed = Random.Range(0.05f, speed);
        }
        //Wait for currentSpawnTime to reactivate the shooting star
        StartCoroutine(waitToActivate(currentSpawnTime));
    }

    IEnumerator waitToActivate(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //After waiting, activate the shooting star
        Activate(true);
    }

    //Activate or deactivate the shooting star movement
    public void Activate(bool activate)
    {
        activated = activate;
        if (activated)
        {
            //Once activated, the first action is to give the shooting star a new position
            Vector3 newPosition = Vector3.zero;

            switch (direction)
            {
                case ScrollDirection.LeftToRight:
                        newPosition = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1.5f), (Random.Range(0, 100) < 50 ? -0.5f : 1.5f), 0f));
                    break;
                case ScrollDirection.RightToLeft:
                        newPosition = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(-0.5f, 1f), (Random.Range(0, 100) < 50 ? -0.5f : 1.5f), 0f));
                    break;
                case ScrollDirection.DownToUp:
                        newPosition = Camera.main.ViewportToWorldPoint(new Vector3((Random.Range(0, 100) < 50 ? -0.5f : 1.5f), Random.Range(0f, 1.5f), 0f));
                    break;
                case ScrollDirection.UpToDown:
                        newPosition = Camera.main.ViewportToWorldPoint(new Vector3((Random.Range(0, 100) < 50 ? -0.5f : 1.5f), Random.Range(-0.5f, 1f), 0f));
                    break;
            }
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            //It defines the point to the shooting star will be pointing
            Vector3 forwardDirection = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f) - transform.position;
            //Force the forwardDirection to don't change the position in the z axis
            transform.forward = new Vector3(forwardDirection.x, forwardDirection.y, 0f);
        }
    }
    
	void Update () {
        //If is not activated, don't update
        if (!activated) return;
        transform.position += transform.forward * currentSpeed;
        //Ask if the shooting star has reached the limit to be regenerated
        Rect rect = new Rect(-1f, -1f, 3f, 3f);
        if (!rect.Contains(Camera.main.WorldToViewportPoint(transform.position)))
        {
            Generate();
        }
    }
}
