using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotoCheckpoint : MonoBehaviour
{
    public bool passed = false;
    public bool final = false;
    public MeshRenderer[] luces;
    public Material matGreen, matBlue;

    void Start()
    {
        if (final)
        {
            foreach (MeshRenderer mr in luces)
                mr.material = matGreen;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!final && !passed && other.tag == "Player")
        {
            passed = true;
            foreach (MeshRenderer mr in luces)
                mr.material = matBlue;
            GameManager.instance.AddToScore(PilotoGM.Instance.score);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (final && !passed && other.tag == "Player")
        {
            if (other.transform.position.x < transform.position.x)
                return;
            passed = true;
            GameManager.instance.FinishLevel();
        }
    }
}
