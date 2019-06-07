using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestCami : MonoBehaviour
{
    public static TestCami instance = null;

    nuitrack.JointType[] typeJoint;
    GameObject[] CreatedJoint;
    public GameObject objectToMoveHandR;
    public GameObject objectToMoveElbowR;
    public GameObject objectToMoveHandL;
    public GameObject objectToMoveElbowL;
    public GameObject objectToMoveTorso;
    public GameObject objectToMoveHead;
    public GameObject objectToMoveFootR;
    public GameObject objectToMoveKneeR;
    public GameObject objectToMoveHip;
    public GameObject objectToMoveKneeL;
    public GameObject objectToMoveFootL;
    public GameObject objectToMoveHandR1;
    public GameObject objectToMoveElbowR1;
    public bool lockMovX = false;
    public bool lockMovY = false;
    public bool lockMovZ = false;
    public float scaleX = 0.001f;
    public float scaleY = 0.001f;
    public float scaleZ = 0.001f;
    private nuitrack.JointType torsoJoint, shoulderJoint, elbowJoint;
    public bool useTorsoRef = true;

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
        torsoJoint = nuitrack.JointType.Torso;
        shoulderJoint = nuitrack.JointType.None;
        elbowJoint = nuitrack.JointType.None;
        SetHand();
        resetObjectToMove();
    }

    public void SetHand()
    {
        //if (PlayerPrefs.GetInt("handSelected", 1) == 1)
        //{
        typeJoint = new nuitrack.JointType[13];
        typeJoint[0] = nuitrack.JointType.RightHand;
        typeJoint[1] = nuitrack.JointType.RightElbow;
        typeJoint[2] = nuitrack.JointType.Torso;
        typeJoint[3] = nuitrack.JointType.LeftElbow;
        typeJoint[4] = nuitrack.JointType.LeftHand;
        typeJoint[5] = nuitrack.JointType.Head;
        typeJoint[6] = nuitrack.JointType.RightFoot;
        typeJoint[7] = nuitrack.JointType.RightKnee;
        typeJoint[8] = nuitrack.JointType.Waist;
        typeJoint[9] = nuitrack.JointType.LeftKnee;
        typeJoint[10] = nuitrack.JointType.LeftFoot;
        typeJoint[11] = nuitrack.JointType.RightShoulder;
        typeJoint[12] = nuitrack.JointType.RightFingertip;

        //}
        //else
        //{
        //    if (PlayerPrefs.GetInt("handSelected", 1) == -1)
        //    {
        //        typeJoint = new nuitrack.JointType[1];
        //        typeJoint[0] = nuitrack.JointType.LeftHand;
        //        shoulderJoint = nuitrack.JointType.LeftShoulder;
        //        elbowJoint = nuitrack.JointType.LeftElbow;
        //    }
        //    else
        //    {
        //        typeJoint = new nuitrack.JointType[2];
        //        typeJoint[0] = nuitrack.JointType.LeftHand;
        //        typeJoint[1] = nuitrack.JointType.RightHand;
        //    }
        //}
    }

    public void resetObjectToMove()
    {
        CreatedJoint = new GameObject[typeJoint.Length];
        /*for (int q = 0; q < typeJoint.Length; q++)
        {
            CreatedJoint[q] = objectToMove;
        }*/
        CreatedJoint[0] = objectToMoveHandR;
        CreatedJoint[1] = objectToMoveElbowR;
        CreatedJoint[2] = objectToMoveTorso;
        CreatedJoint[3] = objectToMoveElbowL;
        CreatedJoint[4] = objectToMoveHandL;
        CreatedJoint[5] = objectToMoveHead;
        CreatedJoint[6] = objectToMoveFootR;
        CreatedJoint[7] = objectToMoveKneeR;
        CreatedJoint[8] = objectToMoveHip;
        CreatedJoint[9] = objectToMoveKneeL;
        CreatedJoint[10] = objectToMoveFootL;
        CreatedJoint[11] = objectToMoveHandR1;
        CreatedJoint[12] = objectToMoveElbowR1;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentUserTracker.CurrentUser != 0)
        {
            nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;
            Vector3 reference = Vector3.zero;
            if (shoulderJoint != nuitrack.JointType.None)
                reference.x = skeleton.GetJoint(shoulderJoint).ToVector3().x;
            if (shoulderJoint != nuitrack.JointType.None)
                reference.y = skeleton.GetJoint(elbowJoint).ToVector3().y;
            reference.z = skeleton.GetJoint(torsoJoint).ToVector3().z;

            for (int q = 0; q < typeJoint.Length; q++)
            {
                nuitrack.Joint joint = skeleton.GetJoint(typeJoint[q]);
                float componentX = -scaleX * (joint.ToVector3().x - reference.x) * (lockMovX ? 0f : 1f);
                float componentY = scaleY * (joint.ToVector3().y - reference.y) * (lockMovY ? 0f : 1f);
                float componentZ = scaleZ * (useTorsoRef ? reference.z - joint.ToVector3().z : joint.ToVector3().z) * (lockMovZ ? 0f : 1f);
                Vector3 newPosition = new Vector3(componentX, componentY, componentZ);

                if (q == 0 || q == 1 || q == 11)
                {
                    CreatedJoint[q].transform.position = newPosition;
                    CreatedJoint[q].transform.rotation = joint.ToQuaternion();
                }
            }

        }
    }
}
