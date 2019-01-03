using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AstraManager : MonoBehaviour {
    public static AstraManager instance = null;

    nuitrack.JointType[] typeJoint;
    GameObject[] CreatedJoint;
    public GameObject objectToMove;
    public bool lockMovX = false;
    public bool lockMovY = false;
    public bool lockMovZ = false;
    public float scaleX = 0.001f;
    public float scaleY = 0.001f;
    public float scaleZ = 0.001f;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }

        if (PlayerPrefs.GetInt("handSelected", 1) == 1)
        {
            typeJoint = new nuitrack.JointType[1];
            typeJoint[0] = nuitrack.JointType.RightHand;
        }
        else
        {
            if (PlayerPrefs.GetInt("handSelected", 1) == 1)
            {
                typeJoint = new nuitrack.JointType[1];
                typeJoint[0] = nuitrack.JointType.LeftHand;
            }
            else
            {
                typeJoint = new nuitrack.JointType[2];
                typeJoint[0] = nuitrack.JointType.LeftHand;
                typeJoint[1] = nuitrack.JointType.RightHand;
            }
        }

        CreatedJoint = new GameObject[typeJoint.Length];
        for (int q = 0; q < typeJoint.Length; q++)
        {
            CreatedJoint[q] = objectToMove;// Instantiate(PrefabJoint);
            //CreatedJoint[q].transform.SetParent(objectToMove.transform);
        }
    }

    // Update is called once per frame
    void Update () {
        if (CurrentUserTracker.CurrentUser != 0)
        {
            nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;

            for (int q = 0; q < typeJoint.Length; q++)
            {
                nuitrack.Joint joint = skeleton.GetJoint(typeJoint[q]);
                float componentX = -scaleX * joint.ToVector3().x * (lockMovX ? 0f : 1f);
                float componentY = scaleY * joint.ToVector3().y * (lockMovY ? 0f : 1f);
                float componentZ = scaleZ * joint.ToVector3().z * (lockMovZ ? 0f : 1f);
                Vector3 newPosition = new Vector3(componentX, componentY, componentZ);

                CreatedJoint[q].transform.localPosition = newPosition;
            }
        }
    }
}
