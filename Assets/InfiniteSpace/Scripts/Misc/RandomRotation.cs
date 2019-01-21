using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour {
    //The rotation speed of the object
    public float rotationSpeedMax = 35f;
    float rotationSpeed;
    //Set whether the rotation speed should be randomized or not
    public bool randomize = true;
    //check if you want that the object rotate over time
    public bool rotate = true;
    public enum RotationAxis { X, Y, Z};
    public RotationAxis rotationAxis = RotationAxis.Z;

	void Start () {
        Generate();
    }

    //Assign a new speed for the rotation
    public void Generate()
    {
        if (randomize)
        {
            //Select randomly if the rotation is clockwise or counterclockwise and assign a random speed
            rotationSpeed = (Random.Range(0, 100) < 50 ? -1f : 1f) * Random.Range(0f, rotationSpeedMax);
            transform.Rotate(new Vector3(rotationAxis == RotationAxis.X ? Random.Range(0f, 360f) : 0f, rotationAxis == RotationAxis.Y ? Random.Range(0f, 360f) : 0f, rotationAxis == RotationAxis.Z ? Random.Range(0f, 360f) : 0f));
        }
        else
        {
            rotationSpeed = rotationSpeedMax;
        }
    }
	
	void Update () {
        if(rotate)
            transform.Rotate(new Vector3(rotationAxis == RotationAxis.X?1f:0f, rotationAxis == RotationAxis.Y ? 1f : 0f, rotationAxis == RotationAxis.Z ? 1f : 0f) * rotationSpeed * Time.deltaTime);
    }
}
