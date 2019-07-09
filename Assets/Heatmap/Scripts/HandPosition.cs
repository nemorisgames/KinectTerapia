using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPosition : MonoBehaviour
{
    public static HandPosition Instance;
    List<Vector2> savedPos = new List<Vector2> ();
    List<Vector4> points = new List<Vector4> ();
    public float tiempoMuestreo = 0.5f;
    public List<float> velocidades = new List<float> ();
    private int index = 0;

    // Use this for initialization
    void Awake ()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start ()
    {
        index = 0;
        StartCoroutine (NextPosition ());
    }

    IEnumerator NextPosition ()
    {
        yield return new WaitForSeconds (tiempoMuestreo);
        Vector4 position = new Vector4 (transform.position.x, transform.position.y, transform.position.z, 0);
        position.x = (Mathf.Round (position.x * 10)) / 10f;
        position.y = (Mathf.Round (position.y * 10)) / 10f;
        position.z = (Mathf.Round (position.z * 10)) / 10f;
        //Debug.Log("saved " + position.x + "," + position.y + "," + position.z);
        points.Add (position);
        index++;
        if (points.Count > 2)
        {
            float v2 = points[index - 1].sqrMagnitude;
            float v1 = points[index - 2].sqrMagnitude;
            if (v2 - v1 != 0)
            {
                velocidades.Add (v2 - v1);
            }
        }
        if (Time.timeScale != 0)
            StartCoroutine (NextPosition ());
    }

    public string SavePoints ()
    {
        return Heatmap.Instance.Generate (points);
    }

    public float MaxVel ()
    {
        float max = 0;
        foreach (float f in velocidades)
        {
            if (f > max)
                max = f;
        }
        return max;
    }

    public float MeanVel ()
    {
        float mean = 0;
        foreach (float f in velocidades)
        {
            mean += f;
        }
        return (mean / velocidades.Count);
    }
}