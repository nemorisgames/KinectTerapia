using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Heatmap : MonoBehaviour
{
    public static Heatmap Instance;
    public List<Vector4> positions = new List<Vector4> ();
    public List<Vector4> properties = new List<Vector4> ();
    private List<Vector4> positionsZ = new List<Vector4> ();
    private List<Vector4> propertiesZ = new List<Vector4> ();
    public Transform quadZ;
    public Material material;
    public Material materialZ;
    public int count = 50;
    public bool randomize = false;
    private bool autoStart = false;
    public RenderTexture rt;

    void Awake ()
    {
        if (Instance == null)
            Instance = this;
    }

    public string Generate (List<Vector4> points = null)
    {
        if (randomize)
        {

            for (int i = 0; i < count; i++)
            {
                positions.Add (new Vector4 (Random.Range (-8f, +8f), Random.Range (-6f, +6f), 0, 0));
                properties.Add (new Vector4 (1f, 1f, 0, 0));
            }

            return "";
        }
        else
        {
            if (points != null)
            {
                count = points.Count;
                for (int i = 0; i < count; i++)
                {
                    positions.Add (new Vector4 (transform.position.x + points[i].x, transform.position.y + points[i].y, 0, 0));
                    properties.Add (new Vector4 (0.8f, 0.8f, 0, 0));

                    if (quadZ != null && materialZ != null)
                    {
                        positionsZ.Add (new Vector4 (quadZ.position.x, quadZ.position.y + points[i].z, 0, 0));
                        propertiesZ.Add (new Vector4 (0.8f, 0.8f, 0, 0));
                    }
                }
            }
            else
            {
                /* 
                string hm = PlayerPrefs.GetString("currentHeatmap");
                string [] hm_points = hm.Split('|');
                count = hm_points.Length;
                for(int i = 0; i < count; i++){
                    //Debug.Log(hm_points[i]);
                    string [] point = hm_points[i].Split(',');
                    //Debug.Log(point.Length);
                    positions.Add(new Vector4(float.Parse(point[0]) + transform.position.x,float.Parse(point[1]) + transform.position.y,0,0));
                    properties.Add(new Vector4(1f,1f,0,0));
                }
                */
            }

            material.SetInt ("_Points_Length", count);
            material.SetVectorArray ("_Points", positions);
            material.SetVectorArray ("_Properties", properties);

            if (quadZ != null && materialZ != null)
            {
                materialZ.SetInt ("_Points_Length", count);
                materialZ.SetVectorArray ("_Points", positionsZ);
                materialZ.SetVectorArray ("_Properties", propertiesZ);
            }

            if (!Directory.Exists (Path.Combine (Application.persistentDataPath, "Heatmap")))
            {
                Directory.CreateDirectory (Path.Combine (Application.persistentDataPath, "Heatmap"));
            }
            string fileName = "Heatmap/" + PlayerPrefs.GetInt ("pk_patient", 0) + "_" + SceneManager.GetActiveScene ().name.Substring (4, 1) + "_" + System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year + "_" + System.DateTime.Now.Hour + "-" + System.DateTime.Now.Minute + ".png";
            string path = Path.Combine (Application.persistentDataPath, fileName);
            PlayerPrefs.SetString ("currentHeatmap", path);

            StartCoroutine (SavePicture (path));

            //Debug.Log (path);

            return path;
        }
    }

    void Start ()
    {
        /*if(autoStart)
            Init();
        */
    }

    IEnumerator SavePicture (string path)
    {
        yield return new WaitForEndOfFrame ();
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D (rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
        byte[] bytes;
        bytes = tex.EncodeToPNG ();
        System.IO.File.WriteAllBytes (path, bytes);
    }
}