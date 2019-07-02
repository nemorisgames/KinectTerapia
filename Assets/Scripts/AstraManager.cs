using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstraManager : MonoBehaviour
{
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
    private nuitrack.JointType torsoJoint, shoulderJoint, elbowJoint;
    public bool useTorsoRef = true;
    public Vector3 offset = Vector3.zero;

    void Start ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != null)
            {
                Destroy (gameObject);
            }
        }
        torsoJoint = nuitrack.JointType.Torso;
        shoulderJoint = nuitrack.JointType.None;
        elbowJoint = nuitrack.JointType.None;
        SetHand ();
        resetObjectToMove ();
    }

    public void SetHand ()
    {
        if (PlayerPrefs.GetInt ("handSelected", 1) == 1)
        {
            typeJoint = new nuitrack.JointType[1];
            typeJoint[0] = nuitrack.JointType.RightHand;
            shoulderJoint = nuitrack.JointType.RightShoulder;
            elbowJoint = nuitrack.JointType.RightElbow;
        }
        else
        {
            if (PlayerPrefs.GetInt ("handSelected", 1) == -1)
            {
                typeJoint = new nuitrack.JointType[1];
                typeJoint[0] = nuitrack.JointType.LeftHand;
                shoulderJoint = nuitrack.JointType.LeftShoulder;
                elbowJoint = nuitrack.JointType.LeftElbow;
            }
            else
            {
                typeJoint = new nuitrack.JointType[2];
                typeJoint[0] = nuitrack.JointType.LeftHand;
                typeJoint[1] = nuitrack.JointType.RightHand;
            }
        }
    }

    public void resetObjectToMove ()
    {
        CreatedJoint = new GameObject[typeJoint.Length];
        for (int q = 0; q < typeJoint.Length; q++)
        {
            CreatedJoint[q] = objectToMove;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (CurrentUserTracker.CurrentUser != 0)
        {
            nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;
            //Vector3 sh = (shoulderJoint != nuitrack.JointType.None ? skeleton.GetJoint(shoulderJoint).ToVector3() : Vector3.zero);
            Vector3 reference = Vector3.zero;
            if (shoulderJoint != nuitrack.JointType.None)
                reference.x = skeleton.GetJoint (shoulderJoint).ToVector3 ().x;
            if (shoulderJoint != nuitrack.JointType.None)
                reference.y = skeleton.GetJoint (elbowJoint).ToVector3 ().y;
            reference.z = skeleton.GetJoint (torsoJoint).ToVector3 ().z;

            for (int q = 0; q < typeJoint.Length; q++)
            {
                nuitrack.Joint joint = skeleton.GetJoint (typeJoint[q]);
                float componentX = -scaleX * (joint.ToVector3 ().x - reference.x) * (lockMovX ? 0f : 1f);
                float componentY = scaleY * (joint.ToVector3 ().y - reference.y) * (lockMovY ? 0f : 1f);
                float componentZ = scaleZ * (useTorsoRef ? reference.z - joint.ToVector3 ().z : joint.ToVector3 ().z) * (lockMovZ ? 0f : 1f);
                Vector3 newPosition = new Vector3 (componentX, componentY, componentZ);

                CreatedJoint[q].transform.localPosition = newPosition + offset;
            }

        }
    }
}